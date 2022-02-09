using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using testarchApi.Connection;
using testarchApi.Models;
using testarchApi.Security;

namespace testarchApi.Controllers
{
    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class LoginController : ApiController
    {
        /// <summary>
        /// Método de autenticación de usuario
        /// </summary>
        /// <param name="credenciales"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(Login credenciales)
        {
            try
            {
                AesSecurity aes = new AesSecurity();
                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[2];
                string cu = string.Empty;
                string cp = string.Empty;
                string cId = string.Empty;

                cp = aes.TCifrado(credenciales.pwd + credenciales.userName);
                cu = aes.TCifrado(credenciales.userName);

                p[0] = new SqlParameter("@email", credenciales.userName);
                p[1] = new SqlParameter("@pwd", cp);

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.GetInfoUser(), p);

                cId = aes.TCifrado(ds.Tables[0].Rows[0].Field<int>("id").ToString());
                var token = TokenMaker.TJwt(cu, cId);

                return Json(new
                {
                    messge = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    nombre = ds.Tables[0].Rows[0].Field<string>("nombre"),
                    usuario = ds.Tables[0].Rows[0].Field<string>("nombreUsuario"),
                    token = token
                });
            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Credenciales incorrectas")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.Unauthorized.ToString()),
                        new JProperty("code", HttpStatusCode.Unauthorized),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.Unauthorized;
                    r.ReasonPhrase = ex.Message;
                    r.Content = new StringContent(m, Encoding.UTF8, "application/json");

                    throw new HttpResponseException(r);
                }
                else
                {
                    JObject jObject = new JObject(
                         new JProperty("message", HttpStatusCode.InternalServerError.ToString()),
                         new JProperty("code", HttpStatusCode.InternalServerError),
                         new JProperty("response", ex.Message)
                     );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.InternalServerError;
                    r.ReasonPhrase = ex.Message;
                    r.Content = new StringContent(m, Encoding.UTF8, "application/json");

                    throw new HttpResponseException(r);
                }
            }
        }
        [HttpPost]
        [Route("VT")]
        public IHttpActionResult Validtoken([FromBody] dynamic d)
        {
            try
            {
                string t = string.Empty;
                t = d["token"];
                TValidator tv = new TValidator();
                if (tv.ValidT(t))
                {
                    return Json(new
                    {
                        messge = HttpStatusCode.OK.ToString(),
                        code = HttpStatusCode.OK
                    });
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.Unauthorized.ToString()),
                        new JProperty("code", HttpStatusCode.Unauthorized)
                    );

                var m = JsonConvert.SerializeObject(jObject);

                r.StatusCode = HttpStatusCode.Unauthorized;
                r.ReasonPhrase = HttpStatusCode.Unauthorized.ToString();
                r.Content = new StringContent(m, Encoding.UTF8, "application/json");

                throw new HttpResponseException(r);
            }
        }
    }
}
