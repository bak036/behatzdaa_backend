using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Logs;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofshonit.Repositories.Helpers
{
    public static class ProductFunctions
    {
        public static decimal? GetVariantPrice(int? premiumType ,ProductsVars productsVar, BusinessSubTypeSpecificationByVariants specByVar, BusinessSubTypeSpecificationCurrent specCurrent, bool isClubCreditCard, bool orgIsNewSubsidy)
        {
            decimal? price = null;

            if (productsVar != null)
            {
                bool paymentModelValid = true;
                var validPaymentModel = new List<int> { 1, 3, 4 };//MoneyOnly,BothStatic,BothDynamic
                int paymentModel = productsVar.PaymentModelId.GetValueOrDefault(1);
                paymentModelValid = validPaymentModel.Contains(paymentModel);

                if (paymentModelValid)
                {
                    if (orgIsNewSubsidy)
                    {
                        decimal subsidy;
                        decimal orgPrice = decimal.Parse(productsVar.IrgunPriceFormula);

                        //If manual subsidy or campaign take from specByVar
                        if (((productsVar.ManualSubsidy.GetValueOrDefault()) || (productsVar.Iscampaign.GetValueOrDefault())) && specByVar != null)
                        {
                            if (premiumType == 3)
                            {
                                if (specByVar.ShekelSubsidyForCardHoldersVar.GetValueOrDefault() > 0)
                                {
                                    subsidy = decimal.Parse(specByVar.ShekelSubsidyForCardHoldersVar.GetValueOrDefault().ToString());
                                }
                                else
                                {
                                    subsidy = (decimal.Parse(specByVar.SubsidyPrecentForCardHoldersVar.GetValueOrDefault().ToString()) * orgPrice) / 100;
                                }

                                if (specCurrent.MaxValueForCardHolders.HasValue && subsidy > specCurrent.MaxValueForCardHolders.Value)
                                {
                                    subsidy = specCurrent.MaxValueForCardHolders.Value;
                                }
                            }
                            else
                            {
                                if (specByVar.ShekelSubsidyRegularVar.GetValueOrDefault() > 0)
                                {
                                    subsidy = decimal.Parse(specByVar.ShekelSubsidyRegularVar.GetValueOrDefault().ToString());
                                }
                                else
                                {
                                    subsidy = (decimal.Parse(specByVar.SubsidyPrecentRegularVar.GetValueOrDefault().ToString()) * orgPrice) / 100;
                                }

                                if (specCurrent.MaxValueRegular.HasValue && subsidy > specCurrent.MaxValueRegular.Value)
                                {
                                    subsidy = specCurrent.MaxValueRegular.Value;
                                }
                            }
                        }
                        else //Take from specCurrent
                        {
                            if (premiumType == 3)
                            {
                                if (specCurrent.ShekelSubsidyForCardHolders.GetValueOrDefault() > 0)
                                {
                                    subsidy = decimal.Parse(specCurrent.ShekelSubsidyForCardHolders.GetValueOrDefault().ToString());
                                }
                                else
                                {
                                    subsidy = (decimal.Parse(specCurrent.SubsidyPrecentForCardHolders.GetValueOrDefault().ToString()) * orgPrice) / 100;
                                }
                                // Max For CardHolders
                                if (specCurrent.MaxValueForCardHolders.HasValue && subsidy > specCurrent.MaxValueForCardHolders.Value)
                                {
                                    subsidy = specCurrent.MaxValueForCardHolders.Value;
                                }
                            }
                            else
                            {
                                
                                if (specCurrent.ShekelSubsidyRegular.GetValueOrDefault() > 0)
                                {
                                    subsidy = decimal.Parse(specCurrent.ShekelSubsidyRegular.GetValueOrDefault().ToString());
                                }
                                else
                                {
                                    subsidy = (decimal.Parse(specCurrent.SubsidyPrecentRegular.GetValueOrDefault().ToString()) * orgPrice) / 100;
                                }
                                // Max For Non CardHolders
                                if (specCurrent.MaxValueRegular.HasValue && subsidy > specCurrent.MaxValueRegular.Value)
                                {
                                    subsidy = specCurrent.MaxValueRegular.Value;
                                }
                            }
                        }

                        price = Math.Ceiling(orgPrice - subsidy);

                    }
                    else if (paymentModel == 1)//MoneyOnly
                    {
                        if (string.IsNullOrEmpty(productsVar.IrgunPriceFormula))
                        {
                            price = 0;
                        }
                        else
                        {
                            price = Math.Ceiling(decimal.Parse(productsVar.IrgunPriceFormula));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(productsVar.VarDiscountFormula))
                        {
                            price = decimal.Parse(productsVar.IrgunPriceFormula);
                        }
                        else
                        {
                            price = decimal.Parse(productsVar.IrgunPriceFormula) - decimal.Parse(productsVar.VarDiscountFormula);
                        }
                    }
                }
                else
                {
                    price = 0;
                }
            }



            return price;
        }

        public static int GetVariantRedimTypeId(ProductsVars productsVar, SimpleInt categoryRedimType)
        {
            int redimTypeId = productsVar.RedimTypeId.GetValueOrDefault(-1);
            if (redimTypeId <= 0 && categoryRedimType != null)
            {
                redimTypeId = categoryRedimType.Id;
            }

            return redimTypeId;
        }
        public static string GetVariantRedimTypeName(ProductsVars productsVar, SimpleInt categoryRedimType, Dictionary<int, string> RedimTypesByOrganization)
        {
            string redimTypeName = string.Empty;
            if (!RedimTypesByOrganization.TryGetValue(productsVar.RedimTypeId.GetValueOrDefault(-1), out redimTypeName) && categoryRedimType != null)
            {
                redimTypeName = categoryRedimType.Name;
            }

            return redimTypeName;
        }

        /// <summary>
        /// Get benefit type id according to variantType.
        /// </summary>
        /// <param name="productVar"></param>
        /// <returns></returns>
        public static int GetBenefitTypeIdByVariantType(int? variantType)
        {
            var type = (int)BenefitType.NormalVariant;
            if (variantType == 11)
            {
                type = (int)BenefitType.GiftCard;
            }
            else if (variantType == 12)
            {
                type = (int)BenefitType.Shows;
            }
            else if (variantType == 2)
            {
                type = (int)BenefitType.Coupon;
            }
            else
            {
                type = (int)BenefitType.NormalVariant;
            }

            return type;
        }

        public enum BenefitType
        {
            GiftCard = 1,
            NormalVariant = 2,
            Coupon = 3,
            Shows = 10,
            LeumiSpecial = 11
        }



        public static bool CheckVariantExpired(ProductsVars productVar)
        {
            bool isExpired = false;

            if (productVar != null)
            {
                if (productVar.LastImplementationDate.HasValue)
                {
                    isExpired = productVar.LastImplementationDate.Value.Date < DateTime.Now.Date;
                }
            }

            return isExpired;
        }

        public static DateTime ReCalculateLastImplementationDate(DateTime lastImplementationDate, int implementationType, List<Common.EF.DTS_Online.TypeImplementationDate> implementationTypesList)
        {
            var result = DateTime.Now;

            if (implementationType == 1)
            {
                result = new DateTime(lastImplementationDate.Year, lastImplementationDate.Month, lastImplementationDate.Day, 23, 59, 59);
            }
            else
            {
                lastImplementationDate = DateTime.Now;
                var impType = implementationTypesList.FirstOrDefault(x => x.Id == implementationType);
                if (impType != null)
                {

                    switch (impType.NumToImplement)
                    {
                        case 0: //days
                            result = DateTime.Now.AddDays(impType.TypeCalc);
                            break;

                        case 1: //month
                            result = DateTime.Now.AddMonths(impType.TypeCalc);
                            if (impType.TypeImplement.Trim() == "1")// שוטף
                            {
                                int days = DateTime.DaysInMonth(result.Year, result.Month);
                                result = new DateTime(result.Year, result.Month, days, 23, 59, 59);
                            }
                            break;

                        case 2: //year
                            result = DateTime.Now.AddYears(impType.TypeCalc);
                            if (impType.TypeImplement.Trim() == "1")
                            {
                                result = new DateTime(result.Year, 12, 31, 23, 59, 59);
                            }
                            break;
                        default:
                            result = DateTime.MaxValue;
                            break;
                    }



                }
                else
                {
                    result = new DateTime(2030, 1, 1);
                }
            }         
            return result;
        }

        public static EOrderStatus GetOrderStatus(IDtsOnlineRepo _dtsOnlineRepo, int benefitTypeId, v_wAllOrders allOrder)
        {
            var status = EOrderStatus.NotImplemented;
            if (allOrder == null)
                return status; //testonly
            try
            {
                long asmachta = allOrder.OrderAsmchta;
                DateTime? lastImplementationDate = allOrder.LastImplementationDate;
                short orderQuantity = allOrder.OrderQuantity;
                short orderBalance = allOrder.OrderBalance;
                List<long> inProCancel = _dtsOnlineRepo.GetCouponCancelRequestByMemberId(allOrder.MemberId).Select(p => p.Asmachta).ToList();
                bool inProc = inProCancel.Contains(asmachta);
                if (orderQuantity == 0 && orderBalance == 0)
                {
                    status = EOrderStatus.Canceled;
                }
                else if (allOrder.TransferToFriendDate != null)
                {
                    status = EOrderStatus.TransferredAsGift;
                }
                else if (allOrder.IsInCancelProcess || inProc)
                {
                    status = EOrderStatus.InCancelProcess;
                }
                else if (benefitTypeId == (int)BenefitType.GiftCard)
                {
                    var cardNumber = allOrder.CardNumber;
                    if (_dtsOnlineRepo.MwcMediasExecuteQuery(query => query.Count(x => x.CardNumber.Equals(cardNumber))) <= 1)
                    {
                        status = EOrderStatus.NotImplemented;
                    }
                    else if (_dtsOnlineRepo.MwcMediasExecuteQuery(query => query.Where(x => x.CardNumber.Equals(cardNumber)).Sum(x => x.Amount)) == 0)
                    {
                        status = EOrderStatus.Implemented;
                    }
                    else
                    {
                        status = EOrderStatus.PartiallyImplemented;
                    }

                }
                else
                {
                    if ((orderQuantity > 0 && orderBalance > 0) || allOrder.IMPLEMENTATION_IN_PROCESS)
                    {
                        status = EOrderStatus.Implemented;
                    }

                    else if (lastImplementationDate.HasValue && lastImplementationDate.Value < DateTime.Now)
                    {
                        status = EOrderStatus.Expired;
                    }
                    else if (orderQuantity > 0 && orderBalance == 0)
                    {
                        status = EOrderStatus.NotImplemented;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, $"Error in GetBenefitStatusId, error message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
            return status;
        }

        public static bool IsCancelable(IsCancelableModel model)
        {
            bool cancelable = false;
            bool daysRangeCheck = false;
            try
            {
                //If implemented
                if (model.Status.Equals(EOrderStatus.Implemented))
                {
                    //Check if variant gets implemented automatically
                    if (model.AutoImplementaionAfterReport)
                        daysRangeCheck = true;
                }
                else if (model.Status.Equals(EOrderStatus.NotImplemented))
                {
                    //If shows variant 
                    if (model.BenefitTypeId == (int)BenefitType.Shows)
                    {
                        if (DateTime.Now.AddDays(model.DaysBeforeShowToAllowCancel) < model.ShowDate) daysRangeCheck = true;
                    }
                    else
                        daysRangeCheck = true;
                }

                if (daysRangeCheck)
                {
                    if (model.TTransactionDateTime.AddDays(model.DaysRangeToCancel) > DateTime.Now)
                        cancelable = true;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in IsCancelable, error message: {ex.Message}, stacktrace: {ex.StackTrace}");
            }
            return cancelable;
        }
    }
}
