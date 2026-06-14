using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nofshonit.Logs;


namespace Nofshonit.BL.Contact
{
    public class ContactUsBL : BaseBL, IContactUsBL
    {
        private bool configCRM;
        private IMapperManager _mapper;
        private List<int> messageKeys;

        public ContactUsBL():base()
        {
            _mapper = Container.Resolve<IMapperManager>();
            messageKeys = new List<int>();
        }

        public List<CrmGetTypeDTO> GetCrmTypes()
        {
            return Container.Resolve<IDtsOnlineRepo>().GetCrmTypes().Result;
        }

    
        public async Task<BaseResponse<object>> OpenServiceCaseRequest(ContactUsDTO contactUsDTO)
        {
            try
            {
                if(string.IsNullOrEmpty(contactUsDTO.Id))
                    contactUsDTO.Id = contactUsDTO.IdentityNumber;
                configCRM = Container.Resolve<Infrastructure.Configuration.IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.ContactUsPath);
                if (configCRM)
                {
                    var userFromDb = Container.Resolve<IClubRepo>().GetUserByUserID(contactUsDTO.Id).Result;
                    if (userFromDb != null)
                    {
                        var userConverted = (Crm)_mapper.Map(contactUsDTO, typeof(Crm));
                        bool? isUnitTest = ContainerManager.Container.Resolve<Infrastructure.Configuration.IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.IsUnitTest);
                        isUnitTest = (isUnitTest == null || (bool)!isUnitTest); //true if not unit tests
                        if (!(bool)isUnitTest)
                        {
                            userConverted.CrmDescription = "UnitTestDescription";
                        }
                        userConverted = CrmCreator(userConverted, contactUsDTO);
                        var userResponse = await Container.Resolve<IDtsOnlineRepo>().AddContactToCrmAsync(userConverted);
                        DtsLoggger.Logger.Info($"OpenServiceCaseRequest userResponse AddContactToCrmAsync memberid: {contactUsDTO.Id} is:{userResponse}");

                        if (userResponse)
                        {
                            messageKeys.Add(50);
                            messageKeys.Add(10221);
                            var emailQueue = EmailQueueCreator(EContactUsEmail.ToClientMail, (EmailQueue)_mapper.Map(contactUsDTO, typeof(EmailQueue)), contactUsDTO);
                            if (await Container.Resolve<IDtsOnlineRepo>().AddContactToEmailQueue(emailQueue))

                                return new BaseResponse<object>
                                {
                                    Status = true,
                                    Message = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10221).MessageText,
                                };

                        }
                        throw new Exception("Any problem with adding to CRM table.");
                    }
                    throw new BusinessException("המשתמש לא נמצא או הקלדת מספר זהות שגוי.");
                }
                var ConvertedEmailQueue = EmailQueueCreator(EContactUsEmail.ToServiceMail, (EmailQueue)_mapper.Map(contactUsDTO, typeof(EmailQueue)), contactUsDTO);
                if (await Container.Resolve<IDtsOnlineRepo>().AddContactToEmailQueue(ConvertedEmailQueue))
                {
                    messageKeys.Add(50);
                    messageKeys.Add(10221);
                    ConvertedEmailQueue = EmailQueueCreator(EContactUsEmail.ToClientMail, (EmailQueue)_mapper.Map(contactUsDTO, typeof(EmailQueue)), contactUsDTO);
                    if (await Container.Resolve<IDtsOnlineRepo>().AddContactToEmailQueue(ConvertedEmailQueue))
                        return new BaseResponse<object>
                        {
                            Status = true,
                            Message = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10221).MessageText
                        };
                }
                throw new Exception("Any problem with adding to EmailQueue table.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in OpenServiceCaseRequest memberid: {contactUsDTO.Id}, error is:{ex.Message}");
                throw new Exception("Any problem with adding to EmailQueue table.");


            }


        }

        private Crm CrmCreator(Crm crm, ContactUsDTO contactUsDTO)
        {
            crm.MemberId = contactUsDTO.Id;
            crm.OrgId = ContextManager.CurrentOrganization().OrgId;
            crm.CrmSeverityId = 1;
            crm.CrmSourceId = 3;
            crm.CrmStatusId = 1;
            //crm.OpIdCreate = 1771;
            crm.OpIdCreate = 2084;
            crm.DateCreate = DateTime.Now;
            crm.CrnEssence = EssencesStrings.ContactUsEssence;
            return crm;
        }

        private EmailQueue EmailQueueCreator(EContactUsEmail mailTo , EmailQueue emailQueue, ContactUsDTO contactUsDTO)
        {
            try
            {
                var serviceMail = ContextManager.CurrentOrganization().ServiceMail;
                emailQueue.EmailDateAdded = DateTime.Now;
                emailQueue.IsBodyHtml = true;
                emailQueue.SeveralAttempts = 0;
                emailQueue.IsSendEmail = false;

                if (mailTo == EContactUsEmail.ToServiceMail)
                {
                    emailQueue.EmailTo = serviceMail;
                    emailQueue.EmailFrom = contactUsDTO.InputEmail;
                }
                else
                {
                    emailQueue.EmailTo = contactUsDTO.InputEmail;
                    emailQueue.EmailFrom = serviceMail;
                }

                var clubName = ContainerManager.Container.Resolve<Infrastructure.Configuration.IConfigurationManager>().GetConfigByValue<string>(ConfigurationKey.ClubNameForSubject);
                emailQueue.EmailSubject = $"{clubName} , {string.Format(EssencesStrings.EmailQueueSubject, contactUsDTO.Subject)}";
                emailQueue.EmailBody = GenerateEmailBody(mailTo, contactUsDTO);
                emailQueue.EmailType = 100000 + ContextManager.CurrentOrganization().OrgId;
                emailQueue.EmailQueueUsersId = 2; //TEMPORARY!
                return emailQueue;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in EmailQueueCreator memberid: {contactUsDTO.Id}, error is:{ex.Message}");
                throw new Exception($"Error in EmailQueueCreator memberid: {contactUsDTO.Id}, error is:{ex.Message}");

            }

        }

        private string GenerateEmailBody(EContactUsEmail mailTo,ContactUsDTO contactUsDTO)
        {
            try
            {
                switch (mailTo)
                {
                    case EContactUsEmail.ToClientMail:
                        var message = new StringBuilder();

                        message.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
                        message.Append("<head>");
                        message.Append("</head>");
                        message.Append("<body dir='rtl'>");
                        message.Append(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 50).MessageText);
                        message.Append("</body>");
                        message.Append("</html>");
                        return message.ToString();

                    case EContactUsEmail.ToServiceMail:
                        message = new StringBuilder();
                        message.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
                        message.Append("<head>");
                        message.Append("</head>");
                        message.Append("<body dir='rtl'>");
                        //message.Append("מועדון: מילואים <br />");
                        message.Append("מועדון: " + ContextManager.CurrentOrganization().OrgName + "<br />");
                        message.Append("שם: " + contactUsDTO.FullName + "<br />");
                        message.Append("אימייל: " + contactUsDTO.InputEmail + "<br />");
                        message.Append("ת.ז.: " + contactUsDTO.IdentityNumber + "<br />");
                        if (string.IsNullOrEmpty(contactUsDTO.MobilePhone))
                            contactUsDTO.MobilePhone = "לא הוזן";
                        message.Append("טלפון: " + contactUsDTO.MobilePhone + "<br />");
                        contactUsDTO.ClubCreditCard = Container.Resolve<IClubRepo>().GetUserByUserID(contactUsDTO.Id).Result.ClubCreditCard;
                        //if (contactUsDTO.PremiumType == null)
                        //    throw new Exception("המשתמש לא נמצא או הקלדת מספר זהות שגוי.");

                        message.Append("מחזיק כרטיס אשראי: " + (contactUsDTO.ClubCreditCard == 0 ? "לא" : "כן") + "<br />");
                        message.Append("תוכן ההודעה: " + "<br />" + contactUsDTO.Description);
                        message.Append("</body>");
                        message.Append("</html>");
                        return message.ToString();

                    default:
                        throw new Exception("No valid EContactUsEmail. (GenerateEmailBody)");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in GenerateEmailBody memberid: {contactUsDTO.Id}, error is:{ex.Message}");
                throw new Exception($"Error in GenerateEmailBody memberid: {contactUsDTO.Id}, error is:{ex.Message}");
            }


        }

    }
}
