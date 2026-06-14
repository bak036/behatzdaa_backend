using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Nofshonit.Common;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Logs;
using Nofshonit.Repositories.ClubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TicketsHubRepository;
using TicketsHubRepository.EF.TicketsHub;
using static Nofshonit.Common.DTOs.PurchaseHistoryResponseDTO;

namespace Nofshonit.BL.Event
{
    public static class EventFunctions
    {
        public static string GetMapJson(List<EventCatalogResponseDTO> response)
        {
            var mapJson = "";
            var result = "";
            var ticketTypes = new List<int>();
            var priceIds = new List<int>();
            var finalPriceByPriceLevelID = new Dictionary<int, int>();
            var finalPriceByPriceId = new Dictionary<int, int>();
            var first = response.FirstOrDefault();
            if (first != null)
            {
                mapJson = first.MapJson;
                //mapJson = "{'SystemID':1,'EventID':10707,'Currency':'ILS','CustomPrices':{'Prices':[{'PriceID':96034,'OutletFee':60,'BasicPrice':250,'EndPrice':310,'BasicPriceVATKey':20,'PriceType':'V','SystemFeeVATKey':20,'ForeignID':98140},{'PriceID':96035,'OutletFee':60,'BasicPrice':250,'EndPrice':310,'BasicPriceVATKey':20,'PriceType':'V','SystemFeeVATKey':20,'ForeignID':98142},{'PriceID':96036,'OutletFee':60,'BasicPrice':250,'EndPrice':310,'BasicPriceVATKey':20,'PriceType':'V','SystemFeeVATKey':20,'ForeignID':98144},{'PriceID':96037,'OutletFee':40,'BasicPrice':200,'EndPrice':240,'BasicPriceVATKey':20,'PriceType':'V','SystemFeeVATKey':20,'ForeignID':98141},{'PriceID':96038,'OutletFee':40,'BasicPrice':200,'EndPrice':240,'BasicPriceVATKey':20,'PriceType':'V','SystemFeeVATKey':20,'ForeignID':98143},{'PriceID':96039,'OutletFee':40,'BasicPrice':200,'EndPrice':240,'BasicPriceVATKey':20,'PriceType':'V','SystemFeeVATKey':20,'ForeignID':98145}],'PriceLevels':[{'PriceLevelID':17648,'Number':1,'ForeignID':15960,'Text':'רמה 1','Currency':'ILS','PriceLevelPrice':310,'Promotions':[{'PromotionID':0,'PriceLevelPriceMin':310,'PriceLevelPriceMax':310,'Holds':[{'Number':0,'FreeSeats':{'PriceLevelID':17648,'TotalSeats':80,'TotalFreeSeats':74,'MaxFreeConnectedSeats':15,'SeatMapSeatType':'DontCare'},'TicketTypes':[{'TicketTypeID':44221,'Number':1,'Text':'מסובסד','ForeignID':43544,'MinTicketsPerOrder':0,'MaxTicketsPerOrder':0,'Code':'1','ItalyFiscalCode':'1','OnlyInGroupsOfPerOrder':0,'Flags':{},'PriceID':96034},{'TicketTypeID':44222,'Number':2,'Text':'לא מסובסד','ForeignID':43545,'MinTicketsPerOrder':0,'MaxTicketsPerOrder':0,'Code':'2','ItalyFiscalCode':'2','OnlyInGroupsOfPerOrder':0,'Flags':{},'PriceID':96035},{'TicketTypeID':44223,'Number':3,'Text':'מבצע','ForeignID':43546,'MinTicketsPerOrder':0,'MaxTicketsPerOrder':0,'Code':'3','ItalyFiscalCode':'3','OnlyInGroupsOfPerOrder':0,'Flags':{},'PriceID':96036}]}]}]},{'PriceLevelID':17649,'Number':2,'ForeignID':15961,'Text':'רמה 2','Currency':'ILS','PriceLevelPrice':240,'Promotions':[{'PromotionID':0,'PriceLevelPriceMin':240,'PriceLevelPriceMax':240,'Holds':[{'Number':0,'FreeSeats':{'PriceLevelID':17649,'TotalSeats':48,'TotalFreeSeats':35,'MaxFreeConnectedSeats':5,'SeatMapSeatType':'DontCare'},'TicketTypes':[{'TicketTypeID':44221,'Number':1,'Text':'מסובסד','ForeignID':43544,'MinTicketsPerOrder':0,'MaxTicketsPerOrder':0,'Code':'1','ItalyFiscalCode':'1','OnlyInGroupsOfPerOrder':0,'Flags':{},'PriceID':96037},{'TicketTypeID':44222,'Number':2,'Text':'לא מסובסד','ForeignID':43545,'MinTicketsPerOrder':0,'MaxTicketsPerOrder':0,'Code':'2','ItalyFiscalCode':'2','OnlyInGroupsOfPerOrder':0,'Flags':{},'PriceID':96038},{'TicketTypeID':44223,'Number':3,'Text':'מבצע','ForeignID':43546,'MinTicketsPerOrder':0,'MaxTicketsPerOrder':0,'Code':'3','ItalyFiscalCode':'3','OnlyInGroupsOfPerOrder':0,'Flags':{},'PriceID':96039}]}]}]}],'CreditLimits':[{'Currency':'ILS','NoCreditLimitCheck':true}],'ClientEventActivations':[{'ClientID':258,'From':'2019-02-26T10:00:00Z','To':'2019-08-02T03:00:00Z','SalesType':'OutletSale','MaxTicketsPerOrder':8,'ShowHolds':false,'ActivatedHolds':'1-9999','MaxReservationMinutes':17295,'Flags':{'WillCallCreate':true,'WillCallPrint':true,'WillCallHijack':true,'TicketTypeChangeable':true,'ReseatAllowed':false,'BestSeatAvoidGapSeats':false,'BestSeatArmageddon':true}}],'MemberCardLinks':[]}}";
            }
            if (!string.IsNullOrEmpty(mapJson))
            {
                var map = JsonConvert.DeserializeObject<JsonMap>(mapJson);
                PrapareMapJasonArray(response, ref finalPriceByPriceLevelID, ref finalPriceByPriceId,ref ticketTypes, ref priceIds);

                if (map != null && map.CustomPrices != null)// && Object.keys(map.CustomPrices).length > 0
                {
                    var customPrices = map.CustomPrices;
                    if (customPrices.PriceLevels != null && customPrices.PriceLevels.Length > 0)
                    {
                        var priceLevels = customPrices.PriceLevels;
                        for (var i = 0; i < priceLevels.Length; i++)
                        {
                            var pricelevel = priceLevels[i];
                            if (finalPriceByPriceLevelID.Keys.Any(x => x == pricelevel.PriceLevelID) && pricelevel.Promotions.Any(p=> p.Holds.Any(h=>h.FreeSeats.TotalFreeSeats > 0)))
                            {
                                pricelevel.Promotions[0].PriceLevelPriceMax = finalPriceByPriceLevelID[pricelevel.PriceLevelID];
                                pricelevel.PriceLevelPrice = finalPriceByPriceLevelID[pricelevel.PriceLevelID];
                            }
                            else
                            {
                                var list = customPrices.PriceLevels.ToList();
                                list.Remove(pricelevel);
                                customPrices.PriceLevels = list.ToArray();
                            }
                            if (pricelevel.Promotions != null && pricelevel.Promotions.Length > 0)
                            {
                                var promotions = pricelevel.Promotions;
                                for (var j = 0; j < promotions.Length; j++)
                                {
                                    var promotion = promotions[j];
                                    if (promotion.Holds != null && promotion.Holds.Length > 0)
                                    {
                                        var holds = promotion.Holds;
                                        for (var x = 0; x < holds.Length; x++)
                                        {
                                            var hold = holds[x];
                                            if (hold.TicketTypes != null && hold.TicketTypes.Length > 0)
                                            {
                                                var currentTicketTypes = hold.TicketTypes.ToList();
                                                var indexToRemove = new List<int>();
                                                for (var y = 0; y < currentTicketTypes.Count; y++)
                                                {
                                                    var ticketTypeId = currentTicketTypes[y].TicketTypeID;
                                                    var ticketPriceID = currentTicketTypes[y].PriceID;
                                                    if (ticketTypes.IndexOf(ticketTypeId) == -1 || priceIds.IndexOf(ticketPriceID) == -1)
                                                    {
                                                        var list = hold.TicketTypes.ToList();
                                                        list.Remove(hold.TicketTypes.FirstOrDefault(t => t.TicketTypeID == ticketTypeId));
                                                        hold.TicketTypes = list.ToArray();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                        }
                    }

                    if (map.CustomPrices.Prices != null && map.CustomPrices.Prices.Length > 0)
                    {

                        for (var i = 0; i < map.CustomPrices.Prices.Length; i++)
                        {
                            //if (finalPriceByPriceId.ElementAt(i).Value>0)//  if (FinalPriceByPriceId[map.CustomPrices.Prices[i].PriceID])
                                if (finalPriceByPriceId.ContainsKey(map.CustomPrices.Prices[i].PriceID))
                                    map.CustomPrices.Prices[i].EndPrice = finalPriceByPriceId[map.CustomPrices.Prices[i].PriceID];

                        }
                    }
                }
                result = JsonConvert.SerializeObject(map);//JSON.stringify(map);
            }
            

            //getTokenForMap(mapJson);



            return result;
        }


        private static void PrapareMapJasonArray(List<EventCatalogResponseDTO> responseData, ref Dictionary<int, int> finalPriceByPriceLevelID,ref Dictionary<int, int> finalPriceByPriceId, ref List<int> ticketTypes, ref List<int> priceIds)
        {
            //var ticketTypes = new List<int>();
            // var FinalPriceByPriceLevelID = new Dictionary<int, decimal>();
            //var FinalPriceByPriceId = new Dictionary<int, decimal>();
            ticketTypes = responseData.Select(x => x.TicketTypeId.GetValueOrDefault()).Where(x => x > 0).Distinct().ToList();
            priceIds = responseData.Select(x => x.EventimPriceId.GetValueOrDefault()).Where(x => x > 0).Distinct().ToList();
            for (var i = 0;  i < responseData.Count; i++)
            {
                var data = responseData[i];
                /*
                if (!ticketTypes.Contains(data.TicketTypeId.GetValueOrDefault()))
                {
                    ticketTypes.Add(data.TicketTypeId.GetValueOrDefault());
                }
                */
                //PriceTicketTypeIdFullBarcodeMap[data.PriceLevelId + "_" + data.TicketTypeId] = data.FullBarCode;
                //PriceLevelNameTicketTypeIdMap[data.PriceLevelId + "_" + data.TicketTypeId] = data.EventimPriceLevelName;
                //CoinsTicketTypeIdMap[data.PriceLevelId + "_" + data.TicketTypeId] = data.Coins;
                if(!finalPriceByPriceId.TryAdd(data.EventimPriceId.GetValueOrDefault(), (int)data.FinalPrice))
                {
                    var FinalPrice = (int)data.FinalPrice;
                    var exist = finalPriceByPriceId[data.EventimPriceId.GetValueOrDefault()];

                }
                finalPriceByPriceLevelID.TryAdd(data.PriceLevelId.GetValueOrDefault(), (int)data.FinalPrice);
                //FinalPriceTicketTypeIdMap[data.PriceLevelId + "_" + data.TicketTypeId] = data.FinalPrice;
            }
        }

        public static bool IsEventimBenefitByCategoryNumber(IClubRepo _clubRepo, int categoryNumber)
        {
            return _clubRepo.IsEventimBenefitByCategoryNumber(categoryNumber); 
        }

        public static bool IsEventimBenefitByBarcode(IClubRepo _clubRepo, string barcode)
        {
            return _clubRepo.IsEventimBenefitByBarcode(barcode);
        }

        public static void AddHistoryEventData(ITicketsHubRepo _ticketsHubRepo, IClubRepo _clubRepo, IConfigurationManager _configuration, ref HistoryVariant variant)
        {
            try
            {
                var baseUrl = _configuration.GetConfigByValue<string>(ConfigurationKey.EventTicketView);
                var url = string.Format($"{baseUrl}?CategoryNumber=");
                variant.EventsTicketLink = url;

                // variant.OrderConfirmation = UniqueOrderIdentity
                int ticketHubTicketId = _clubRepo.GetAttractionsOrderByUniqueOrderIdentity(long.Parse(variant.OrderConfirmation)).TicketHubTicketId.GetValueOrDefault();
                var eventGuid = string.Empty;
                var externalSystemOrderId = 0;
                OrderTickets eventTicket = _ticketsHubRepo.GetOrderTickets(new List<int> { ticketHubTicketId }).FirstOrDefault();
                if (eventTicket != null)
                {
                    Orders order = _ticketsHubRepo.GetOrders(eventTicket.OrderId);
                    if (order != null)
                    {
                        eventGuid = order.OrderGuiud.ToString();
                        externalSystemOrderId = order.ExternalSystemOrderId.GetValueOrDefault();
                    }
                }
                if (eventTicket != null)
                {
                    variant.EventRow = eventTicket.Row;
                    variant.EventSeat = eventTicket.Seat;
                    variant.EventDate = eventTicket.EventDateTime.GetValueOrDefault().ToString("dd-MM-yyyy");
                    variant.EventTime = string.Format("{0:D2}:{1:D2}", eventTicket.EventDateTime.GetValueOrDefault().Hour, eventTicket.EventDateTime.GetValueOrDefault().Minute);//string.Format("{0:D2}:{1:D2}", eventTicket.EventDateTime, eventTicket.EventDateTime);
                    variant.VenueName = eventTicket.VenueName;
                    variant.PriceLevelName = eventTicket.PriceLlevelName;
                    variant.EventTicketType = eventTicket.TicketTypeId;
                    variant.TicketTypeName = eventTicket.TicketTypeName;
                    variant.EventsTicketLink = url + eventGuid;
                    variant.OrderTicketId = eventTicket.OrderTicketId;
                    variant.TicketArea = eventTicket.Area;
                    variant.EventsGuid = eventGuid;
                    variant.ExternalSystemOrderId = externalSystemOrderId;
                }
                else
                {
                    LoggerHelper.Error($"Warning in EventsFunctions.AddHistoryEventData, Can't find EventTicket. OrderGuid: {variant.OrderGuid}, FullBarcode:{variant.VariantBarCode}");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, $"Error in AddHistoryEventData, error message: {ex.Message}");
            }
        }

        public static bool ValidateEventGuidReservationStatus(ITicketsHubRepo _ticketsHubRepo, string eventGuid)
        {
            var orderTicketHub = _ticketsHubRepo.GetOrdersByEventGuid(eventGuid);
            return !(orderTicketHub == null || orderTicketHub.OrderStatusId != 1);
        }

    }
    public static class ListExtension
    {
        public static List<T> Splice<T>(this List<T> source, int start, int size)
        {
            var items = source.Skip(start).Take(size).ToList<T>();
            if (source.Count >= size)
                source.RemoveRange(start, size);
            else
                source.Clear();
            return items;
        }
    }
}
