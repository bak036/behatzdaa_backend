using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nofshonit.Common.Interfaces;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.BL.Utils;
using System.Linq;
using Nofshonit.Repositories.DtsOnlineModel;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.BL.Limitations;
using Microsoft.Extensions.Logging;
using Google.Apis.ServiceUser.v1.Data;
using Nofshonit.Common.DTOs.Limitations;

namespace Nofshonit.BL.ShopingBasketFolder
{

    public class ShopingBasketBL : BaseBL, IShopingBasketBL
    {
        private IClubRepo _clubRepo;
        private IMapperManager _mapper;
        private ICategoryBL _categoryBL;
        private ICategoryService _categoryService;
        private IConfigurationManager _configuration;
        private IDtsOnlineRepo _dtsOnlineRepo;
        private int organizationId;
        private List<int> messageKeys;

        public ShopingBasketBL() : base()
        {
            _clubRepo = Container.Resolve<IClubRepo>();
            _mapper = Container.Resolve<IMapperManager>();
            _categoryBL = Container.Resolve<ICategoryBL>();
            _categoryService = Container.Resolve<ICategoryService>();
            _configuration = Container.Resolve<IConfigurationManager>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            organizationId = ContextManager.CurrentOrganization().OrgId;
            messageKeys = new List<int>() { 725, 10002 };
        }

        private void ValidateShpoingProductDetails(ProductDTO productDto)
        {
            // Get current cart items for the user
            string memberId = ContextManager.CurrentUser().Id;
            List<ShopingBasket> currentCartItems = _clubRepo.GetShopingBasketByMemberId(memberId, null);

            // Check 1 (before loop): total request qty + cart qty for non-campaign/non-ignored variants vs subtype configured limits
            var requestVariantsWithLimits = productDto.Variants
                .Select(v => new { Variant = v, ProductVar = _clubRepo.GetProductsVar(v.Barcode) })
                .Where(x => x.ProductVar != null
                    && !x.ProductVar.IsIgnoreBusinessSubTypeLimits
                    && (!x.ProductVar.Iscampaign.HasValue || !x.ProductVar.Iscampaign.Value))
                .ToList();

            if (requestVariantsWithLimits.Count > 0)
            {
                int requestQtyBySubtype = requestVariantsWithLimits.Sum(x => x.Variant.Quantity);
                var first = requestVariantsWithLimits.First();

                int existingCartQtyBySubtype = 0;
                if (first.ProductVar.BusinessSubTypeId.HasValue && currentCartItems != null)
                {
                    existingCartQtyBySubtype = currentCartItems
                        .Where(x => {
                            if (x.ProductSubType != first.ProductVar.BusinessSubTypeId.Value) return false;
                            var cartPv = _clubRepo.GetProductsVar(x.ProductBarcode);
                            return cartPv != null
                                && !cartPv.IsIgnoreBusinessSubTypeLimits
                                && (!cartPv.Iscampaign.HasValue || !cartPv.Iscampaign.Value);
                        })
                        .Sum(x => (int)x.Quantity);
                }

                int totalSubtypeQty = requestQtyBySubtype + existingCartQtyBySubtype;

                List<BusinessSubTypeDTO> subTypeDTOs = _clubRepo.GetBussinessSubType(_dtsOnlineRepo.GetBussinessSubTypeNames());
                var subtypeSpec = subTypeDTOs.FirstOrDefault(s => s.Id == first.ProductVar.BusinessSubTypeId);

                if (subtypeSpec != null)
                {
                    // Get member transactions to account for previous purchases (like GetMemberLimits does)
                    var memberTransacionsList = _clubRepo.GetMemberTransactions(memberId, false, false, true);
                    var memberTransacionsListFiltered = memberTransacionsList
                        .Where(m => (m.IsIgnoreBusinessSubTypeLimits == false || m.IsIgnoreBusinessSubTypeLimits == null)
                            && (m.Iscampaign == null || m.Iscampaign == false))
                        .ToList();
                    var memberTransactionsOfSubType = memberTransacionsListFiltered
                        .Where(r => r.BussinesSubTypeID == subtypeSpec.Id);

                    int? subtypeLimit = null;

                    if (subtypeSpec.LimitWeekly.HasValue && subtypeSpec.LimitWeekly > 0)
                    {
                        DateTime startOfWeek = LimitationsBL.GetStartOfWeek();
                        var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > startOfWeek).Sum(r => r.QuantityInt);
                        var remainingLimit = subtypeSpec.LimitWeekly.Value - totalOrders;
                        subtypeLimit = remainingLimit;
                    }

                    if (subtypeSpec.LimitMontly.HasValue && subtypeSpec.LimitMontly > 0)
                    {
                        DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        var remainingLimit = subtypeSpec.LimitMontly.Value - totalOrders;
                        subtypeLimit = subtypeLimit.HasValue ? Math.Min(subtypeLimit.Value, remainingLimit) : remainingLimit;
                    }

                    if (subtypeSpec.LimitYearly.HasValue && subtypeSpec.LimitYearly > 0)
                    {
                        DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
                        var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        var remainingLimit = subtypeSpec.LimitYearly.Value - totalOrders;
                        subtypeLimit = subtypeLimit.HasValue ? Math.Min(subtypeLimit.Value, remainingLimit) : remainingLimit;
                    }

                    if (subtypeLimit.HasValue && totalSubtypeQty > subtypeLimit.Value)
                    {
                        var message = MessagesUtil.GetMessagesByKey(new List<int> { 10272 }).FirstOrDefault().MessageText;
                        var subTypeName = subtypeSpec.Name;
                        throw new BusinessException(string.Format(message, subTypeName));
                    }
                }
            }

