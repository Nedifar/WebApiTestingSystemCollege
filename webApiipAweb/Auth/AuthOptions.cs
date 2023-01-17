using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace webApiipAweb.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";

        public const string AUDIENCE = "MyAuthClient";

        const string KEY = "mysupersecret_secret!123";

        public const int LIFETIME = 15;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
