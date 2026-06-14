using Newtonsoft.Json.Linq;
using Nofshonit.BL.BLHelper;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.DTOs.ResponseDTOs;
using Nofshonit.BL.Exceptions;

using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nofshonit.Logs;

namespace Nofshonit.BL.Event
{
    public class EventBL : BaseBL, IEventBL
    {
        private readonly IContextManager _contextManager;
        private readonly IDtsOnlineRepo _dtsOnlineRepo;

        public EventBL() : base()
        {
            _contextManager = Container.Resolve<IContextManager>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
        }

        public List<EventDTO> GetEventsByCategoryId(long categoryId)
        {
            List<EventDTO> result = null;

            /*
            var request = new EventsByBenefitDTO()
            {
                BenefitId = categoryId.ToString(),
                MemberId = ContextManager.CurrentUser().MemberGuid.TrimEnd(),
                UniqueId = ContextManager.CurrentOrganization().OrganizationGuid.TrimEnd(),
            };

            var eventsByBenefitJResult = EventsByBenefit(request).Result;
            var json = eventsByBenefitJResult.ToString();
            var responseEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseData<GetEventsByBenefitIdResponse>>(json).data;
            if (responseEvent != null && responseEvent.Events != null)
            {
                result = responseEvent.Events.ConvertAll(x => new EventDTO()
                {
                   CategoryTypeId = x.BenefitTypeId,
                   EventDate = x.EventDate,
                   EventId = x.EventId,
                   EventTime =x.EventTime,
                   ExpireDate = x.ExpireDate,
                   IframeUrl = x.IframeUrl,
                   IsSendToFriend =x.IsSendToFriend,
                   Name =x.Name,
                   OrderLimit = x.OrderLimit,
                   VenueId =x.VenueId,
                  VenueName = x.VenueName,        
                  IsCampaign = x.IsCampaign,
                });
            }

            */

            GetEventsRequestDTO request = new GetEventsRequestDTO
            {
                CategoryNum = categoryId.ToString(),
                OrganizationId = ContextManager.CurrentOrganization().OrgId.ToString(),
            };

            var resultEvents = HttpRequestManager.HttpRequestGet(HttpUrls.TicketHubURL + TicketHubClientKeys.GetActiveEvents, request);
            var json = resultEvents.Result.ToString();
            var responseEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<ShellTicketHub<List<ActivesEventsResponseDTO>>>(json);
            if (responseEvent != null && responseEvent.Data != null && responseEvent.Data.Count > 0)
            {
                result = responseEvent.Data.ConvertAll(x => new EventDTO()
                {
                    CategoryId = categoryId,
                    EventDate = x.EventDate,
                    EventId = x.EventId,
                    EventTime = x.EventTime,
                    ExpireDate = x.EventDate,
                    Name = string.IsNullOrWhiteSpace(x.Title) ? x.VeneueName : x.Title,
                    OrderLimit = 1,
                    VenueId = x.VenueId,
                    VenueName = x.VeneueName,
                    IsCampaign = false,
                });
            }
            else if(responseEvent != null && responseEvent.Data != null)
            {
                result = new List<EventDTO>();
            }
            return result;
        }

