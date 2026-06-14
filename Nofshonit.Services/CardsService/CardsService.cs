using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Services.CardsService
{
    public class CardsService : BaseService, ICardsService
    {
        private ICardsBL _cardsBL;
        public CardsService()
        {
            _cardsBL = Container.Resolve<ICardsBL>();
        }

        public Task<string> BlockCard()
        {
            return _cardsBL.BlockCard();
        }

        public bool CancelLoadingWallet()
        {
            return _cardsBL.CancelLoadingWallet();
        }

        public Task<CardActivitiesDTO> GetCardActivities()
        {
            return _cardsBL.GetCardActivities();
        }

       
        public Task<CardDTO> GetCardGeneralInfo()
        {
            return _cardsBL.GetCardGeneralInfo();
        }

        public Task<WalletBusinessesDTO> GetWalletBusinessesByChain(string walletId, string chainId)
        {
            return _cardsBL.GetWalletBusinessesByChain(walletId, chainId);
        }


        public Task<List<WalletTagChainsData>> GetWalletChain(string walletId)
        {
            return _cardsBL.GetWalletChain(walletId);
        }

        public Task<List<WalletChainBranches>> GetWalletChainBranches(string walletId, string chainId)
        {
            return _cardsBL.GetWalletChainBranches(walletId, chainId);
        }

        public Task<WalletDTO> GetWalletInfo(string walletId)
        {
            return _cardsBL.GetWalletInfo(walletId);
        }

        public Task<string> LoadWallet(PayerDataDTO payerData)
        {
            return _cardsBL.LoadWallet(payerData);
        }

        public bool GetNewCardOrderPossible()
        {
            return _cardsBL.GetNewCardOrderPossible();
        }

        public bool ActiveateCard()
        {
            return _cardsBL.ActiveateCard();
        }

        public Task<BarCodeDTO> GeneratePayCode()
        {
            return _cardsBL.GeneratePayCodeAsync();
        }

        public Task MoveBalance()
        {
            return _cardsBL.MoveBalanceAsync(true);
        }

        public Task<decimal> GetAvailableBalanceForDischarge(int walletId)
        {
            return _cardsBL.GetAvailableBalanceForDischarge(walletId);
        }

        public Task<decimal> GetGlobalSelfDischargeLimit()
{
    return _cardsBL.GetGlobalSelfDischargeLimit();
}

        public Task Discharge(int walletId, string phoneNumber, string email, string creditCardNumber, string ExpiredDate, int cvv, decimal amountToDischarge)
        {
            return _cardsBL.Discharge(walletId, phoneNumber, email, creditCardNumber, ExpiredDate, cvv, amountToDischarge);
        }
        public Task<bool> AllowedToDischarge(string walletId)
        {
            return _cardsBL.AllowedToDischarge(walletId);
        }
		public Task<CardDetailsDTO> GetCardDetailsByCardNumber(string cardNumber)
		{
			return _cardsBL.GetCardDetailsByCardNumber(cardNumber);
		}
        public Task<List<WalletTagChainsData>> GetWalletChainNearMe(string walletId, decimal lat, decimal lon)
        {
            return _cardsBL.GetWalletChainNearMe(walletId, lat, lon);
        }

        public Task<List<WalletChainBranches>> GetWalletChainBranchesNearMe(string walletId, string chainId, decimal lat, decimal lon)
        {
            return _cardsBL.GetWalletChainBranchesNearMe(walletId, chainId, lat, lon);
        }
    }
}
