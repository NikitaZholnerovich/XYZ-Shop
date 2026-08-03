namespace XYZ_shop.Web.Models.Api
{
    public abstract class BaseApiResponse
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
    }
}
