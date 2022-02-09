using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using testarchApi.Connection;

namespace testarchApi.Security
{
    internal static class TokenMaker
    {
        public static string TJwt(string username, string userId)
        {
            ConexionDb c = new ConexionDb();
            DataSet ds = new DataSet();
            SqlParameter[] sqlParam = new SqlParameter[0];
            string secret = string.Empty;
            string expireT = string.Empty;

            ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.getApiJwtParams(), sqlParam);
            secret = ds.Tables[0].Rows[0].Field<string>("apiSecret");
            expireT = ds.Tables[1].Rows[0].Field<string>("expirationT");

            var audience = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuer = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];

            var secKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secret));
            var signature = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha512Signature);

            ClaimsIdentity ci = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, userId) ,
                new Claim(ClaimTypes.NameIdentifier, username)
            });

            var thandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = thandler.CreateJwtSecurityToken(
                audience: audience,
                issuer: issuer,
                subject: ci,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireT)),
                signingCredentials: signature
            );

            var jwtTokenString = thandler.WriteToken(jwtToken);

            return jwtTokenString;
        }
    }
}