using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.Services.Utils.Authentication
{
    public class JwtProvider(IConfiguration configuration)
    {
        public string Generate(User user)
        {
            // a safe way to get JwtSecretKey is to add a secret to secret.json in api project by right click -> Manage User Secret
            // you can create the secret key as a new Guid and then encode it as a base 64 string
            string secretKey = configuration["Jwt:Secret"]!;

            // create a security using a SymmetricSecurityKey that will get byte[] from the secretKey
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // create a SigningCredentials with the securityKey and SecurityAlgorithms.HmacSha256
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name,user.Name),
                    new Claim("username",user.Username),
                ]),
                // if somehow the configuration.GetValue() doesn't work, manage NuGet package and install Microsoft.Extensions.Configuration.Binder
                Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:ExpirationInDays")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };
            // using JsonWebTokenHandler is recommended than JwtSecurityTokenHandler because it's faster about 30%
            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
