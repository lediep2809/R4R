using R4R_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace R4R_API.Services
{
    public class UserService 
    {
        private readonly R4rContext _Db;

        public UserService(R4rContext Db)
        {
            _Db = Db;
        }
        public string getTokenValue(HttpRequest Request, string value)
        {
            try
            {
                var re = Request;
                var headers = re.Headers;
                string tokenString = headers.Authorization;
                var jwtEncodedString = tokenString.Substring(7); // trim 'Bearer ' from the start since its just a prefix for the token string
                var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
                var val = token.Claims.First(c => c.Type == value).Value;

                return val;

            }
            catch
            {

                return null;
            }

        }
    }
}
