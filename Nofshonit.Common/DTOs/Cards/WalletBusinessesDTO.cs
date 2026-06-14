using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.Cards
{
    public class WalletBusinessesDTO
    {
        public List<WalletBusinessesData> WalletBusinessesList { get; set; }
    }

    public class WalletBusinesses
    {
        public Properties properties { get; set; }
        public Row2[] rows { get; set; }
    }

    public class Properties
    {
        public WalletBusinessesData[] Data { get; set; }
    }

    public class WalletBusinessesData
    {
        public string ChainID { get; set; }
        public string ChainName { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
    }

    public class Row2
    {
        public string ErrorID { get; set; }
    }

}
