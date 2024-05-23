namespace JobFinder.Api.Models.Contracts
{
    public interface IAuthService
    {
        Task<string> GetAccessTokenAsync();
    }
}