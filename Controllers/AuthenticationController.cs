using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CityInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public class AuthenticationRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        private class CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
        {
            public int UserId { get; set; } = userId;
            public string UserName { get; set; } = userName;
            public string FirstName { get; set; } = firstName;
            public string LastName { get; set; } = lastName;
            public string City { get; set; } = city;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            var user=ValidateUserCredentials(authenticationRequestBody.Username, authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
            {
                new("sub", user.UserId.ToString()),
                new("given_name", user.FirstName.ToString()),
                new("family_name", user.LastName.ToString()),
                new("city", user.City.ToString())
            };

            var jwtSecurityToken = new JwtSecurityToken(_configuration["Authentication:Issuer"], _configuration["Authentication:Audience"], claimsForToken, DateTime.Now, DateTime.Now.AddHours(1), signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);

        }

        private CityInfoUser ValidateUserCredentials(string? username, string? password)
        {
            //Dummy data
            return new CityInfoUser(1, username ?? "", "Joe", "Bloggs", "Antwerp");
        }
    }
}
