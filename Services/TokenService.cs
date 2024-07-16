using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access Token Key from appsettings!");
        if(tokenKey.Length < 64) throw new Exception("Your token key needs to be longer!");
        // Two types of keys: SymmetricSecurityKeys and AsymmetricSecurityKeys. 
        // In SSK we use single key for encryption & decryption where as in ASSK we use 2 keys, one for encryption and other for decryption
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        // Token contains claims about the user. I claim my dob is this, etc
        var claims = new List<Claim>{
            new(ClaimTypes.NameIdentifier, user.UserName)
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
        
    }
}
