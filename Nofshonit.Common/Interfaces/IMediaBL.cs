using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.EF.DTS_Logs;
using Nofshonit.Common.EF.DTS_Online;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nofshonit.Common.EF.DTS_Logs;
namespace Nofshonit.Common.Interfaces
{
    public interface IMediaBL
    {
        List<string> GetGreetingTypes();
        List<GreetingDTO> GetGreetings();
        List<string> GetMessagesByType(string type);
		string GetOrganizationNews();
		List<ImageSliderDTO> GetImagesSlider();
		List<Messages> GetMessagesById(List<int> idList);
		List<Messages> GetMessagesByKey(List<int> keyList);
		List<Messages> GetMessagesByContext(string context);
        List<Messages> GetCommercial();
        bool AddReferralHistory(ReferralHistory data);
    }
}
