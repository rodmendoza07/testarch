using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using testarchApi.Connection;

namespace testarchApi.Security
{
    internal class TokenValidationManager : DelegatingHandler
    {
        private static bool TryRecuperacionToken(HttpRequestMessage req, out string t)
        {
            t = null;
            IEnumerable<string> aHeaders;

            if (!req.Headers.TryGetValues("Authorization", out aHeaders) || aHeaders.Count() > 1)
            {
                return false;
            }

            var bToken = aHeaders.ElementAt(0);

            t = bToken.StartsWith("Bearer ") ? bToken.Substring(7) : bToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken cancellationToken)
        {
            HttpResponseMessage r = new HttpResponseMessage();
            HttpStatusCode hStatus;
            string token = string.Empty;
            string m = string.Empty;

            if (!TryRecuperacionToken(req, out token))
            {
                hStatus = HttpStatusCode.Unauthorized;
                return base.SendAsync(req, cancellationToken);
            }

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

                return base.SendAsync(req, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.Unauthorized.ToString()),
                        new JProperty("code", HttpStatusCode.Unauthorized),
                        new JProperty("response", "Token inválido")
                    );

                m = JsonConvert.SerializeObject(jObject);
                //r.StatusCode = HttpStatusCode.Unauthorized;
                //r.ReasonPhrase = "Token expirado";
                //r.Content = new StringContent(m, Encoding.UTF8, "application/json");

                //throw new HttpResponseException(r);
                hStatus = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.InternalServerError.ToString()),
                        new JProperty("code", HttpStatusCode.InternalServerError),
                        new JProperty("response", ex.Message)
                    );

                m = JsonConvert.SerializeObject(jObject);
                hStatus = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(hStatus)
            {
                StatusCode = hStatus,
                ReasonPhrase = hStatus.ToString(),
                Content = new StringContent(m, Encoding.UTF8, "application/json")
            });
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