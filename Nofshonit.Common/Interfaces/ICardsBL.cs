using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.EF.DTS_Online;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
    public interface ICardsBL
    {
        Task<CardDTO> GetCardGeneralInfo();
        Task<string> LoadWallet(PayerDataDTO payerData);
        bool CancelLoadingWallet();
        Task<CardActivitiesDTO> GetCardActivities();
        Task<string> BlockCard();
        Task<WalletBusinessesDTO> GetWalletBusinessesByChain(string walletId, string chainId);
        ECheckCreditCard CheckMaxClubCreditCard(ResponseUserDTO currentUser, string PayerCardNumber, string PinCode, bool SaveForQuickLoad, out string ClubKey);
        Task<List<WalletTagChainsData>> GetWalletChain(string walletId);
        Task<List<WalletChainBranches>> GetWalletChainBranches(string walletId, string chainId);
        bool GetNewCardOrderPossible();
        Task<WalletDTO> GetWalletInfo(string walletId);
        bool ActiveateCard();
        Task<BarCodeDTO> GeneratePayCodeAsync();

        Task MoveBalanceAsync(bool? isVirtual);
        Task<decimal> GetAvailableBalanceForDischarge(int walletId);
        Task<decimal> GetGlobalSelfDischargeLimit();
        Task Discharge(int walletId, string phoneNumber, string email, string creditCardNumber, string ExpiredDate, int cvv, decimal amountToDischarge);
        Task<bool> AllowedToDischarge(string walletId);
		Task<CardDetailsDTO> GetCardDetailsByCardNumber(string cardNumber);
        Task<List<WalletTagChainsData>> GetWalletChainNearMe(string walletId, decimal lat, decimal lon);
        Task<List<WalletChainBranches>> GetWalletChainBranchesNearMe(string walletId, string chainId, decimal lat, decimal lon);
    }
}
