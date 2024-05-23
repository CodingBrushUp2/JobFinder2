using JobFinder.Domain.Models.Contracts;
using JobFinder.Domain.Models.DTOs;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text;

namespace JobFinder.Api.Services;

public class AuthService : IAuthService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _tokenUrl;
    private readonly RestClient _client;
    private string _accessToken;
    private DateTime _tokenExpiration;
    private readonly object _lock = new();

    public AuthService(IOptions<AuthOptions> options)
    {
        var authOptions = options.Value;
        _clientId = authOptions.ClientId;
        _clientSecret = authOptions.ClientSecret;
        _tokenUrl = authOptions.TokenUrl;
        _client = new RestClient();
        _tokenExpiration = DateTime.MinValue;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        if (DateTime.UtcNow >= _tokenExpiration)
        {
            lock (_lock)
            {
                if (DateTime.UtcNow >= _tokenExpiration)
                {
                    RefreshTokenAsync().Wait();
                }
            }
        }
        return _accessToken;
    }

    private async Task RefreshTokenAsync()
    {
        var request = new RestRequest(_tokenUrl, Method.Post);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}"))}");
        request.AddParameter("grant_type", "client_credentials");

        var response = await _client.ExecuteAsync<TokenResponse>(request);
        if (!response.IsSuccessful)
        {
            throw new Exception("Failed to retrieve access token");
        }

        _accessToken = response.Data.AccessToken;
        _tokenExpiration = DateTime.UtcNow.AddSeconds(response.Data.ExpiresIn - 60); // Refresh token 60 seconds before it actually expires
    }
}
