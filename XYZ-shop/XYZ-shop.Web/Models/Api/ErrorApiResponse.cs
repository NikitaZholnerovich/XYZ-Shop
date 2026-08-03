namespace XYZ_shop.Web.Models.Api
{
    public class ErrorApiResponse : BaseApiResponse
    {
        public ErrorApiResponse(string error)
        {
            IsSuccess = false;
            Error = error;
        }
    }
}
