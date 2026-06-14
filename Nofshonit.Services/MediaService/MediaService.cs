using Nofshonit.BL.Media;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.EF.DTS_Logs;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Services.MediaService
{
    /// <summary>
    /// service for Greeting messages
    /// </summary>
    public class MediaService : BaseService, IMediaService
    {
        private IMediaBL _mediaBL;

        public MediaService()
        {
            _mediaBL = Container.Resolve<IMediaBL>();
        }

        public List<string> GetGreetingTypes()
        {
            return _mediaBL.GetGreetingTypes();
        }

        public List<GreetingDTO> GetGreetings()
        {
            return _mediaBL.GetGreetings();
        }

        public List<string> GetMessagesByType( string type)
        {
            return _mediaBL.GetMessagesByType(type);
        }

		public string GetOrganizationNews()
		{
			return _mediaBL.GetOrganizationNews();
		}

		public List<ImageSliderDTO> GetImagesSlider()
		{
			return _mediaBL.GetImagesSlider();
		}

		public List<Messages> GetMessagesById(List<int> idList)
		{
			return _mediaBL.GetMessagesById(idList);
		}

		public List<Messages> GetMessagesByKey(List<int> keyList)
		{
			return _mediaBL.GetMessagesByKey(keyList);
		}

		public List<Messages> GetMessagesByContext(string context)
		{
			return _mediaBL.GetMessagesByContext(context);
		}
        public List<Messages> GetCommercial()
        {
            return _mediaBL.GetCommercial();
        }
        public bool AddReferralHistory(ReferralHistory data)
        {
            return _mediaBL.AddReferralHistory(data);
        }
    }
}
