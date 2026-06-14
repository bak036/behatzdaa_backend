using Nofshonit.BL.BLHelper;
using Nofshonit.BL.Purchase.PurchaseUtils.PurchaseHandler;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Nofshonit.Logs;


namespace Nofshonit.BL.Purchase
{
    public class PurchaseBL: BaseBL, IPurchaseBL
	{
        private IClubRepo _clubRepo;
        private IConfigurationManager _config;
        private IMapperManager _mapper;
        private IPurchaseHandler _purchaseHandler;
        private List<int> messageKeys;

        public PurchaseBL() : base()
        {
            _clubRepo = Container.Resolve<IClubRepo>();
            _mapper = Container.Resolve<IMapperManager>();
            _config = Container.Resolve<IConfigurationManager>();
            _purchaseHandler = Container.Resolve<IPurchaseHandler>();
            messageKeys = new List<int>() { 10007 , 10304 , 10305, 10303 , 10048 };
        }
        public async Task<PurchaseHistoryResponseDTO> PurchaseHistoryAsync(MemberHistoryDTO memberHistoryDTO)
        {
            try
            {
                var memberHistoryFullDTO = new MemberHistoryRequestDTO(memberHistoryDTO, ContextManager.CurrentUser().MemberGuid.TrimEnd(), ContextManager.CurrentOrganization().OrganizationGuid.TrimEnd());
                return new PurchaseHistoryResponseDTO
                {
                    Variants = _purchaseHandler.GetHistoryForMember(memberHistoryDTO),
                    Status = 1,
                    FromDate = memberHistoryDTO.FromDate,
                    ToDate = memberHistoryDTO.ToDate,
                    MemberId = ContextManager.CurrentUser().Id,
                    ErrorId = 0,
                };
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in PurchaseHistoryAsync, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10007).MessageText);
            }
        }

        public string PurchaseHistoryOld()
        {
            return Cryptor.ConvertStringToHex(Cryptor.Encrypt(ContextManager.CurrentUser().Id + DateTime.Today.ToString()), System.Text.Encoding.Unicode);
        }

        public async Task<string> SendOldOrdersLinkToUnauthorizedUser(string memberId)
        {
            

            var member = await _clubRepo.GetUserByUserID(memberId);
            if (member == null)
                return("לא נמצאה זכאות למידע נוסף יש לפנות לשירות לקוחות");
             
            string phonenumber = (string.IsNullOrEmpty( member.PhoneNumber ) || member.PhoneNumber.Length <10 )? member.MobilePhone : member.PhoneNumber;

            //אין טלפון תקין לשליחת הודעה
            if ( string.IsNullOrEmpty(phonenumber) || phonenumber.Length <  10)
                return (MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10304).MessageText);
            string phonenMask = phonenumber.Substring(6).PadRight(10, '*');
            //נשלח למשתמש כבר הודעה ב 10 דקות האחרונות
            if (CacheManager.Exists(string.Format(CacheKeys.Send_OldOrders_SMS_ToMember, memberId)))
                return String.Format(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10305).MessageText, phonenMask);

            string oldOrdersHistoryLink = _config.GetConfigByValue<string>(ConfigurationKey.OldMiluimOrdersUrl);
            string shortUrlBase = _config.GetConfigByValue<string>(ConfigurationKey.ShortUrlBase);
            string userToken = Cryptor.ConvertStringToHex(Cryptor.Encrypt(member.MemberId + DateTime.Today.ToString()), System.Text.Encoding.Unicode);
            string shortUrlParam = _clubRepo.GetOrCreateShortUrl(oldOrdersHistoryLink+userToken, member.MemberId);

            //תקלה טכנית
            if (string.IsNullOrEmpty(shortUrlParam)  )
                return (MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10048).MessageText);

            string message = $"שלום {member.MemberName?? $"{member.MemberFirstName} {member.MemberLastName}"}, לצפייה בפרטי ההזמנות מאתר \"בהצדעה\" הישן לחץ על הקישור: " +$"{shortUrlBase}/{shortUrlParam}";
            Container.Resolve<IDtsOnlineRepo>().AddSmsQueue(new Common.EF.DTS_Online.SmsQueue()
            {
                MemberId = member.MemberId,
                DateAdded = DateTime.Now,
                OrganizationId = ContextManager.CurrentOrganization().OrgId,
                SenderName = "Behatsdaa",
                Subscribers = phonenumber,
                Message = message,
                MessageLengh = message.Length,
                DeliveryDelayInMinutes = 0,
                ExpirationDelayInMinutes = 120,
                SmsType = (byte)20,
                SeveralAttempts = 0,
                Priority = 0,
                SmsSend = false,
            });

            CacheManager.Set(string.Format(CacheKeys.Send_OldOrders_SMS_ToMember, member.MemberId), message, TimeSpan.FromMinutes(10));
            //"הקישור נשלח בהצלחה";
            return string.Format( MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10303).MessageText, phonenMask);
             
        } 
       

    }
}
