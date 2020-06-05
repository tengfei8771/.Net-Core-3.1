
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UIDP.UTILITY.JWTHelper
{
    public class JWTHelper
    {
        public static IConfiguration Configuration { get; set; }
        public JWTHelper()
        {
            securityKey = GetSecurityKey();
        }

        static string securityKey;
        public static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public string CreateToken(Dictionary<string, object> payLoad=null, int expiresMinute=2, Dictionary<string, object> header = null)
        {
            if (header == null)
            {
                header = new Dictionary<string, object>()
                {
                    {"alg", "HS256" },
                    {"typ", "JWT" }
                };
            }
            if (payLoad == null)
            {
                payLoad = new Dictionary<string, object>();
            }
            var now = DateTime.UtcNow;
            payLoad["nbf"] = ToUnixEpochDate(now);//可用时间起始
            payLoad["exp"] = ToUnixEpochDate(now.Add(TimeSpan.FromMinutes(expiresMinute)));//可用时间结束
            var encodedHeader = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(header));
            var encodedPayload = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(payLoad));
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(securityKey));
            var encodedSignature = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(encodedHeader, ".", encodedPayload))));
            var encodedJwt = string.Concat(encodedHeader, ".", encodedPayload, ".", encodedSignature);
            return encodedJwt;
        }

        public  bool Validate(string encodeJwt)
        {
            bool success = true;           
            try
            {
                string[] JWTarr = encodeJwt.Split('.');
                Dictionary<string, object> header = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(JWTarr[0]));
                Dictionary<string, object> payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(JWTarr[1]));
                var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(securityKey));
                success = success && string.Equals(JWTarr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(JWTarr[0], ".", JWTarr[1])))));
                if (!success)
                {
                    return success;
                }
                var now = ToUnixEpochDate(DateTime.UtcNow);
                success = success && (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));
                
            }
            catch
            {
                return false;
            }
            return success;

        }

        private string GetSecurityKey()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration["SecurityKey"];
        }
    }
}
