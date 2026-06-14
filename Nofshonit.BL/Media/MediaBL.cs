using Microsoft.AspNetCore.Http;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nofshonit.Repositories.DtsLogsModel;
using Nofshonit.Common.EF.DTS_Logs;
using Nofshonit.Common.DTOs.Enums;

namespace Nofshonit.BL.Media
{
    public class MediaBL : BaseBL, IMediaBL
    {
        //private IClubRepo _clubRepo;
        private IDtsOnlineRepo _dtsOnlineRepo;
        private readonly ICacheManager _cacheManager;
        private readonly IDtsLogsRepo _dtsLogsRepo;
        private int organizationId;

        public MediaBL() : base()
        {
            _dtsLogsRepo = Container.Resolve<IDtsLogsRepo>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _cacheManager = Container.Resolve<ICacheManager>();
            organizationId = ContextManager.CurrentOrganization().OrgId;
		}               

        public List<string> GetGreetingTypes()
        {
            List<string> types = new List<string>();

            types = _dtsOnlineRepo.GetGreetingTypes(organizationId).Result;

            return types;
        }

        public List<GreetingDTO> GetGreetings()
        {
            return _dtsOnlineRepo.GetGreetings(organizationId).Result;            
        }
        public List<string> GetMessagesByType(string type)
        {
            List<string> messages = new List<string>();

            messages = _dtsOnlineRepo.GetMessagesByType(organizationId, type).Result;

            return messages;
        }

		/// <summary>
		/// Returns the current organization news
		/// </summary>
		public string GetOrganizationNews()
		{
			return _dtsOnlineRepo.GetOrganizationNews(organizationId);
		}

		/// <summary>
		///  Returns the current organization imges silder from the db
		/// </summary>
		public List<ImageSliderDTO> GetImagesSlider()
		{
            var imagesSlider = _cacheManager.Get(CacheKeys.ImagesSlider) as List<ImageSliderDTO>;

            if (imagesSlider == null)
            {

                imagesSlider = _dtsOnlineRepo.GetImagesSlider(organizationId);
                _cacheManager.Set(CacheKeys.ImagesSlider, imagesSlider);
                
            }
            return _dtsOnlineRepo.GetImagesSlider(organizationId);
		}

		/// <summary>
		///  Returns a list of messages by the id's given
		/// </summary>
		/// <param name="IdList">The id's of the messages</param>
		public List<Messages> GetMessagesById(List<int> idList)
		{
            if (idList == null || idList.Count == 0)
                throw new Exception("ID List must be poulated.");
            return MessagesUtil.GetMessagesByID(idList);
		}

		/// <summary>
		///  Returns a list of messages by the key's given
		/// </summary>
		/// <param name="KeyList">The key's of the messages</param>
		public List<Messages> GetMessagesByKey(List<int> keyList)
		{
			return MessagesUtil.GetMessagesByKey(keyList);
		}

		/// <summary>
		///  Returns a list of messages by the context given
		/// </summary>
		/// <param name="context">The context of the messages</param>
		public List<Messages> GetMessagesByContext(string context)
		{
			return MessagesUtil.GetMessagesByContext(context);
		}

        public List<Messages> GetCommercial()
        {
            int? premiumType = ContextManager.CurrentUser().PremiumType;
            string context = premiumType.Value == (int)EIDFPremiumeType.Approved ? "Banner" : "Banner - ממשיכים";
            return GetMessagesByContext(context).OrderBy(m=> m.MessageId).ToList();
        }
        public bool AddReferralHistory(ReferralHistory data)
        {
            ReferralHistory newLine = new ReferralHistory();
            ResponseUserDTO currentUser = new ContextManager().CurrentUser();

            newLine.MemberId = currentUser.Id;
            newLine.LastUpdateMember = DateTime.Now;
            newLine.OrgId = (short)ContextManager.CurrentOrganization().OrgId;
            newLine.ExternalLlink = data.ExternalLlink;
            newLine.IneerLlink = data.IneerLlink;

            try
            {

                _dtsLogsRepo.AddReferralHistory(newLine);
            }
            catch (Exception e)
            {

            }
            return true;
        }

    }
}
