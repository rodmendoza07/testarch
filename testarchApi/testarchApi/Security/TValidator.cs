using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using testarchApi.Connection;

namespace testarchApi.Security
{
    public class TValidator
    {
        public bool ValidT(string token)
        {
            try
            {
                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] sqlParam = new SqlParameter[0];
                string secret = string.Empty;

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.getApiJwtParams(), sqlParam);
                secret = ds.Tables[0].Rows[0].Field<string>("apiSecret");

                var audience = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                var issuer = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];

                var secKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secret));

                SecurityToken securityToken;

                var tokenH = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

                TokenValidationParameters validParams = new TokenValidationParameters()
                {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = secKey
                };

                Thread.CurrentPrincipal = tokenH.ValidateToken(token, validParams, out securityToken);
                HttpContext.Current.User = tokenH.ValidateToken(token, validParams, out securityToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}