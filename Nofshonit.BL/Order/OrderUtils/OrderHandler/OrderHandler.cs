using Microsoft.CodeAnalysis;
using Nofshonit.BL.CreditGuard;
using Nofshonit.BL.Event;
using Nofshonit.BL.Limitations;
using Nofshonit.BL.RestApiGW;
using Nofshonit.Common;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.ResponseDTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using Nofshonit.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TicketsHubRepository;
using static Nofshonit.BL.Cards.CardsBL;
using static Nofshonit.Repositories.Helpers.ProductFunctions;
using Azure.Core;
using Nofshonit.Common.Utils;
using Nofshonit.Logs;
using static CreditServices.Interfaces.ResponseMandatoryProperties;
using CreditServices.Interfaces;
using PaymentAPICommon.APIRequest;
using PaymentsAPI.Models;
using PurchaseHandler;

namespace Nofshonit.BL.Order.OrderUtils.OrderHandler
{
	public class OrderHandler : BaseBL, IOrderHandler
	{
		private readonly IContextManager _contextManager;
		private readonly IClubRepo _clubRepo;
		private readonly IDtsOnlineRepo _dtsOnLineRepo;
		private readonly ILimitationsBL _limitationsBL;
		private readonly IRestApiGW _restApiGW;
		private readonly IConfigurationManager _configuration;
		private readonly ITicketsHubRepo _ticketsHubRepo;
		private readonly IOrderHandler _orderHandler;
		private readonly IEventBL _EventBl;
		private readonly IUserService _userService;
        ICreditGuard _creditGuard;
		public OrderHandler()
		{
			_contextManager = Container.Resolve<IContextManager>();
			_clubRepo = Container.Resolve<IClubRepo>();
			_limitationsBL = Container.Resolve<ILimitationsBL>();
			_restApiGW = Container.Resolve<IRestApiGW>();
			_configuration = Container.Resolve<IConfigurationManager>();
			_ticketsHubRepo = Container.Resolve<ITicketsHubRepo>();
			_creditGuard = Container.Resolve<ICreditGuard>();
			_dtsOnLineRepo = Container.Resolve<IDtsOnlineRepo>();
			_EventBl = Container.Resolve<IEventBL>();
            _userService = Container.Resolve<IUserService>();

        }

		private void ValidateOrderEventBooking(string eventsGuid)
		{
			if (!string.IsNullOrEmpty(eventsGuid))
			{
				DtsLoggger.Logger.Info("PurchaseHandler ValidateOrderEventBooking Checking Event Validation");
				bool isEventValid = _ticketsHubRepo.ValidateTicketHubEventBooked(eventsGuid);
				if (!isEventValid)
				{
					LoggerHelper.Error("PurchaseHandler ValidateOrderEventBooking Failed due to Invalid Event Booking");
					throw new BusinessException("ההזמנה לא נקלטה - רכישת רכטיסים למופע נכשלה");
				}
			}
		}
		private List<OrderVariantDTO> FillResponseVariants(List<ProductsVars> productsVars, PurchaseRequestDTO request, long orderId)
		{
			try
			{
				DtsLoggger.Logger.Info("PurchaseHandler FillResponseData Started");
				if (orderId > 0)
				{
					List<OrderVariantDTO> list = new List<OrderVariantDTO>();
					foreach (var cartItem in request.Cart)
					{
						foreach (var requestVariant in cartItem.Variants)
						{
							var productVar = productsVars.FirstOrDefault(p => p.FullBarCode.Equals(requestVariant.Barcode));
							for (int i = 0; i < requestVariant.Quantity; i++)
							{
								var redimTypeId = productVar.RedimTypeId > 0 ? productVar.RedimTypeId.Value : _dtsOnLineRepo.GetFatherRedimTypeId(cartItem.CategoryId);
								var redimTypeName = productVar.RedimTypeId > 0 ? _dtsOnLineRepo.GetRedimTypeNameById(productVar.RedimTypeId.Value) : _dtsOnLineRepo.GetFatherRedimTypeName(cartItem.CategoryId);

								var orderVariant = new OrderVariantDTO
								{
									VariantName = productVar.VarName,
									Qty = 1,
									VariantBarCode = requestVariant.Barcode,
									MoneyPay = requestVariant.Price,
									CoinsPay = 0,
									RedimTypeId = redimTypeId,
									RedimTypeName = redimTypeName,
									ExpiredDate = productVar.LastImplementationDate.GetValueOrDefault(),
									BenefitTypeId = ProductFunctions.GetBenefitTypeIdByVariantType(productVar.VariantType),
									GiftCardValue = ProductFunctions.GetBenefitTypeIdByVariantType(productVar.VariantType) == (int)BenefitType.GiftCard ? productVar.LoadingAmount : null,
									BusinessSubTypeId = productVar.BusinessSubTypeId.Value,
									IsCoupon = productVar.CuponStockId.HasValue && productVar.CuponStockId.Value != 0
								};
								list.Add(orderVariant);
							}
						}
					}
					return list;
				}
				DtsLoggger.Logger.Info("PurchaseHandler FillResponseData Finished");
				return null;
			}
			catch (Exception e)
			{
				LoggerHelper.Error($"Error in FillResponseVariants, error message: {e.Message}");
				throw;
			}
		}
		private string GetRedimCode(string orderConfirmation, List<v_wAllOrders> allOrders, string barCode)
		{
			var redimCode = string.Empty;


			try
			{

				var confirmation = long.Parse(orderConfirmation);
				var order = new v_wAllOrders();


				order = allOrders.FirstOrDefault(x => x.OrderAsmchta == confirmation && x.BarCode == barCode);
				if (order == null)
				{
					order = allOrders.FirstOrDefault(x => x.OrderAsmchta == confirmation);

				}


				redimCode = order.CardNumber;
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, $"Error in GetRedimCode, orderConfirmation: {orderConfirmation}, error message: {ex.Message}");
				throw;
			}

