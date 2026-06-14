using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
    public interface ICardsService
    {
        Task<CardDTO> GetCardGeneralInfo();
        Task<string> LoadWallet(PayerDataDTO payerData);
        bool CancelLoadingWallet();
        Task<CardActivitiesDTO> GetCardActivities();
        Task<string> BlockCard();
        bool ActiveateCard();
        Task<WalletBusinessesDTO> GetWalletBusinessesByChain(string walletId, string chainId);
        Task<List<WalletTagChainsData>> GetWalletChain(string walletId);
        Task<List<WalletChainBranches>> GetWalletChainBranches(string walletId, string chainId);
        bool GetNewCardOrderPossible();
        Task<WalletDTO> GetWalletInfo(string walletId);
        Task<BarCodeDTO> GeneratePayCode();
        Task MoveBalance();
        Task<decimal> GetAvailableBalanceForDischarge(int walletId);
        Task Discharge(int walletId, string phoneNumber, string email, string creditCardNumber, string ExpiredDate, int cvv, decimal amountToDischarge);
        Task<decimal> GetGlobalSelfDischargeLimit();
        Task<bool> AllowedToDischarge(string walletId);
		Task<CardDetailsDTO> GetCardDetailsByCardNumber(string cardNumber);
        Task<List<WalletTagChainsData>> GetWalletChainNearMe(string walletId, decimal lat, decimal lon);
        Task<List<WalletChainBranches>> GetWalletChainBranchesNearMe(string walletId, string chainId, decimal lat, decimal lon);
    }
}