            // Check 2 (per variant): per-variant limit (existing cart qty + new qty)
            foreach (ShoppingBasketVariantDTO variantToAdd in productDto.Variants)
            {
                ProductsVars productVar = _clubRepo.GetProductsVar(variantToAdd.Barcode);
                if (productVar == null)
                    throw new BusinessException("הנתונים שגויים או שאירעה תקלה בהכנסת הפריט לעגלת קניות");


                int quantityInCart = 0;
                if (currentCartItems != null)
                {
                    quantityInCart = currentCartItems
                        .Where(x => x.ProductBarcode == variantToAdd.Barcode)
                        .Sum(x => (int)x.Quantity);
                }

                // Recalculate OrderLimit with cart quantity taken into account
                int cartQtyBySubtype = 0;
                if (productVar.BusinessSubTypeId.HasValue && currentCartItems != null)
                {
                    cartQtyBySubtype = currentCartItems
                        .Where(x => {
                            if (x.ProductSubType != productVar.BusinessSubTypeId.Value) return false;
                            var cartPv = _clubRepo.GetProductsVar(x.ProductBarcode);
                            return cartPv != null
                                && !cartPv.IsIgnoreBusinessSubTypeLimits
                                && (!cartPv.Iscampaign.HasValue || !cartPv.Iscampaign.Value);
                        })
                        .Sum(x => (int)x.Quantity);
                }

                int actualOrderLimit = Container.Resolve<ILimitationsBL>().GetVariantOrderLimit(
                    variantToAdd.Barcode,
                    cartQtyBySubtype,
                    true,
                    false
                );

                int totalQuantity = quantityInCart + variantToAdd.Quantity;

                if (actualOrderLimit >= 0 && actualOrderLimit < totalQuantity)
                {
                    var message = MessagesUtil.GetMessagesByKey(new List<int> { 51134 }).FirstOrDefault(m => m.MessageKey == 51134)?.MessageText;

                    if (!string.IsNullOrEmpty(message))
                    {
                        int configuredMaxLimit = GetConfiguredMaxLimit(productVar);
                        throw new BusinessException(message.Replace("X", configuredMaxLimit.ToString()));
                    }
                    else
                    {
                        throw new BusinessException("הנתונים שגויים או שאירעה תקלה בהכנסת הפריט לעגלת קניות");
                    }
                }

            }
        }


