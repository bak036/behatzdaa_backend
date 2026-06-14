using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Cards
{
    public class WalletChainDTO
    {

        public List<WalletChainData> WalletChainDataList { get; set; }

        public static implicit operator Task<object>(WalletChainDTO v)
        {
            throw new NotImplementedException();
        }
    }

    public class WalletChainsDTO
    {
        public WalletChainsDTO()
        {
            items = new Dictionary<TagData, List<WalletChainData>>();
        }
        public Dictionary<TagData, List<WalletChainData>> items { get; set; }
    }
    public class WalletChain
    {
        public WalletChainProperties properties { get; set; }
        public WalletChainDataRow[] rows { get; set; }
    }

    public class WalletChainProperties
    {
        public WalletChainData[] Data { get; set; }
    }

    public class WalletTagChainsData
    {
        public string TagName { get; set; }
        public string TagDescription { get; set; }
        public int TagSort { get; set; }
        public int TagType { get; set; }
        public int TagId { get; set; }
        public string TagImg { get; set; }
        public string SubBranchRegions { get; set; }
        public string SubBranchCities { get; set; }
        public List<WalletChainData> WalletChainData { get; set; }
    }

    public class WalletChainBranches
    {
        public int WalletID { get; set; }
        public string WalletName { get; set; }
        public int ChainID { get; set; }
        public string ChainName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string BuisnessID { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone1 { get; set; }
        public decimal? DistanceKm { get; set; }
        public string WebSite { get; set; }

    }

    public class TagData
    {
        public string TagName { get; set; }
        public string TagDescription { get; set; }
        public int TagSort { get; set; }
        public int TagType { get; set; }
        public int TagId { get; set; }

        public override int GetHashCode()
        {
            if (TagName == null) return 0;
            return TagName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var k = obj as TagData;
            if (k != null)
            {
                return this.TagName == k.TagName;
            }
            return base.Equals(obj);
        }
    }

    public class WalletChainData
    {
        public string ErrorID { get; set; }
        public string ChainID { get; set; }
        public string ChainName { get; set; }
        public string LogoURL { get; set; }
        public string SearchKeyWords { get; set; }
        public bool IsWebOnline { get; set; }
        public string WebSite { get; set; }
        public string SubBranchRegions { get; set; }
        public string SubBranchCities { get; set; }

    }

    public class WalletChainDataRow
    {
        public string ErrorID { get; set; }
    }
    public class SubBranchCityRegionData
    {
        public int BusinessId { get; set; }
        public string SubBranchCities { get; set; }
        public string SubBranchRegions { get; set; }
    }

}
