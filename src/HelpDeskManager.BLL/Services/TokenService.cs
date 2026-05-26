using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Services;
using HelpDeskManager.Core.Settings;
using HelpDeskManager.DAL.Mappers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelpDeskManager.BLL.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(IOptions<JwtSettings> jwtOptions, IUnitOfWork unitOfWork)
    {
        _jwtSettings = jwtOptions.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> CreateToken(AppUserDto userDto)
    {

        if (_jwtSettings.SecretKey.Length < 64) throw new Exception("Your token key needs to be longer.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userDto.Id),
        };

        var roles = await _unitOfWork.UserRepository.GetAssignedUserRolesAsync(userDto);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return await Task.FromResult(tokenHandler.WriteToken(token));
    }
}
