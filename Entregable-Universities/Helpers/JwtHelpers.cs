using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Entregable_Universities.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;

namespace Entregable_Universities.Helpers
{
    public static class JwtHelpers
    {
        public static IEnumerable<Claim> GetClaims(this UserTokensModel userAccount, Guid Id)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Id", userAccount.Id.ToString()),
                new Claim(ClaimTypes.Name, userAccount.userName),
                new Claim(ClaimTypes.Email, userAccount.EmailId),
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };
            if (userAccount.userName == "Admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            }
            else if (userAccount.userName == "User 1")
            {
                claims.Add(new Claim(ClaimTypes.Role, "user"));
                claims.Add(new Claim("userOnly", "user 1"));
            }
            return claims;
        }
        public static IEnumerable<Claim> GetClaims(this UserTokensModel userAccount, out Guid Id)
        {
            Id = Guid.NewGuid();
            return GetClaims(userAccount, Id);
        }
        public static UserTokensModel GenTokenKey(UserTokensModel model, JwtSettingsModel jwtSettings)
        {
            try
            {
                var userToken = new UserTokensModel();
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigninKey);
                Guid Id;
                DateTime expireTime = DateTime.UtcNow.AddDays(1);
                userToken.Validity = expireTime.TimeOfDay;
                var jwToken = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer,
                    audience: jwtSettings.ValidAudience,
                    claims: GetClaims(model, out Id),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(expireTime).DateTime,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256));
                userToken.Token = new JwtSecurityTokenHandler().WriteToken(jwToken);
                userToken.userName = model.userName;
                userToken.Id = model.Id;
                userToken.GuidId = Id;
                return userToken;

            }
            catch (Exception ex)
            {
                throw new Exception("Error Generating the JWT", ex);
            }
        }
    }
}