        /// <summary>  
        ///  The method adds variants of the same category or 
        ///  tickets of an event, to the shoppingbasket table  
        /// </summary>  
        /// <param name="productDto">  
        ///  The Dto with the information on the category and the list of variants to  
        ///  add to the list, or the tickets information of an event 
        ///	</param>  
        /// <returns> 
        ///  Returns a list of products(CartVarsDTO), that each product contains which   
        ///  category it belongs and more details about the variant, or details about  
        ///  the ticket 
        /// </returns>  
        public List<CartVarsDTO> AddProduct(ProductDTO productDto)
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            if (currentUser.ClubCreditCard == 0)
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10002).MessageText);

            bool result = true;
            bool isEventsInBasket = false;
            var listOfProducts = _clubRepo.GetAllProducts();
            List<long> listOfExpiredProducts = new List<long>();
            List<ShopingBasket> listOfUserProducts = new List<ShopingBasket>();

			// Removal all expired products from the shopping basket that the job has not yet removed
			foreach (var product in listOfProducts)
            {
                if(product.Expired > DateTime.Now)
                    listOfUserProducts.Add(product);
                else
                    listOfExpiredProducts.Add(product.Id);
            }
            if(listOfExpiredProducts.Count  > 0) {
                _clubRepo.SyncShoppingBasket(ContextManager.CurrentUser().Id.ToString(), listOfExpiredProducts);
            }
			var categories = listOfUserProducts.Select(x => long.Parse(x.CategoryNumber)).ToList();
                        
            isEventsInBasket = _dtsOnlineRepo.IsEvents(categories, organizationId);
            //ValidateLimitOnUpdate(productDto);

            Container.Resolve<ILimitationsBL>().ValidateCategoryForMemberLimitations(long.Parse(productDto.CategoryNumber));
            var isEvent = _dtsOnlineRepo.IsEvents(long.Parse(productDto.CategoryNumber), organizationId);
            if (!isEvent)
            {
                ValidateShpoingProductDetails(productDto);
            }

            // checks if its a normal product or tickets of an event 
            if (!isEvent)
            {
                foreach (var variant in productDto.Variants)
                {
                    if (result)
                    {
                        var productFromDb = _clubRepo.GetProductByBarcode(variant.Barcode);
                        var minutesToAdd = _configuration.GetConfigByValue<int>(ConfigurationKey.ShoppingBasket_ExpireDate);
                        var stock =   _dtsOnlineRepo.CheckCartVariantBarCodeDtsStock(variant.Barcode);
                        if (!stock)
                        throw new BusinessException(MessagesUtil.GetMessagesByKey(new List<int> { 10013 }).FirstOrDefault().MessageText);

                        if (productFromDb == null)
                        {
                            ShopingBasket productToAdd = new ShopingBasket();
                            
                            productToAdd.CategoryNumber = productDto.CategoryNumber;
                            productToAdd.CreateDate = DateTime.Now;
                            productToAdd.Expired = productToAdd.CreateDate.Value.Add(new TimeSpan(0, minutesToAdd, 0));
                            productToAdd.ProductSubType = variant.BusinessSubTypeId;
                            productToAdd.MemberId = currentUser.Id;
                            productToAdd.Quantity = variant.Quantity;
                            productToAdd.FinalPrice = variant.Price * variant.Quantity;
                            productToAdd.ProductBarcode = variant.Barcode;
                            productToAdd.ProductJsonForGA = variant.ProductJsonForGA;
                            result &= _clubRepo.AddProduct(productToAdd);
                        }
                        else
                        {
                            productFromDb.ProductSubType = variant.BusinessSubTypeId;
                            productFromDb.Quantity += variant.Quantity;
                            productFromDb.FinalPrice = variant.Price * productFromDb.Quantity;
                            productFromDb.CreateDate = DateTime.Now;
                            productFromDb.Expired = productFromDb.CreateDate.Value.Add(new TimeSpan(0, minutesToAdd, 0));
                            productFromDb.ProductJsonForGA = variant.ProductJsonForGA;
                            result &= _clubRepo.UpdateProduct(productFromDb).Result;
                        }
                    }
                }
            }
            //Events
            else
            {
                if (isEventsInBasket)
                {
                    result = false;
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(new int[] { 10239 }.ToList()).FirstOrDefault(m => m.MessageKey == 10239).MessageText); 
                }
                var response = Container.Resolve<IEventBL>().CreateReservation(productDto.CreateReservation);
                foreach (var ticket in response.OrderTickets)
                {
                    if (result)
                    {
                        var productJsonForGAToAdd = productDto.CreateReservation.ListOfSeats.Where(s => s.VariantFullBarcode == ticket.VariantFullBarcode).FirstOrDefault();

                        ShopingBasket productToAdd = new ShopingBasket();
                        var minutesToAdd = _configuration.GetConfigByValue<int>(ConfigurationKey.ShoppingBasket_EventimExpireDate);

                        productToAdd.CategoryNumber = productDto.CategoryNumber;
                        productToAdd.CreateDate = DateTime.Now;
                        productToAdd.Expired = productToAdd.CreateDate.Value.Add(new TimeSpan(0, minutesToAdd, 0));
                        productToAdd.Quantity = 1;
                        productToAdd.MemberId = currentUser.Id;
                        productToAdd.FinalPrice = ticket.Price;
                        productToAdd.ProductBarcode = ticket.VariantFullBarcode;
                        productToAdd.ProductJsonForGA = productDto.ProductJsonForGA;
                        if (productJsonForGAToAdd != null)
                            productToAdd.ProductJsonForGA = productJsonForGAToAdd.ProductJsonForGA;
                        OrderTicketResponseDTO ticketToXml = new OrderTicketResponseDTO(ticket, response.OrderGuid, response.EventDate, response.EventTime,response.IsSelfPrint);
                        productToAdd.XmlParams = XmlGenerator.ObjectToXml(ticketToXml);
                        result &= _clubRepo.AddProduct(productToAdd);
                    }
                }
            }

            if (result)
            {
                var currCart = GetCart();
                return GetCartVars(currCart);
            }
            return null;
        }

        /// <summary>
        ///  Update one product in the table
        /// </summary>
        /// <param name="productToUpdate">The product to add with the new quantity</param>
        /// <returns>
        ///  Returns a list of products(CartVarsDTO), that each product contains which   
        ///  category it belongs and more details about the variant, or details about  
        ///  the ticket
        /// </returns>
        public List<CartVarsDTO> UpdateProductInCart(ShoppingBasketVariantDTO productToUpdate)
        {
            bool result = false;
            if (productToUpdate.Quantity > 0)
            {
                var productFromDb = _clubRepo.GetProductByBarcode(productToUpdate.Barcode);
                if (productFromDb != null)
                {
                    var minutesToAdd = _configuration.GetConfigByValue<int>(ConfigurationKey.ShoppingBasket_ExpireDate);
                    var oneProductPrice = productFromDb.FinalPrice / productFromDb.Quantity;

                    productFromDb.Quantity = productToUpdate.Quantity;
                    productFromDb.FinalPrice = oneProductPrice * productToUpdate.Quantity;
                    productFromDb.CreateDate = DateTime.Now;
                    productFromDb.Expired = productFromDb.CreateDate.Value.Add(new TimeSpan(0, minutesToAdd, 0));
                    result = _clubRepo.UpdateProduct(productFromDb).Result;
                }
            }
            else
            {
                result = _clubRepo.RemoveProduct(productToUpdate.Barcode);
            }
            //if (result)
            //{
            //    var currCart = GetCart();
            //    return GetCartVars(currCart);
            //}
            //else
            //{
            //    throw new Exception("update didnt work-variant does'nt exist in cart");
            //}
            var currCart = GetCart();
            return GetCartVars(currCart);
        }

        /// <summary>  
        ///  The Method will return a list of the products from the db shoppingbasket table  
        /// </summary>  
        /// <returns> 
        ///  Returns list of the products from the db shoppingbasket table 
        /// </returns>  
        public List<ShopingBasket> GetCart()
        {
            return _clubRepo.GetCart(ContextManager.CurrentUser().Id);
        }





        /// <summary>  
        ///  The method will return a list of products(CartVarsDTO),  
        ///  that each product contains which category it belongs 
        ///  and more details about the variant, or details about  
        ///  the ticket 
        /// </summary>  
        /// <param name="cart">  
        ///  The list of products from the shopping basket table  
        /// </param>  
        /// <returns>  
        ///  Returns a list of products(CartVarsDTO), that each product contains which   
        ///  category it belongs and more details about the variant, or details about  
        ///  the ticket 
        /// </returns>  
        /// 

        public List<CartVarsDTO> GetCartVars(List<ShopingBasket> cart)
        {
            // the list to return for display  
            List<CartVarsDTO> productsList = new List<CartVarsDTO>();

            var eventCards = cart.Where(x => !string.IsNullOrEmpty(x.XmlParams)).ToList();
            var variantCart = cart.Where(x => string.IsNullOrEmpty(x.XmlParams)).ToList();
            var byBusinessSubTypeId = cart.GroupBy(x => x.ProductSubType);
            var businessSubTypeCurrent = _clubRepo.GetAllBusinessSubTypes();
            var cartQtyBySubtype = byBusinessSubTypeId.ToDictionary(x => x.Key, x => x.Sum(y => (int)y.Quantity));
            if (eventCards.Count > 0)
            {
                CartVarsDTO eventItem = new CartVarsDTO();
                ShopingBasket first = eventCards.First();
                eventItem.CategoryId = first.CategoryNumber;
                var categoryDetails = _categoryService.GetCategoryDetails(long.Parse(first.CategoryNumber));

                OrderTicketResponseDTO firstTicket = null;
                try
                {
                    firstTicket = XmlGenerator.XmlToObject<OrderTicketResponseDTO>(first.XmlParams);
                }
                catch (Exception ex) //במקרים שבטעות הוריאנט מזוהה כאיוונטים, מוחק את הוריאנטים השגויים מהסל קניות
                {
                    foreach (var item in eventCards)
                    {
                        _clubRepo.RemoveProduct(item.ProductBarcode);
                    }
                }
                if(firstTicket != null)
                {
                    eventItem.EventDate = firstTicket.EventDate;
                    eventItem.CategoryName = categoryDetails.CategoryName;
                    eventItem.ShortDescription = categoryDetails.ShortDescription;
                    eventItem.SupplierName = categoryDetails.SupplierName;
                    eventItem.ExpireDate = first.Expired;
                    eventItem.Quantity = (byte)eventCards.Count;
                    eventItem.ProductJsonForGA = eventCards.FirstOrDefault().ProductJsonForGA;
                    List<OrderTicketResponseDTO> tickets = new List<OrderTicketResponseDTO>();

                    int finalPrice = 0;
                    foreach (var ticket in eventCards)
                    {
                        var orderTicket = XmlGenerator.XmlToObject<OrderTicketResponseDTO>(ticket.XmlParams);
                        orderTicket.ProductJsonForGA = ticket.ProductJsonForGA;
                        orderTicket.EventimTicketTypeName = GetVariant(orderTicket.VariantFullBarcode).EventimTicketTypeName;
                        orderTicket.Price = (ticket.FinalPrice.HasValue && ticket.FinalPrice.Value > orderTicket.Price) ? ticket.FinalPrice.Value : orderTicket.Price;
                        finalPrice += orderTicket.Price;
                        tickets.Add(orderTicket);
                    }
                    eventItem.Tickets = tickets;
                    eventItem.FinalPrice = finalPrice;
                    // order limit
                    foreach (var ticket in tickets)
                    {
                        ticket.OrderLimit = Container.Resolve<ILimitationsBL>().ValidatePurchesAllowed(eventItem).OrderLimit;
                    }
                    productsList.Add(eventItem);
                }
            }


            // saves all the variants by the product barcode from the member cart 
            List<long>productsToRemove = new List<long>();  
            foreach (var product in variantCart)
            {
                CartVarsDTO add_to_list_product = new CartVarsDTO();
                ProductsVars productVariant = GetVariant(product.ProductBarcode);
                VariantDTO variantDto = new VariantDTO();
                var monthlyLimit = int.Parse(LimitationsBL.GetMemberMonthlyLimit(productVariant, businessSubTypeCurrent));

                var categoryDetails = _categoryService.GetCategoryDetails(long.Parse(product.CategoryNumber));

                if (categoryDetails != null)
                {
                    //category
                    add_to_list_product.SupplierName = categoryDetails.SupplierName;
                    add_to_list_product.FinalPrice = product.FinalPrice;
                    add_to_list_product.CategoryId = product.CategoryNumber;
                    add_to_list_product.Quantity = product.Quantity;
                    add_to_list_product.CategoryName = categoryDetails.CategoryName;
                    add_to_list_product.ShortDescription = categoryDetails.ShortDescription;
                    add_to_list_product.EventDate = productVariant.StartDate;
                    add_to_list_product.ExpireDate = product.Expired;
                    add_to_list_product.ProductJsonForGA = product.ProductJsonForGA;
                    add_to_list_product.AllowPriceZero = productVariant.AllowPriceZero != null ? productVariant.AllowPriceZero : false;

                    //variant
                    variantDto.BarCode = productVariant.FullBarCode;
                    variantDto.Name = productVariant.ShortNameVar;
                    variantDto.IsCampaign = productVariant.Iscampaign;
                    variantDto.ExpireDate = DateCalculation.CalculateExpirationDate(productVariant.TypeCalcImplementationDate ?? 0, productVariant.LastImplementationDate.GetValueOrDefault());
                    variantDto.MonthlyLimit = string.IsNullOrEmpty(monthlyLimit.ToString()) ? -1 : monthlyLimit;
                    variantDto.YearlyLimit = !string.IsNullOrEmpty(productVariant.MemberYearlyLimitFormula) && (productVariant.MemberYearlyLimitFormula.All(char.IsNumber)) ? int.Parse(productVariant.MemberYearlyLimitFormula) : -1;
                    variantDto.GeneralLimit = !string.IsNullOrEmpty(productVariant.MemberGeneralLimitFormula) && (productVariant.MemberGeneralLimitFormula.All(char.IsNumber)) ? int.Parse(productVariant.MemberGeneralLimitFormula) : -1;

                    variantDto.IrgunPrice = decimal.Parse(productVariant.IrgunPriceFormula);
                    add_to_list_product.Variant = variantDto;

                    if (product.Quantity != 0)
                        variantDto.Price = product.FinalPrice / product.Quantity;
                    else
                        variantDto.Price = product.FinalPrice;
                    variantDto.BusinessSubTypeId = product.ProductSubType;

                    productsList.Add(add_to_list_product);
                    variantDto.OrderLimit = Container.Resolve<ILimitationsBL>().ValidatePurchesAllowed(add_to_list_product).OrderLimit;

                    if (productVariant.CuponStockId.HasValue)
                    {
                        using (var dtsContext = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
                        {
                            var res = dtsContext.CouponsStocksDetails.FirstOrDefault(f => f.StockId == productVariant.CuponStockId);
                            variantDto.isExternalCoupon = res != null && res.StockType == 0;
                        }
                    }
                }
                else
                    productsToRemove.Add(product.Id);
            }
            _clubRepo.SyncShoppingBasket(ContextManager.CurrentUser().Id.ToString(),productsToRemove);

            return productsList;
        }

        /// <summary> 
        ///  The method will return a variant from the ProductVars  
        ///  table in the db by its barcode 
        /// </summary> 
        /// <param name="barcode">The variant barcode</param> 
        /// <returns> 
        ///  Returns the variant from the ProductVars table in the db 
        /// </returns> 
        public ProductsVars GetVariant(string barcode)
        {
            return _clubRepo.GetVariant(barcode);
        }

        /// <summary> 
        ///  The method removes a product from the shoppingbasket table in the db, 
        ///  and the returns the list of the user other products from the GetCartVars method 
        /// </summary> 
        /// <param name="productBarcode"> 
        ///  The variant/product barcode that will be removed from the shoppingbasket table 
        ///  </param> 
        /// <returns> 
        ///  Returns the list of the user other products from the GetCartVars method 
        /// </returns> 
        public List<CartVarsDTO> RemoveProduct(string productBarcode)
        {
            if (productBarcode != null)
            {
                _clubRepo.RemoveProduct(productBarcode);
                var currCart = GetCart();
                return GetCartVars(currCart);
            }
            return null;
        }

        /// <summary> 
        ///  The method removes all the products of the user that belongs to the category given 
        ///  and the returns the list of the user other products from the GetCartVars method 
        /// </summary> 
        /// <param name="categoryNumber"> 
        ///  The category of of the products that its products will be removed 
        /// </param> 
        /// <returns> 
        ///  Returns the list of the user other products from the GetCartVars method 
        /// </returns> 
        public List<CartVarsDTO> RemoveProductsByCateoryNumber(string categoryNumber)
        {
            if (categoryNumber != null)
            {
                _clubRepo.RemoveProductsByCategoryNumber(categoryNumber);
                var currCart = GetCart();
                return GetCartVars(currCart);
            }
            return null;
        }

        /// <summary> 
        ///  The method removes all the products of the user 
        /// </summary> 
        /// <returns> 
        ///  Returns true if the removal was successful, false otherwise 
        /// </returns> 
        public bool RemoveAllProducts()
        {
            return _clubRepo.RemoveAllProducts();
        }
        public List<ShopingBasket> UpdateShoppingCartPrices(int creditCardType)
        {
            return _clubRepo.UpdateShoppingCartPricesGetCart(ContextManager.CurrentUser().Id, creditCardType);

        }

        private int GetConfiguredMaxLimit(ProductsVars productVar)
        {
            List<int> configuredLimits = new List<int>();

            if (!string.IsNullOrEmpty(productVar.MemberMonthlyLimitFormula) && int.TryParse(productVar.MemberMonthlyLimitFormula, out int monthlyLimit) && monthlyLimit > 0)
                configuredLimits.Add(monthlyLimit);

            if (!string.IsNullOrEmpty(productVar.MemberQuarterLimitFormula) && int.TryParse(productVar.MemberQuarterLimitFormula, out int quarterLimit) && quarterLimit > 0)
                configuredLimits.Add(quarterLimit);

            if (!string.IsNullOrEmpty(productVar.MemberYearlyLimitFormula) && int.TryParse(productVar.MemberYearlyLimitFormula, out int yearlyLimit) && yearlyLimit > 0)
                configuredLimits.Add(yearlyLimit);

            if (!string.IsNullOrEmpty(productVar.MemberGeneralLimitFormula) && int.TryParse(productVar.MemberGeneralLimitFormula, out int generalLimit) && generalLimit > 0)
                configuredLimits.Add(generalLimit);

            if (productVar.BusinessSubTypeId.HasValue)
            {
                using (var orgDb = ContextManager.ClubContext())
                {
                    var spec = orgDb.BusinessSubTypeSpecificationCurrent
                        .FirstOrDefault(x => x.BusinessSubTypeId == productVar.BusinessSubTypeId.Value);
                    if (spec?.MonthlyLimitValue != null && spec.MonthlyLimitValue.Value > 0)
                        configuredLimits.Add(spec.MonthlyLimitValue.Value);
                    if (spec?.MonthlyLimitVariantValue != null && spec.MonthlyLimitVariantValue.Value > 0)
                        configuredLimits.Add(spec.MonthlyLimitVariantValue.Value);
                }
            }

            return configuredLimits.Count > 0 ? configuredLimits.Min() : 0;
        }
    }
}
