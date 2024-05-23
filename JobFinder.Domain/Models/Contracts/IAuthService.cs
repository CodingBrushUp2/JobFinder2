namespace JobFinder.Domain.Models.Contracts;

public interface IAuthService
{
    Task<string> GetAccessTokenAsync();
}