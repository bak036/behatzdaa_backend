namespace Nofshonit.Common.DTOs.Cards
{


    public class ErrorDTO
    {
        public Error[] rows { get; set; }
    }

    public class Error
    {
        public string Result { get; set; }
        public string ErrorID { get; set; }
    }


}