			return redimCode;
		}
		private PurchaseResponseDTO SetVariantsOrderConfirmations(PurchaseResponseDTO response, PurchaseRequestDTO purchaseRequestDTO, List<v_wAllOrders> allOrders)
		{
			try
			{
				var allOrderConfirmations = allOrders.Select(x => new VariantOrderConfirmationDTO
				{
					Barcode = x.BarCode,
					Qty = x.OrderQuantity,
					Confirmation = x.OrderAsmchta.ToString()
				}).ToList();
				string baseUrl = _configuration.GetConfigByValue<string>(ConfigurationKey.EventTicketView);
				var orderConfirmationsIsTaken = new Dictionary<VariantOrderConfirmationDTO, int>();
				object lockingObj = new object();

				foreach (var orderConfirmation in allOrderConfirmations)
				{
					orderConfirmationsIsTaken.Add(orderConfirmation, 0);
				}

				//Use dictionary here
				foreach (var variant in response.Data.Variants)
				{
					if (orderConfirmationsIsTaken.Any(x => x.Key.Barcode.Equals(variant.VariantBarCode) && x.Value < x.Key.Qty))
					{
						var variantOrderConfirmation = orderConfirmationsIsTaken.FirstOrDefault(x => x.Key.Barcode.Equals(variant.VariantBarCode) && x.Value < x.Key.Qty);
						variant.OrderConfirmation = variantOrderConfirmation.Key.Confirmation;
						orderConfirmationsIsTaken[variantOrderConfirmation.Key]++;
						variant.DtsRedimCode = GetRedimCode(variant.OrderConfirmation, allOrders, variant.VariantBarCode);
						if (!string.IsNullOrEmpty(purchaseRequestDTO.EventsGuid))
						{
							var url = string.Format($"{baseUrl}?CategoryNumber={purchaseRequestDTO.EventsGuid}");
							variant.EventsTicketLink = url;
						}
					}
					else
					{
						LoggerHelper.Error($"Error in SetVariantsOrderConfirmations, No OrderConfirmation. DtsorderId: {response.Data.DtsOrderId}, BarCode: {variant.VariantBarCode}");
					}
				};
				return response;
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in SetVariantsOrderConfirmations, error message: {ex.Message}");
				throw;
			}
		}
		private int SendEmail(PurchaseRequestDTO purchaseRequestDTO, PurchaseResponseDTO purchaseResponseDTO)
		{
			var template = _dtsOnLineRepo.GetConfirmationTemplate(ContextManager.CurrentOrganization().OrgId);
			var ToMail = purchaseRequestDTO.Email ?? ContextManager.CurrentUser().Email;
			Common.EF.DTS_Online.EmailQueue email = new Common.EF.DTS_Online.EmailQueue
			{
				/*
                 *  EmailTo = userFromDb.Email,
                    EmailDateAdded = DateTime.Now,
                    EmailFrom = ContextManager.CurrentOrganization().ServiceMail,
                    EmailBody = message,
                    EmailSendDate = DateTime.Now,
                    EmailSubject = "כניסה חד פעמית לאתר בהצדעה",
                    EmailType = 100000 + ContextManager.CurrentOrganization().OrgId,
                    IsBodyHtml = true,
                    IsSendEmail = false,
                    SeveralAttempts = 0,
                    EmailQueueUsersId = 2 //TEMPORARY!
                 */
				EmailFrom = template.EmailFrom,
				EmailTo = ToMail,
				EmailBody = string.Format(template.Template, purchaseResponseDTO.Data.PaymentId, ContextManager.CurrentOrganization().OrgId, ContextManager.CurrentUser().Id),
				EmailType = template.EmailTypeId.HasValue ? template.EmailTypeId : 100000 + ContextManager.CurrentOrganization().OrgId,
				EmailCc = template.Cc,
				EmailSubject = template.Subject,
				PaymentId = purchaseResponseDTO.Data.PaymentId,
				EmailDateAdded = DateTime.Now,
				EmailQueueUsersId = 2,
				SeveralAttempts = 0,
				IsSendEmail = false,
				IsBodyHtml = true
			};
			int emailId = _dtsOnLineRepo.AddContactToEmailQueueSync(email);
			return emailId;
		}
		private void SendSmsCoupon(PurchaseRequestDTO purchaseRequestDTO, PurchaseResponseDTO purchaseResponseDTO, List<v_wAllOrders> allOrders, List<Common.EF.Club.ProductsVars> productsVars)
		{
			Common.EF.Club.Orders order = _clubRepo.GetOrderByOrderId(purchaseResponseDTO.Data.DtsOrderId);
			var orderDate = order.InsertDate.Day.ToString().PadLeft(2, '0') + "/" + order.InsertDate.Month.ToString().PadLeft(2, '0') + "/" + order.InsertDate.Year.ToString();
			var payment = purchaseResponseDTO.Data.PaymentId;
			var items = (from a in allOrders group a by new { a.BarCode, a.OrderAsmchta } into g select new { VarBarCode = g.Key.BarCode, Data = g, Asmachta = g.Key.OrderAsmchta }).ToList();
			foreach (var item in items)
			{
				var variant = productsVars.First(x => x.FullBarCode == item.VarBarCode);
				var firstData = item.Data.First();
				var lastImpDate = firstData.LastImplementationDate.Value.Day.ToString().PadLeft(2, '0') + "/" + firstData.LastImplementationDate.Value.Month.ToString().PadLeft(2, '0') + "/" + firstData.LastImplementationDate.Value.Year.ToString();
				var productInfo = variant.VarName;
				var redimCodes = item.Data.Where(x => x.CardNumber != ContextManager.CurrentUser().CardNumber).Select(s => s.CardNumber).ToList();
				if (redimCodes.Any())
				{
					foreach (var redimCode in redimCodes)
					{
						var productInfoForSms = redimCodes.Aggregate((i, j) => i + ", " + j);
						var messageForCopuns = MessagesUtil.GetMessagesByKey(new List<int> { 10280 }).FirstOrDefault().MessageText;//dtsOnlineContext.Messages.FirstOrDefault(m => m.OrganizationID.Value.Equals(orgDetails.OrgId) && m.MessageKey == 10280).MessageText;
						var message = string.Format(messageForCopuns, productInfoForSms, variant.ShortNameVar);
						if (!string.IsNullOrEmpty(variant.CouponSMS))
						{
							message += variant.CouponSMS;
						}
						message += " הקופון בתוקף עד לתאריך " + lastImpDate;
						if (variant.IsSendDigitalCode == true && !redimCode.Contains("http"))
						{
							string longUrl = string.Format(_configuration.GetConfigByValue<string>(ConfigurationKey.LongUrl) + item.Asmachta);
							string shortUrl = _clubRepo.GenerateUrl(longUrl).Result;
							message += "למימוש, לחץ על הקישור: " + string.Format(_configuration.GetConfigByValue<string>(ConfigurationKey.SendSmsByBl2) + shortUrl);
						}
						Common.EF.DTS_Online.SmsQueue sms = new Common.EF.DTS_Online.SmsQueue
						{
							Message = message,
							DateAdded = DateTime.Now,
							MemberId = ContextManager.CurrentUser().Id,
							MessageLengh = message.Length,
							OrganizationId = ContextManager.CurrentOrganization().OrgId,
							Priority = 100,
							SmsType = 0,
							ExpirationDelayInMinutes = 120,
							DeliveryDelayInMinutes = 0,
							SenderName = _configuration.GetConfigByValue<string>(ConfigurationKey.ConfirmationSmsSenderName),
							SeveralAttempts = 0,
							SmsQueueUsersId = 1,
							Subscribers = purchaseRequestDTO.Mobile,
							CouponId = 0
						};
						_dtsOnLineRepo.AddSmsQueue(sms);
					}
				}
			}

            // NEW LOOP - regular items (those not sent in the existing loop)
            var regularItems = allOrders
                .Where(w => w.CardNumber == ContextManager.CurrentUser().CardNumber)
                .GroupBy(x => x.BarCode)
                .Select(g => new
                {
                    VarBarCode = g.Key,
                    Data = g.ToList(),
                    Quantity = g.Count()
                })
                .ToList();

            foreach (var reg in regularItems)
            {
                var first = reg.Data.First();
                var variant = productsVars.First(x => x.FullBarCode == reg.VarBarCode);

                var senderName = _configuration.GetConfigByValue<string>(ConfigurationKey.ConfirmationSmsSenderName);
                var orgIdStr = ContextManager.CurrentOrganization().OrgId.ToString();

                _dtsOnLineRepo.SendVariantSMS(
                    variantType: variant.VariantType,
                    quantity: reg.Quantity,
                    posBarCode: variant.PosbarCode,
                    businessSubTypeId: (int)variant.BusinessSubTypeId,
                    businessId: variant.BusinessId,
                    firstName: purchaseRequestDTO.FirstName,
                    cardNumber: first.CardNumber,
                    shortNameVar: variant.ShortNameVar,
                    lastImplementationDate: first.LastImplementationDate ?? DateTime.Now,
                    mobile: purchaseRequestDTO.Mobile,
                    orgId: orgIdStr,
                    senderName: senderName,
                    memberId: ContextManager.CurrentUser().Id,
                    smsQueueUsersId: null
                );
            }

        }
		private PurchaseResponseDTO OrderConfirmation(PurchaseRequestDTO purchaseRequestDTO, PurchaseResponseDTO purchaseResponseDTO, List<ProductsVars> productsVars)
		{
			DtsLoggger.Logger.Info("PurchaseHandler OrderConfirmation Started");
			List<v_wAllOrders> allOrders = new List<v_wAllOrders>();
			allOrders = _clubRepo.GetAllOrdersByOrderId(purchaseResponseDTO.Data.DtsOrderId);
			purchaseResponseDTO.Data.OrderGuid = purchaseRequestDTO.OrderGuid;
			if (string.IsNullOrEmpty(purchaseResponseDTO.Data.OrderConfirmation))
			{
				var paymentRow = _clubRepo.GetPaymentRowByPaymentId(purchaseResponseDTO.Data.PaymentId);
				if (paymentRow != null)
				{
					purchaseResponseDTO.Data.OrderConfirmation = paymentRow.ConfirmationNumber;
					DtsLoggger.Logger.Info($"PurchaseHandler ServetTransactionID:{purchaseResponseDTO.Data.OrderConfirmation}");
					purchaseResponseDTO = SetVariantsOrderConfirmations(purchaseResponseDTO, purchaseRequestDTO, allOrders);
				}
			}
			try
			{
				DtsLoggger.Logger.Info("PurchaseHandler SendMemberConfirmationEmail - Send Confirmation Email to Buyer member");
				SendSmsCoupon(purchaseRequestDTO, purchaseResponseDTO, allOrders, productsVars);
				int res = SendEmail(purchaseRequestDTO, purchaseResponseDTO);
				DtsLoggger.Logger.Info($"SendMemberConfirmationEmail result => {res > 0}");
			}
			catch (Exception ex)
			{
				LoggerHelper.Error("Send confirmation EMAIL failed. Exception:" + ex.ToString());
			}
			return purchaseResponseDTO;
		}
		private PurchaseResponseDTO InitResponse(PurchaseRequestDTO purchaseRequestDTO, PurchaseResponseDTO purchaseResponseDTO)
		{
			purchaseResponseDTO.Data.FirstName = purchaseRequestDTO.FirstName;
			purchaseResponseDTO.Data.LastName = purchaseRequestDTO.LastName;
			purchaseResponseDTO.Data.Mobile = purchaseRequestDTO.Mobile;
			purchaseResponseDTO.Data.CreditCardExpiration = purchaseRequestDTO.CreditCardExpirey;
			purchaseResponseDTO.Data.TotalPayments = purchaseRequestDTO.TotalPayment;
			purchaseResponseDTO.Data.NumOfPayments = purchaseRequestDTO.NumOfPayments;
			purchaseResponseDTO.Data.Email = purchaseRequestDTO.Email;
			return purchaseResponseDTO;
		}
		public async Task<PurchaseResponseDTO> Purchase(PurchaseRequestDTO purchaseRequestDTO)
		{
			PurchaseResponseDTO purchaseResponseDTO = new PurchaseResponseDTO();
			try
			{
				purchaseResponseDTO = InitResponse(purchaseRequestDTO, purchaseResponseDTO);
                List<ProductsVars> productsVars  = await this.Validations(purchaseRequestDTO);
				purchaseResponseDTO = await this.CreateNewOrder(purchaseRequestDTO, productsVars, purchaseResponseDTO);
				this.UpdateMemberData(purchaseRequestDTO, productsVars, purchaseResponseDTO.Data.DtsOrderId);
				this.OrderEvent(purchaseRequestDTO, purchaseResponseDTO.Data.DtsOrderId);
				this.ValidateOrderEventBooking(purchaseRequestDTO.EventsGuid);
				purchaseResponseDTO.Data.Variants = FillResponseVariants(productsVars, purchaseRequestDTO, purchaseResponseDTO.Data.DtsOrderId);
				purchaseResponseDTO = this.OrderConfirmation(purchaseRequestDTO, purchaseResponseDTO, productsVars);
				purchaseResponseDTO.Status = 1;
				return purchaseResponseDTO;
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Main Catch Exception: {ex.ToString()}");
				HandleRollback(purchaseRequestDTO, purchaseResponseDTO);
				purchaseResponseDTO.Status = 0;
				purchaseResponseDTO.ErrorDescription = ex.Message;
				return purchaseResponseDTO;
			}
		}
		private TransactionResults CreditGuardSimpleRefund(PurchaseRequestDTO purchaseRequestDTO, PurchaseResponseDTO purchaseResponseDTO)
		{
			TransactionResults t = null;
			ApiClient cg = new ApiClient();
			t = cg.Refund(new RefundRequest()
			{
				GUID = "0",
				TerminalNumber = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalNumber),
				TotalPrice = (double)purchaseRequestDTO.TotalPayment,
				MemberID = ContextManager.CurrentUser().Id,
				OrganizationID = ContextManager.CurrentOrganization().OrgId,
				TransactionID = purchaseResponseDTO.Data.CreditGuardResult.ServerTransactionID
			});
			return t;
		}
		private bool DoVariantsRollBack(PurchaseResponseDTO purchaseResponseDTO, Common.EF.Club.Orders order)
		{
			List<AtractionsOrders> attractionsOrders = _clubRepo.GetAtractionsOrdersByOrderId(purchaseResponseDTO.Data.DtsOrderId);
			if (attractionsOrders != null && attractionsOrders.Any())
			{
				foreach (var attractionsOrder in attractionsOrders)
				{
					var coupon = _clubRepo.GetCouponsStock(attractionsOrder.MemberOrderAsmchta);
					if (coupon != null && coupon.CouponStockId > 0)
					{
						_dtsOnLineRepo.DoCouponRollBack(coupon.CouponStockId, order.MemberId);
						_clubRepo.RemoveCouponsStockAtrOrders(coupon);
					}
				}
			}
			bool result = _clubRepo.DoVariantsRollBack(order, attractionsOrders);
			return true;
		}
		private void HandleRollback(PurchaseRequestDTO purchaseRequestDTO, PurchaseResponseDTO purchaseResponseDTO)
		{
			try
			{
				Common.EF.Club.Orders order = _clubRepo.GetOrderByOrderId(purchaseResponseDTO.Data.DtsOrderId);
				bool isOrderExists = order != null && order.OrderId > 0;
				bool shouldRefundCG = purchaseResponseDTO.Data.CreditGuardResult != null && purchaseResponseDTO.Data.CreditGuardResult.Code == ResponseCode.SUCCESS;
				bool isHavePaymentId = purchaseResponseDTO.Data.PaymentId > 0;
				TransactionResults result = null;
				if (shouldRefundCG)
				{
					DtsLoggger.Logger.Info($"PurchaseHandler Rollback CreditGuard, ServerTransactionID: {purchaseResponseDTO.Data.CreditGuardResult.ServerTransactionID}.");
					result = CreditGuardSimpleRefund(purchaseRequestDTO, purchaseResponseDTO);
					DtsLoggger.Logger.Info($"PurchaseHandler Rollback CreditGuard finished: Message: {0}. Code: {1}", result.Message, result.ServerResponseCode);
				}
				if (isOrderExists)
				{
					DoVariantsRollBack(purchaseResponseDTO, order);
					if (!string.IsNullOrWhiteSpace(purchaseRequestDTO.EventsGuid))
					{
						var isTicketsHubBooked = IsTicketshubOrderBooked(purchaseRequestDTO.EventsGuid);
						if (isTicketsHubBooked)
						{
							var responseFromTicketsHub = _EventBl.CanselOrder(purchaseRequestDTO.EventsGuid);
							DtsLoggger.Logger.Info(string.Format("PurchaseHandler Rollback TicketsHub Status: {0}.", responseFromTicketsHub != null ? responseFromTicketsHub.Status.ToString() : "null"));
						}
					}
				}
				if (isHavePaymentId)
				{
					if (result != null && result.Code == ResponseCode.SUCCESS)
					{
						string last4digits = purchaseRequestDTO.CreditCard16Digits != null && purchaseRequestDTO.CreditCard16Digits.Length > 10 ? Last4Digits(purchaseRequestDTO.CreditCard16Digits) : "";
						_clubRepo.InsertPayment(ContextManager.CurrentUser().Id, purchaseRequestDTO.TotalPayment * -1, "", purchaseRequestDTO.identity, last4digits, result.ServerTransactionID);
					}
					else
						throw new Exception(string.Format("CreditGuard Refund Failed. ConfirmationNumber: {0}. Code: {1}. Message: {2}. ServerResponseCode: {3}", purchaseResponseDTO.Data.CreditGuardResult.ServerTransactionID, result.Code, result.Message, result.ServerResponseCode));
				}
			}
			catch (Exception ex)
			{
				DtsLoggger.Logger.Info(string.Format("PurchaseHandler Rollback Failed! Exception: {0}.", ex.ToString()));
				var messageToCRM = "אירעה שגיאה בתהליך הרולבק של הרכישה. נא להעביר מייל זה לצוות הפיתוח.";
				EmailQueue emailToSend = new EmailQueue
				{
					EmailFrom = ContextManager.CurrentOrganization().ServiceMail,
					EmailTo = "MeravG@dts-4u.com,support@bl-mail.com",
					EmailBody = messageToCRM + "\r\n" + "Exception: " + ex.ToString(),
					EmailType = 100000 + ContextManager.CurrentOrganization().OrgId,
					EmailSubject = "תקלה בתהליך הרכישה והזיכוי",
					IsBodyHtml = true,
					EmailDateAdded = DateTime.Now,
					EmailQueueUsersId = 2,
				};
				_dtsOnLineRepo.AddContactToEmailQueueSync(emailToSend);
				throw;
			}
		}
		private bool IsTicketshubOrderBooked(string eventsGuid)
		{
			var orderBook = _ticketsHubRepo.GetOrdersByEventGuid(eventsGuid);
			return orderBook != null && orderBook.IsBooked;
		}
		/*private string JsonSerializeObject<T>(this T toSerialize)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(toSerialize);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, $"Error in JsonSerializeObject, Error message: {ex.Message}");
                throw;
            }
        }*/
		private bool UpdateOrdersByEvent(long orderId, string eventsGuid)
		{
			var orderTicketHub = _ticketsHubRepo.GetOrdersByEventGuid(eventsGuid);
			if (orderTicketHub != null)
			{
                var orderTickets = _ticketsHubRepo.GetOrderTicketsByOrderId(orderTicketHub.OrderId);
                _clubRepo.UpdateCreateOrderByEvent((int)orderId, orderTicketHub.OrderId, orderTickets, eventsGuid);
			}
			else
			{
				DtsLoggger.Logger.Info($"EventsFunctions.UpdateOrdersByEvent' warning! orderTicketHub is not found by eventGuid:{eventsGuid}");
				return false;
			}
			return true;
		}
		private void OrderEvent(PurchaseRequestDTO purchaseRequestDTO, long orderId)
		{
			bool isEvents = !string.IsNullOrEmpty(purchaseRequestDTO.EventsGuid);
			DtsLoggger.Logger.Info("PurchaseHandler OrderEvent Started. isEvents: " + isEvents);
			if (!isEvents)
				return;
			try
			{
				ShellTicketHub<object> result = _EventBl.BookOrder(purchaseRequestDTO.EventsGuid);
				if (result != null)
				{
					DtsLoggger.Logger.Info($"PurchaseHandler OrderEvent ThicketHub Response ErrorMessage: {result.ErrorMessage}");
					DtsLoggger.Logger.Info($"PurchaseHandler OrderEvent ThicketHub Response Status: {result.Status}");
					if (result.Status)
					{
						DtsLoggger.Logger.Info("PurchaseHandler UpdateOrdersByEvent");
						this.UpdateOrdersByEvent(orderId, purchaseRequestDTO.EventsGuid);
					}
					else
					{
						LoggerHelper.Error("PurchaseHandler OrderEvent Failed due TicketsHub Error: " + result.ErrorMessage);
						throw new BusinessException("ההזמנה לא נקלטה - רכישת רכטיסים למופע נכשלה");
					}
				}
				else
				{
					LoggerHelper.Error("PurchaseHandler OrderEvent Failed due TicketsHub Error: responseData is empty");
					throw new BusinessException("ההזמנה לא נקלטה - רכישת רכטיסים למופע נכשלה");
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error("PurchaseHandler OrderEvent Failed: " + ex.ToString());
				throw new BusinessException("שגיאה כללית או תקלת תקשורת");
			}
		}
		private void UpdateMemberData(PurchaseRequestDTO purchaseRequestDTO, List<ProductsVars> productsVars, int orderId)
		{
			try
			{
				DtsLoggger.Logger.Info("PurchaseHandler UpdateMemberData Started");
				long memberAddressId = 0;

				string cityName = null;
				var SubTypeTzarchanut = _configuration.GetConfigByValue<string>(ConfigurationKey.SubTypeTzarchanut).Split(",");
				if (productsVars.Exists(p => p.BusinessSubTypeId == 26))
				{
					if (purchaseRequestDTO.City.HasValue)
					{
						City city = _dtsOnLineRepo.GetCityByCityId(purchaseRequestDTO.City.Value.ToString());
						if (city != null)
							cityName = city.CityName;
					}
					MemberAddress memberAddress = _clubRepo.GetMemberAddressForTzarchanut(purchaseRequestDTO);
					if (memberAddress == null)
					{
						MemberAddress memberAddressToAdd = new MemberAddress
						{
							InsertDate = DateTime.Now,
							MemberId = ContextManager.CurrentUser().Id,
							StreetName = purchaseRequestDTO.StreetName,
							HouseNumber = purchaseRequestDTO.HouseNumber,
							ApartmentNumber = purchaseRequestDTO.ApartmentNumber,
							CityId = purchaseRequestDTO.City,
							CityName = cityName,
							Zip = purchaseRequestDTO.ZipCode,
							Mailbox = purchaseRequestDTO.Mailbox,
							Entrance = purchaseRequestDTO.Entrance,
							UpdatedTime = DateTime.Now
						};
						memberAddressId = _clubRepo.AddMemberAdress(memberAddressToAdd);
					}
					else
					{
						memberAddressId = memberAddress.MemberAddressId;
						if (memberAddress.Zip != null && memberAddress.Zip.Trim() != purchaseRequestDTO.ZipCode)
						{
							memberAddress.Zip = purchaseRequestDTO.ZipCode;
							memberAddress.UpdatedTime = DateTime.Now;
							_clubRepo.UpdateMemberAddress(memberAddress);
						}
					}
				}
				else
				{
					var member = _clubRepo.GetUserByUserID(ContextManager.CurrentUser().Id).Result;
					if (member != null)
					{
						if (member.City.HasValue)
						{
							//get city name by city id - allMembers
							City city = _dtsOnLineRepo.GetCityByCityId(member.City.Value.ToString());
							if (city != null)
								cityName = city.CityName;
						}
						MemberAddress memberAddress = _clubRepo.GetMemberAddress(member, purchaseRequestDTO.ApartmentNumber);
						if (memberAddress == null)
						{
							MemberAddress memberAddressToAdd = new MemberAddress
							{
								InsertDate = DateTime.Now,
								MemberId = ContextManager.CurrentUser().Id,
								StreetName = member.StreetName != null ? member.StreetName : member.Address,
								HouseNumber = member.HouseNumber,
								ApartmentNumber = purchaseRequestDTO.ApartmentNumber,
								CityId = member.City,
								CityName = cityName,
								Zip = member.Zip,
								Mailbox = purchaseRequestDTO.Mailbox,
								Entrance = purchaseRequestDTO.Entrance,
								UpdatedTime = DateTime.Now
							};
							memberAddressId = _clubRepo.AddMemberAdress(memberAddressToAdd);
						}
						else
							memberAddressId = memberAddress.MemberAddressId;
					}
				}
				var BarCodes = productsVars.Select(s => s.FullBarCode);
				foreach (var productBarCode in BarCodes)
				{
					_clubRepo.UpdateAddressForAtractionsOrders(ContextManager.CurrentUser().Id, productBarCode, orderId.ToString(), memberAddressId);
				}
			}
			catch (Exception ex)
			{
				throw new BusinessException("שגיאה כללית או תקלת תקשורת");
			}
		}
		private bool CheckNumOfPaymentsLogic(decimal totalPayment, int numOfPayment)
		{

			var result = false;

			var list = new Dictionary<int, int>();
			list.Add(150, 2);
			list.Add(300, 3);
			list.Add(500, 4);
			list.Add(700, 5);
			list.Add(900, 6);
			list.Add(1200, 7);
			list.Add(1400, 8);
			list.Add(1600, 9);
			list.Add(1800, 10);
			list.Add(2000, 12);

			try
			{
				var selected = list.AsQueryable().Where(x => x.Key <= totalPayment).Last();

				if (numOfPayment <= selected.Value)
					result = true;
			}
			catch
			{
				if (numOfPayment == 1)
					result = true;
			}
			return result;
		}
        private async Task<List<ProductsVars>> Validations(PurchaseRequestDTO purchaseRequestDTO)
        {
            List<PurchaseRequestDTO.Variant> variants = purchaseRequestDTO.Cart.SelectMany(c => c.Variants).ToList();
            List<string> barcodesList = variants.Select(x => x.Barcode).ToList();
            bool tsarhanutVariantsExist = _clubRepo.isHaveTsarchanutVariant(barcodesList);
            Guid tempOrderGuid;
            List<ProductsVars>  productsVars = new List<ProductsVars>();
            bool allVariantsExists = true;
            foreach (var cartItem in purchaseRequestDTO.Cart)
            {
                List<ProductsVars> pVars = _clubRepo.GetProductsVars(cartItem.CategoryId);
                pVars = pVars.Where(p => barcodesList.Contains(p.FullBarCode)).ToList();
                if (!pVars.Any())
                    allVariantsExists = false;
                productsVars.AddRange(pVars);
            }
            productsVars = productsVars.Distinct().ToList();

            if (purchaseRequestDTO.TotalPayment < 0)
                throw new BusinessException("ההזמנה לא נקלטה - סכום הכסף הכללי אינו תואם את ההזמנה");
            if (!Guid.TryParse(purchaseRequestDTO.OrderGuid, out tempOrderGuid))
                throw new BusinessException("מבנה OrderGuid אינו תקין");
            if (_contextManager.CurrentUser().ClubCreditCard <= 0)
                throw new BusinessException("המשתמש לא רשאי לבצע רכישות באתר");
			if (!(!tsarhanutVariantsExist || purchaseRequestDTO.TotalPayment == 0 || (tsarhanutVariantsExist && (!string.IsNullOrEmpty(purchaseRequestDTO.ApartmentNumber) && purchaseRequestDTO.City != null && !string.IsNullOrEmpty(purchaseRequestDTO.HouseNumber) && !string.IsNullOrEmpty(purchaseRequestDTO.StreetName) && !string.IsNullOrEmpty(purchaseRequestDTO.ZipCode)))))
                throw new BusinessException("חסרים פרטים אישיים בתשלום עבור מוצר צרכנות");

            if (purchaseRequestDTO.NumOfPayments != 1 &&
                !CheckNumOfPaymentsLogic(purchaseRequestDTO.TotalPayment, purchaseRequestDTO.NumOfPayments))
                throw new BusinessException("מס' תשלומים אינו תואם לחוקיות התשלומים של האתר");

            if (!string.IsNullOrEmpty(purchaseRequestDTO.PinCode) && !string.IsNullOrEmpty(purchaseRequestDTO.CreditCard16Digits))
            {
              
                utils.ValidatePinCodeFormat(purchaseRequestDTO.PinCode, _configuration.GetConfigByValue<int>(ConfigurationKey.PincodeLength));
                
                if (!CheckPinCodeLenth(purchaseRequestDTO.PinCode.Trim()))
                    throw new BusinessException("אורך קוד מקוצר לא תקין");
            }

            if (string.IsNullOrEmpty(purchaseRequestDTO.CreditCard16Digits) && !string.IsNullOrEmpty(purchaseRequestDTO.PinCode))
            {              
                var validationResult = await _userService.ValidatePinCode(_contextManager.CurrentUser().Id, purchaseRequestDTO.PinCode.Trim());

                if (validationResult == 1)
                {
                    throw new BusinessException("קוד מקוצר אינו מוכר");
                }
                else if (validationResult == 2)
                {
                    throw new BusinessException("חסום");
                }
            }

            if (purchaseRequestDTO.TotalPayment > 0 && string.IsNullOrEmpty(purchaseRequestDTO.PinCode) &&
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) >
                new DateTime(int.Parse(purchaseRequestDTO.CreditCardExpirey.Substring(2, 2)) + 2000,
                             int.Parse(purchaseRequestDTO.CreditCardExpirey.Substring(0, 2)), 1))
                throw new BusinessException("ההזמנה לא נקלטה - תוקף הכרטיס שהתקבל אינו חוקי או בעבר");

            if (!_clubRepo.CheckAllBenefitIdExists(purchaseRequestDTO.Cart.Select(x => x.CategoryId).ToList()))
                throw new BusinessException("קוד הטבה לא מזוהה או לא פעיל");

            if (!StockFunctions.IsVariantInStock(_restApiGW, _configuration, ContextManager.CurrentOrganization().OrgId, purchaseRequestDTO.Cart))
                throw new BusinessException("ההזמנה לא נקלטה - וריאנט אזל מהמלאי");

            if (productsVars.Count(x => x.VarDiscountFormula != null && x.VarDiscountFormula.Length > 10) > 0)
                throw new BusinessException("VarDiscountFormula לא תקין");

            if (!allVariantsExists)
                throw new BusinessException("ההזמנה לא נקלטה - אין מספיק מטבעות");

            List<Common.EF.Club.BusinessSubTypeSpecificationByVariants> specsByVars = _clubRepo.GetBusinessSubTypeSpecificationByVariants(productsVars);
            List<Common.EF.Club.BusinessSubTypeSpecificationCurrent> specsCurrent = _clubRepo.GetBusinessSubTypeSpecificationCurrents(productsVars);
            int totalPrice = 0;

            foreach (var variant in variants)
            {
                ProductsVars productVar = productsVars.FirstOrDefault(p => p.FullBarCode.Equals(variant.Barcode));
                Common.EF.Club.BusinessSubTypeSpecificationByVariants specByVar = specsByVars.FirstOrDefault(spec => spec.BarCode.Equals(variant.Barcode));
                Common.EF.Club.BusinessSubTypeSpecificationCurrent specCurrent = specsCurrent.FirstOrDefault(spec => spec.BusinessSubTypeId == productVar.BusinessSubTypeId);
                int benefitTypeId = ProductFunctions.GetBenefitTypeIdByVariantType(productVar.VariantType);
                decimal? price = ProductFunctions.GetVariantPrice(
                    ContextManager.CurrentUser().PremiumType,
                    productVar, specByVar, specCurrent,
                    !(purchaseRequestDTO.CreditCardStatus == 0 || purchaseRequestDTO.CreditCardStatus == 6)
                    && _contextManager.CurrentUser().ClubCreditCard > 0,
                    _contextManager.CurrentOrganization().IsNewSubsidy
                );

                if (benefitTypeId == (int)BenefitType.Coupon &&
                    !(new OrganizationFunctions().CheckCouponStock(productVar.CuponStockId.GetValueOrDefault()) == true))
                    throw new BusinessException("ההזמנה לא נקלטה - וריאנט אזל מהמלאי");

                if (productVar.DisabledToOrder)
                    throw new BusinessException("ההזמנה לא נקלטה - וריאנט לא מזוהה או לא פעיל");

                if (productVar.LastImplementationDate.HasValue && productVar.LastImplementationDate.Value.Date < DateTime.Now.Date)
                    throw new BusinessException("ההזמנה לא נקלטה - הוריאנט פג תוקף");

                if (variant.Quantity <= 0)
                    throw new BusinessException("ההזמנה לא נקלטה - נרכש וריאנט בכמות 0 או שלילית");

                if (productVar.BusinessSubTypeId != null &&
                    _limitationsBL.GetVariantOrderLimit(productVar.FullBarCode, (int)productVar.BusinessSubTypeId, true, false) < variant.Quantity)
                    throw new BusinessException("ההזמנה לא נקלטה - עברת את מגבלות הרכישה");

                if (price == null)
                    throw new BusinessException("שגיאה בחישוב מחיר וריאנט");

                if (price != variant.Price)
                    variant.Price = int.Parse(price.ToString());

                totalPrice += int.Parse(price.ToString()) * variant.Quantity;
            }

            if (totalPrice != purchaseRequestDTO.TotalPayment)
                throw new BusinessException("ההזמנה לא נקלטה - סכום הכסף הכללי אינו תואם את ההזמנה");

            if (string.IsNullOrWhiteSpace(purchaseRequestDTO.PinCode) &&
                (string.IsNullOrEmpty(purchaseRequestDTO.Mobile) || purchaseRequestDTO.Mobile.Length != 10))
                throw new BusinessException("ההזמנה לא נקלטה - הטלפון נייד איננו חוקי או חסר");

            if (!string.IsNullOrWhiteSpace(purchaseRequestDTO.EventsGuid) &&
                !EventFunctions.ValidateEventGuidReservationStatus(_ticketsHubRepo, purchaseRequestDTO.EventsGuid))
                throw new BusinessException("ההזמנה לא נקלטה - רכישת כרטיסים למופע נכשלה");

			return productsVars;
        }
        private bool CheckPinCodeLenth(string pincode)
		{
			bool res = true;
			if (!string.IsNullOrEmpty(pincode))
			{
				int requiredLength = _configuration.GetConfigByValue<int>(ConfigurationKey.PincodeLength);
                res = pincode.Length == requiredLength;
            }
			return res;
		}
		private Dictionary<int, int> GetNumOfPaymentsOptions()
		{
			var list = new Dictionary<int, int>();
			list.Add(150, 2);
			list.Add(300, 3);
			list.Add(500, 4);
			list.Add(700, 5);
			list.Add(900, 6);
			list.Add(1200, 7);
			list.Add(1400, 8);
			list.Add(1600, 9);
			list.Add(1800, 10);
			list.Add(2000, 12);
			return list;
		}

		private async Task<PurchaseResponseDTO> CreateNewOrder(PurchaseRequestDTO purchaseRequestDTO, List<ProductsVars> productsVars, PurchaseResponseDTO purchaseResponseDTO)
		{
			try
			{
				string cardId = string.Empty;
				DtsLoggger.Logger.Info($"PurchaseHandler NewOrder Started for MemberId: {_contextManager.CurrentUser().Id}");
				AllMembersProperties memberProperties = null;
				bool isPayWithPinCode = !string.IsNullOrEmpty(purchaseRequestDTO.PinCode) && string.IsNullOrEmpty(purchaseRequestDTO.CreditCard16Digits);
				bool isNeedSavePinCode = !string.IsNullOrEmpty(purchaseRequestDTO.PinCode) && !string.IsNullOrEmpty(purchaseRequestDTO.CreditCard16Digits);
				bool allowZeroPayment = purchaseRequestDTO.TotalPayment == 0;
				string known4LastDigits = string.Empty;
				if (isPayWithPinCode)
				{
					memberProperties = await _clubRepo.GetMembersPropertiesById(purchaseRequestDTO.identity);
					purchaseRequestDTO.CreditCardExpirey = memberProperties.CardExpiration;
					purchaseRequestDTO.identity = !string.IsNullOrWhiteSpace(memberProperties.PayerTz) ? memberProperties.PayerTz.PadLeft(9, '0').Trim() : purchaseRequestDTO.identity.Trim();
				}
				cardId = memberProperties != null && memberProperties.CardId != null ? memberProperties.CardId : "";
				known4LastDigits = memberProperties != null && memberProperties.CardNum != null ? memberProperties.CardNum : "";
				TransactionResults creditGuardResult = new TransactionResults() { Code = 0, Message = "", ServerResponseCode = "0", ServerTransactionID = "000" };
				if (!allowZeroPayment)
					creditGuardResult = this.CreditGuardPayment(purchaseRequestDTO, cardId.Trim(), isPayWithPinCode);
				purchaseResponseDTO.Data.CreditGuardResult = creditGuardResult;
				if (isNeedSavePinCode)
					this.SavePinCode(purchaseRequestDTO, creditGuardResult.ServerData.CardToken);
				purchaseResponseDTO.Data.PaymentId = this.InsertPayment(creditGuardResult, purchaseRequestDTO, known4LastDigits);
				purchaseResponseDTO.Data.OrderConfirmation = creditGuardResult.ServerTransactionID;
				OrderDll.GeneralOrdersItem order = this.InsertToOrdersTable(purchaseRequestDTO, productsVars, purchaseResponseDTO.Data.PaymentId);
				purchaseResponseDTO.Data.DtsOrderId = order.OrderId;
				this.InsertOrders(purchaseRequestDTO, productsVars, purchaseResponseDTO.Data.PaymentId, order);
				return purchaseResponseDTO;
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in CreateNewOrder, MemberId: {purchaseRequestDTO.identity}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
				throw new Exception(ex.Message, ex);
			}
		}
		private OrderDll.GeneralOrdersItem InsertToOrdersTable(PurchaseRequestDTO purchaseRequestDTO, List<ProductsVars> productsVars, long paymentId)
		{
			var order = new OrderDll.GeneralOrdersItem
			{
				MemberId = _contextManager.CurrentUser().Id,
				InsertDate = DateTime.Now,
				IsSentToFriend = false,
				FriendName = "",
				FriendMobile = "",
				NumberOfRetries = 0,
				ExternalGuid = purchaseRequestDTO.OrderGuid,
				EventsGuid = "",
				CreditCardToken = "",
				OrderStatusId = 0,
				CreditCard16Digits = purchaseRequestDTO.CreditCard16Digits,
				CreditCardExpirey = purchaseRequestDTO.CreditCardExpirey,
				OrderId = 0

			};

			Common.EF.Club.Orders orderResult = _clubRepo.InsertNewOrder(new Common.EF.Club.Orders()
			{
				CreditCard16Digits = Encrypt(order.CreditCard16Digits, _configuration.GetConfigByValue<string>(ConfigurationKey.EncryptionKeyNewOrder)),
				CreditCardExpirey = order.CreditCardExpirey,
				InsertDate = DateTime.Now,
				ExternalGuid = order.ExternalGuid,
				MemberId = order.MemberId,
				NumberOfRetries = 0,
				OrderStatusId = 0,
				IsSentToFriend = false,
				CreditCardToken = order.CreditCardToken,
				EventsGuid = order.EventsGuid,
				FriendMobile = order.FriendMobile,
				FriendName = order.FriendName,
				Slink = ""
			});
			order.OrderId = orderResult.OrderId;
			return order;
		}
		private TransactionResults CreditGuardPayment(PurchaseRequestDTO purchaseRequestDTO, string cardToken, bool isPayWithPinCode)
		{
			DtsLoggger.Logger.Info($"Start CreditGuardPayment for memberId: {_contextManager.CurrentUser().Id}");
			TransactionResults creditGuardResult = new TransactionResults();
			if (isPayWithPinCode)
				creditGuardResult = _creditGuard.PaymentToken(new CardTokenPaymentModelView
				{
					TerminalNumber = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalNumber),
					Expiration = purchaseRequestDTO.CreditCardExpirey,
					CVV = purchaseRequestDTO.Cvv,
					UserPersonalID = purchaseRequestDTO.identity,
					TotalPrice = double.Parse(purchaseRequestDTO.TotalPayment.ToString()),
					NumberOfPayments = purchaseRequestDTO.NumOfPayments,
					CardToken = cardToken,
				});
			else
			{
				creditGuardResult = _creditGuard.Payment(new CardNumberPaymentModelView
				{
					TerminalNumber = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalNumber),
					Expiration = purchaseRequestDTO.CreditCardExpirey,
					CVV = purchaseRequestDTO.Cvv,
					UserPersonalID = purchaseRequestDTO.identity,
					TotalPrice = double.Parse(purchaseRequestDTO.TotalPayment.ToString()),
					NumberOfPayments = purchaseRequestDTO.NumOfPayments,
					CardNumber = purchaseRequestDTO.CreditCard16Digits,
				});
				DtsLoggger.Logger.Info($"HelperFunctions CreditGuardPayment => Regular Payment, CardNumber = {purchaseRequestDTO.CreditCard16Digits.Substring(purchaseRequestDTO.CreditCard16Digits.Length - 4)}, Expiration = {purchaseRequestDTO.CreditCardExpirey}, MemberID = {_contextManager.CurrentUser().Id}, UserPersonalID = {purchaseRequestDTO.identity}");
			}

			DtsLoggger.Logger.Info($"Payment.res : Message = {creditGuardResult.Message}, Code = {creditGuardResult.Code}, DBRequestID = {creditGuardResult.DBRequestID}");

			if (creditGuardResult.Code != ResponseCode.SUCCESS)
			{
				//string cgErrorMsg = $"{creditGuardResult.Code}|{creditGuardResult.Message}";
				LoggerHelper.Error($"CreateOrder CreditGuardPayment' false result. CodeId: {(int)creditGuardResult.Code}, Code:: {creditGuardResult.Code},ServerResponseCode: {creditGuardResult.ServerResponseCode}, Message: {creditGuardResult.Message}. TotalPayment: {purchaseRequestDTO.TotalPayment}, MemberID(dtsId): {_contextManager.CurrentUser().Id}, CardId: {cardToken}. TZ: {purchaseRequestDTO.identity}");
				var responseCode = CreditGuardErrorsGenerator.Generate(creditGuardResult.ServerResponseCode);
				throw new CreditGuardException(responseCode);
			}
			return creditGuardResult;
		}

		private void SavePinCode(PurchaseRequestDTO purchaseRequestDTO, string cardId)
		{
			try
			{
				DtsLoggger.Logger.Info($"Start SavePinCode for memberId: {_contextManager.CurrentUser().Id}");
				_clubRepo.SavePinCode(purchaseRequestDTO.PinCode, _contextManager.CurrentUser());
				ExtandPeyerDataDTO edp = new ExtandPeyerDataDTO
				{
					PayerData = new PayerDataDTO
					{
						PayerCardExpiresMonth = purchaseRequestDTO.CreditCardExpirey.Substring(0, 2),
						PayerCardExpiresYear = purchaseRequestDTO.CreditCardExpirey.Substring(2, 2),
						PayerCardTZ = purchaseRequestDTO.identity,
					},
					MaxClubId = purchaseRequestDTO.CreditCardClub,
				};
				_clubRepo.UpdateOrAddToAllMembersProperties(edp, cardId, _contextManager.CurrentUser().Id);
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error to save pincode for memberId: {_contextManager.CurrentUser().Id}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
		}
		private string Last4Digits(string cardNumber)
		{
			return cardNumber.Substring(cardNumber.Length - 4);
		}

		private long InsertPayment(TransactionResults creditGuardResult, PurchaseRequestDTO purchaseRequestDTO, string last4Digits)
		{
			DtsLoggger.Logger.Info($"Start InsertPayment for memberId: {_contextManager.CurrentUser().Id}");
			if (!string.IsNullOrEmpty(purchaseRequestDTO.CreditCard16Digits) && purchaseRequestDTO.TotalPayment.ToString() != "0.0")
				last4Digits = Last4Digits(purchaseRequestDTO.CreditCard16Digits);

			XDocument doc = _creditGuard.GetXmlPaymentInformation(_contextManager.CurrentOrganization().DBName, creditGuardResult.ServerTransactionID, creditGuardResult.ServerData.AuthNumber,
				_configuration.GetConfigByValue<string>(ConfigurationKey.TerminalNumber), purchaseRequestDTO.NumOfPayments, creditGuardResult.ServerData.CardToken);
			long paymentId = _clubRepo.InsertPayment(_contextManager.CurrentUser().Id, purchaseRequestDTO.TotalPayment, doc.ToString(), purchaseRequestDTO.identity, last4Digits, creditGuardResult.ServerTransactionID, 1);
			if (paymentId <= 0)
			{
				LoggerHelper.Error($"Error in InsertPayment for memberId: {_contextManager.CurrentUser().Id}, paymentId is {paymentId}");
				throw new Exception($"Error in InsertPayment for memberId: {_contextManager.CurrentUser().Id}, paymentId is {paymentId}");
			}
			DtsLoggger.Logger.Info($"Success Insert payment {paymentId} for memberId: {_contextManager.CurrentUser().Id}");
			return paymentId;
		}

		private string GetGiftCardNumber(string barCode, string dtsId)
		{
			string giftCardNumber = string.Empty;
			object giftCardLock = new object();
			try
			{
				lock (giftCardLock)
				{
					var productVar = _clubRepo.GetVariant(barCode);
					int SeriesId = productVar.SerieId.Value;
					int walletId = productVar.WalletId.Value;
					int loadingAmount = productVar.LoadingAmount.Value;
					var cardSqeleton = _clubRepo.GetCardSkeleton(productVar.SerieId.Value.ToString());

					Nofshonit.Common.EF.Club.Cards card = new Nofshonit.Common.EF.Club.Cards
					{
						CardNumber = cardSqeleton.CardNumber,
						CardStatus = 1,
						CardType = 3,
						ActivationType = 9,
						ActivationTime = DateTime.Now,
						ActivationOp = 0,
						ExpiredCard = DateTime.Now.AddYears(5),
						Idmember = dtsId
					};
					var mwcSerie = _dtsOnLineRepo.GetMwcSeriesBySerieId(SeriesId);


					DischargeData dischargeObj = new DischargeData();
					dischargeObj.responseDeposit = dischargeObj.client.SingelDepositAsync(new VPayServiceWS.SingelDepositRequest()
					{
						request = new VPayServiceWS.SingelChargeDischargeRequest_DTS()
						{
							chargeDischarge = new VPayServiceWS.ChargeDischarge() { Amount = loadingAmount, WalletID = walletId },
							CardNumber = cardSqeleton.CardNumber,
							OrganizationID = ContextManager.CurrentOrganization().OrgId,
							TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId),
						}
					}).Result;

					if (dischargeObj.responseDeposit != null && dischargeObj.responseDeposit.SingelDepositResult.ResponseStatus_DTS == VPayServiceWS.ReponseStatuses_DTS.Succeeded)
					{
						long cardId = _clubRepo.AddCard(card);
						cardSqeleton.CardId = cardId;
						_clubRepo.UpdateCardSkeleton(cardSqeleton);
						return cardSqeleton.CardNumber;
					}
					else
						return "39";
				}
			}
			catch
			{

				return "";
			}
		}
		private string Encrypt(string plainText, string PasswordHash)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			byte[] bytes2 = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(_configuration.GetConfigByValue<string>(ConfigurationKey.SaltKey))).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.Zeros
			};
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes2, Encoding.ASCII.GetBytes(_configuration.GetConfigByValue<string>(ConfigurationKey.VIBaseKey)));
			byte[] inArray;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.FlushFinalBlock();
					inArray = memoryStream.ToArray();
					cryptoStream.Close();
				}

				memoryStream.Close();
			}

			return Convert.ToBase64String(inArray);
		}
		private string GetCoupon(string MemberId, string CuponStockId)
		{

			return "";
		}
		private long InsertOrders(PurchaseRequestDTO purchaseRequestDTO, List<ProductsVars> productsVars, long paymentId, OrderDll.GeneralOrdersItem order)
		{
			string creditCardLast4Digits = string.Empty;
			if (!string.IsNullOrEmpty(purchaseRequestDTO.CreditCard16Digits) && purchaseRequestDTO.CreditCard16Digits.Length == 16)
				creditCardLast4Digits = purchaseRequestDTO.CreditCard16Digits?.Substring(purchaseRequestDTO.CreditCard16Digits.Length - 4, 4);

			int index = 1;
			List<PurchaseRequestDTO.Variant> variants = purchaseRequestDTO.Cart.SelectMany(x => x.Variants).ToList();
			foreach (PurchaseRequestDTO.Variant variant in variants)
			{
				ProductsVars productVar = productsVars.FirstOrDefault(x => x.FullBarCode.Equals(variant.Barcode));
				int benefitTypeId = ProductFunctions.GetBenefitTypeIdByVariantType(productVar.VariantType);
				for (int i = 0; i < variant.Quantity; i++)
				{
					string metaData = string.Empty, dtsRedimCode = string.Empty;
					if (benefitTypeId == (int)BenefitType.Coupon)
					{
						DtsLoggger.Logger.Info($"Get coupon code ==> CuponStockID: {productVar.CuponStockId.GetValueOrDefault().ToString()}");
						string couponCode = _dtsOnLineRepo.GetCoupon(productVar.CuponStockId.GetValueOrDefault().ToString(), _contextManager.CurrentUser().Id);
						DtsLoggger.Logger.Info($"Get coupon code ==> couponCode: {couponCode}");

						metaData = $@"<Details><Price>{variant.Price}</Price><CouponStockID>{productVar.CuponStockId}</CouponStockID>
                                <StockID>{productVar.CuponStockId}</StockID><MemberID>{_contextManager.CurrentUser().Id}</MemberID><SendingTime>{DateTime.Now}</SendingTime><CouponCode>{couponCode}</CouponCode>
                                <isInternalCoupon>{true}</isInternalCoupon><catalogicPrice>{productVar.CatalogicPrice}</catalogicPrice><OrganizationPrice>{productVar.IrgunPriceFormula}</OrganizationPrice><Payments>{purchaseRequestDTO.NumOfPayments}</Payments>
                                <OrderMail>{purchaseRequestDTO.Email}</OrderMail><SMSPhoneNumber>{purchaseRequestDTO.Mobile}</SMSPhoneNumber><Phone>{purchaseRequestDTO.Mobile}</Phone><OrderTZ>{purchaseRequestDTO.identity}</OrderTZ><CC>{creditCardLast4Digits}</CC></Details>";

						dtsRedimCode = couponCode;
						if (couponCode == "-1")
							throw new BusinessException("ההזמנה לא נקלטה - וריאנט אזל מהמלאי");
					}
					else
					{
						metaData = $"<Details><Price>{variant.Price}</Price><catalogicPrice>{productVar.CatalogicPrice}</catalogicPrice><OrganizationPrice>{productVar.IrgunPriceFormula}</OrganizationPrice><Comission>{productVar.VarComissionFormula}</Comission><Discount>{productVar.VarDiscountFormula}</Discount><CancelComission>{productVar.CancelCommission}</CancelComission><Payments>{purchaseRequestDTO.NumOfPayments}</Payments>" +
										$"<OrderMail>{purchaseRequestDTO.Email}</OrderMail><SMSPhoneNumber>{purchaseRequestDTO.Mobile}</SMSPhoneNumber><Phone>{purchaseRequestDTO.Mobile}</Phone><OrderTZ>{purchaseRequestDTO.identity}</OrderTZ><CC>{creditCardLast4Digits}</CC></Details>";
						if (benefitTypeId == (int)BenefitType.GiftCard)
						{
							dtsRedimCode = GetGiftCardNumber(productVar.FullBarCode, _contextManager.CurrentUser().Id);
							if (dtsRedimCode.Equals("39"))
							{
								throw new BusinessException("ההזמנה לא נקלטה - טעינת כרטיס גיפט נכשלה");
							}
						}
						else
						{
							dtsRedimCode = _clubRepo.GetCardByMemberId(_contextManager.CurrentUser().Id);
						}
					}


					DtsLoggger.Logger.Info($"index: {i}, variant:{variant.Barcode}, dtsRedimCode:{dtsRedimCode}");
					string res = Main.InsertNewOrder(ContextManager.CurrentUser().PremiumType, _contextManager.CurrentOrganization().OrgId.ToString(), _contextManager.CurrentOrganization().Password, variant.Barcode, _contextManager.CurrentUser().Id, "1", purchaseRequestDTO.OrderGuid + "-" + index++, metaData, string.Empty, "192.168.111.120", paymentId.ToString(), 0, order, dtsRedimCode, null, "", null);
					DtsLoggger.Logger.Info($"Main.InsertNewOrder Finished: Index: {i}, Main.InsertNewOrder Code: {res}");

					int code;
					if (int.TryParse(res, out code))
					{
						if (code <= 300) // Amir: Greater than 300 means the code is the WebServiceTransaction ID.
							throw new Exception("InsertNewOrder Failed! Error Code: " + code);
					}


                    //try -- On comment REQ0143 - Liroy
                    //{
                    //	if (productVar.VariantType == 5 && productVar.PosbarCode != "638" && productVar.PosbarCode != "195")
                    //	{

                    //		string smsTxt = $"שלום {purchaseRequestDTO.FirstName}, מצורף קופון {dtsRedimCode} ל" +
                    //			$"{productVar.ShortNameVar}, הקופון בתוקף עד לתאריך {productVar.LastImplementationDate}. " +
                    //			$"יש להראות את ההודעה בבית העסק. מימוש השובר דרך מועדון לקוחות ביזנס לוג'יק";

                    //		BaseMethods.SendSMS(purchaseRequestDTO.Mobile, smsTxt, _contextManager.CurrentOrganization().DBName, 0, 120, _contextManager.CurrentOrganization().OrgId.ToString(), purchaseRequestDTO.identity, -1, 48);
                    //	}
                    //}
                    //catch (Exception ex)
                    //{
                    //	LoggerHelper.Error($"Failed to send sms to {purchaseRequestDTO.MemberId} for BL realization info {ex.Message}, StackTrace: {ex.StackTrace}");
                    //}
                }
            }
			return order.OrderId;
		}
	}
}
