using System.IdentityModel.Tokens.Jwt;

namespace R4R_API.Services
{
    public class ConstainServices
    {

        public string getTokenType(HttpRequest Request, string type)
        {
            try
            {
                var re = Request;
                var headers = re.Headers;
                string tokenString = headers.Authorization;

                var jwtEncodedString = tokenString.Substring(7); // trim 'Bearer ' from the start since its just a prefix for the token string

                var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);

                var value = token.Claims.First(c => c.Type == type).Value;

                return value;
            }
            catch (Exception ex) {
                return "";
              }
        }
    }
}
