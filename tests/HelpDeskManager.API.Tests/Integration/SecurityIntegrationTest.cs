using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HelpDeskManager.API.Tests.Integration;

public class SecurityIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public SecurityIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AccessEndpoint_WithoutToken_ShouldTriggerOnChallengeAndReturn401()
    {
        // Act
        var response = await _client.GetAsync("/api/supportrequests");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("MISSING_TOKEN", responseString);
        Assert.Contains("The access token is required and was not provided or the format is incorrect.", responseString);
    }

    [Fact]
    public async Task AccessAdminEndpoint_WithAgentToken_ShouldTriggerOnForbiddenAndReturn403()
    {
        // Arrange
        var agentToken = GenerateTestJwtToken("Agent");

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", agentToken);


        // Act
        var response = await _client.GetAsync("/api/users/admin-dashboard");


        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("FORBIDDEN", responseString);
        Assert.Contains("You do not have permission to access this resource.", responseString);
    }

    private string GenerateTestJwtToken(string role)
    {
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();

        var secretKey = configuration["JwtSettings:SecretKey"];
        var issuer = configuration["JwtSettings:Issuer"];
        var audience = configuration["JwtSettings:Audience"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, role)
    };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
