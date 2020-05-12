using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace MojGrad.Services
{
    public class MojToken
    {
        private static String secret = "14b9b10b5912ab73891125368b68c2140ba28a2f";
        private static SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        private static TokenValidationParameters parametri = new TokenValidationParameters
        {
            IssuerSigningKey = secretKey, //Key used for token generation
            ValidIssuer = "",
            ValidAudience = "",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
        };


        public string dajToken(long id)
        {
            var hendler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                }),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = hendler.CreateToken(tokenDescriptor);
            return hendler.WriteToken(token);
           
        }

        public string dajRecToken(long id)
        {
            var hendler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                }),
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = hendler.CreateToken(tokenDescriptor);
            return hendler.WriteToken(token);
        }

        public long verifikujToken(string token)
        {
            var hendler = new JwtSecurityTokenHandler();
            try
            {
                var t = hendler.ValidateToken(token, parametri, out SecurityToken validatedToken);
                return Convert.ToInt64(t.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }
            catch
            {
                return -1;
            }
        }

    }
}
