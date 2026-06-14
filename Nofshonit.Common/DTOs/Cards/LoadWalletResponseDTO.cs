namespace Nofshonit.Common.DTOs.Cards
{


    public class LoadWalletResponseDTO
    {
        public LoadWalletRow[] rows { get; set; }
    }

    public class LoadWalletRow
    {
        public string ErrorID { get; set; }
		public long RequestID { get; set; }
	}



}
