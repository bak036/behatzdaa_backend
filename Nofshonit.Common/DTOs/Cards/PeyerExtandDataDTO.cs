namespace Nofshonit.Common.DTOs.Cards
{
    public class ExtandPeyerDataDTO
    {
        public PayerDataDTO PayerData { get; set; }
        public string CardNumber { set; get; }
        public bool UseQuickLoad { set; get; }
        public string MaxClubId { set; get; }
       

        public override string ToString()
        {
            return $"&CardNumber={CardNumber}&UseQuickLoad={UseQuickLoad}{PayerData.ToString()}";
        }
    }

   
}
