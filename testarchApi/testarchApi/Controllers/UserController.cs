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
    public class UserController : ApiController
    {
        /// <summary>
        /// Registro de usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register/User")]
        public IHttpActionResult AddUser(User user)
        {
            try
            {
                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[5];
                AesSecurity aes = new AesSecurity();
                string cpass = aes.TCifrado(user.Pass + user.Email);


                p[0] = new SqlParameter("@nombre", user.Nombre);
                p[1] = new SqlParameter("@pApellido", user.Papellido);
                p[2] = new SqlParameter("@sApellido", user.Sapellido);
                p[3] = new SqlParameter("@email", user.Email);
                p[4] = new SqlParameter("@pwd", cpass);

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.AddUsers(), p);

                return Json(new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Usuario existente")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.Forbidden.ToString()),
                        new JProperty("code", HttpStatusCode.Forbidden),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.Forbidden;
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
    }
}
