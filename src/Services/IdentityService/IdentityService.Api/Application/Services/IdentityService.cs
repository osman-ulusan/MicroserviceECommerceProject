using IdentityService.Api.Application.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Api.Application.Services
{
    public class IdentityService : IIdentityService
    {
        public Task<LoginResponseModel> Login(LoginRequestModel requestModel)
        {
            //veritabanı işlemleri

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, requestModel.UserName),
                new Claim(ClaimTypes.Name,"deneme deneme")//ekranda web tarafında göstermek için eklenebilir vt nın gelen veri koyulur
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MicroserviceETicaretProjesiKeyPassword"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            LoginResponseModel response = new()
            {
                UserName = requestModel.UserName,
                UserToken = encodedJwt
            };

            return Task.FromResult(response);
        }
    }
}
