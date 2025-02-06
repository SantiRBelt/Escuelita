using Entregable_Universities.Helpers;
using Entregable_Universities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Entregable_Universities.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettingsModel _jwtSettings;
        public AccountController(JwtSettingsModel jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }
        private IEnumerable<UserModel> Logins = new List<UserModel>()
        {
            new UserModel()
            {
                Id = 1,
                Email = "martin@correo.com",
                Name = "Admin",
                Password = "Admin"
            },
            new UserModel()
            {
                Id = 1,
                Email = "otro@correo.com",
                Name = "User1",
                Password = "User1"
            }
        };

        [HttpPost]
        public IActionResult GetToken(UserLoginsModel userLogin)
        {
            try
            {
                var token = new UserTokensModel();
                var valid = Logins.Any(user => user.Name.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase));
                if (valid)
                {
                    var user = Logins.FirstOrDefault(user => user.Name.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase));
                    token = JwtHelpers.GenTokenKey(new UserTokensModel()
                    {
                        userName = user.Name,
                        EmailId = user.Email,
                        Id = user.Id.ToString(),
                        GuidId = Guid.NewGuid(),
                    }, _jwtSettings);
                }
                else
                {
                    return BadRequest("Wrong Password");
                }
                return Ok(token);
            }
            catch (Exception ex)
            {
                throw new Exception("GetToken Error", ex);
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public IActionResult GetUserList()
        {
            return Ok(Logins);
        }
    }
}