        public CatalogDTO EventCatalog(int eventId,long categoryId)
        {
            CatalogDTO model = new CatalogDTO();
            var organizationId = _contextManager.CurrentOrganization().OrgId;
            var memberGuid = _contextManager.CurrentUser().MemberGuid;
            EventCatalogRequest request = new EventCatalogRequest()
            {
                EventId = eventId,
                MemberId = memberGuid,
                OrganizationId = organizationId,

            };
           // var categoryId = Container.Resolve<IClubRepo>().CategoryIdByEventId(eventId);           
            model.Category = _dtsOnlineRepo.GetCategoryDetails(categoryId, 0, organizationId);
            List<EventCatalogResponseDTO> result = null;
            var resultEventCataog = HttpRequestManager.HttpRequestGet(HttpUrls.TicketHubURL + TicketHubClientKeys.EventCatalog, request);
            var json = resultEventCataog.Result.ToString();
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ShellTicketHub<List<EventCatalogResponseDTO>>>(json);
            if (response != null && response.Data != null && response.Data.Count > 0)
            {
                result = response.Data;
                model.Category.isSelfPrint = response.Data[0].IsSelfPrint;
                model.JourneyId = response.JourneyId;
                var mapJson = EventFunctions.GetMapJson(response.Data);
                if (!string.IsNullOrEmpty(mapJson) && response.Data.Any(x=>x.IsSeatMap))
                {
                    MapHtmlRequest htmlRequest = new MapHtmlRequest() { MapJson = mapJson };
                    var mapHtmlResponse = HttpRequestManager.HttpRequest(HttpUrls.TicketHubURL + TicketHubClientKeys.GetMapHtml, htmlRequest, HttpMethod.Post);

                    var jsonHtml = mapHtmlResponse.Result.ToString();
                    var responseHtml = Newtonsoft.Json.JsonConvert.DeserializeObject<ShellTicketHub<string>>(jsonHtml);
                    if (responseHtml != null && responseHtml.Data != null)
                    {
                        model.MapHTML = responseHtml.Data;
                    } else
                    {
                        var errorMessage = responseHtml != null ? responseHtml.ErrorMessage : "";
                        throw new Exception("Error while getting map Map. Error: " + errorMessage);
                    }

                }
                //List<CartVarsDTO> varList = Container.Resolve<IShopingBasketBL>().GetCartVars(Container.Resolve<IShopingBasketBL>().GetCart().Result);
                //var cardLimits = Container.Resolve<ILimitationsBL>().ValidatePurchesAllowed(varList).ToDictionary(x => x.Barcode, x => x.OrderLimit);
                //var eventsWithCards = result.Where(x => cardLimits.ContainsKey(x.FullBarCode)).ToList();
                //foreach (var eventsWithCard in eventsWithCards)
                //{
                //    var limit = eventsWithCard.OrderLimit;
                //    cardLimits.TryGetValue(eventsWithCard.FullBarCode, out limit);
                //    eventsWithCard.OrderLimit = limit;
                //}
            }
            else if (response != null && response.Data != null)
            {
                result = new List<EventCatalogResponseDTO>();
            }
            else if(response != null && !string.IsNullOrEmpty( response.ErrorMessage))
            {
                throw new BusinessException(response.ErrorMessage);
            }
            model.EventCatalogList = result.OrderBy(x => x.FinalPrice).ToList();

            return model;
        }
        public ShellTicketHub<object> CanselOrder(string eventGuid)
        {
            BookOrderRequest bookOrderRequest = new BookOrderRequest() { OrderGuid = eventGuid };
            var result = HttpRequestManager.HttpRequest(HttpUrls.TicketHubURL + TicketHubClientKeys.CancelOrder, bookOrderRequest, HttpMethod.Post).Result;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ShellTicketHub<object>>(result.ToString());
        }
        public ShellTicketHub<object> BookOrder(string eventGuid)
        {
            BookOrderRequest bookOrderRequest = new BookOrderRequest() { OrderGuid =  eventGuid };
            var result = HttpRequestManager.HttpRequest(HttpUrls.TicketHubURL + TicketHubClientKeys.BookOrder, bookOrderRequest, HttpMethod.Post).Result;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ShellTicketHub<object>>(result.ToString());
        }
        public CreateReservationResponseDTO CreateReservation(CreateReservationRequest requestBase)
        {  
            CreateReservationRequestDTO request = new CreateReservationRequestDTO(requestBase);
            request.MemberId = _contextManager.CurrentUser().MemberGuid;
            request.OrganizationId = _contextManager.CurrentOrganization().OrgId;
            CreateReservationResponseDTO result = null;
            var resultEventCataog = HttpRequestManager.HttpRequest(HttpUrls.TicketHubURL + TicketHubClientKeys.CreateReservation, request,HttpMethod.Post);
            var json = resultEventCataog.Result.ToString();
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ShellTicketHub<CreateReservationResponseDTO>>(json);


            if (response != null && response.Data != null && response.Data != null)
            {
                result = response.Data;
            }
            else if(response != null && !string.IsNullOrEmpty( response.ErrorMessage))
            {
                throw new BusinessException(response.ErrorMessage);
            }

            if (response.Data.OrderTickets.Count != requestBase.ListOfSeats.Count)
            {
                try
                {

                    LoggerHelper.Error("Error On CreateReservation response.Data.OrderTickets.Count != requestBase.ListOfSeats.Count ");
                    LoggerHelper.Error("reques => {0}", $"{Newtonsoft.Json.JsonConvert.SerializeObject(requestBase)}");
                    LoggerHelper.Error("response => {0}", $"{json}");
                }
                catch(Exception e) { LoggerHelper.Error(e, "Error on CreateReservation"); }

            }


            return result;
        }
    }
}
