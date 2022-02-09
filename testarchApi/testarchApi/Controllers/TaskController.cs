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
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using testarchApi.Connection;
using testarchApi.Models;
using testarchApi.Security;

namespace testarchApi.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class TaskController : ApiController
    {
        /// <summary>
        /// Método para recuperar todas las tareas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Task")]
        public IHttpActionResult GetTasks()
        {
            try
            {
                AesSecurity aes = new AesSecurity();
                var identity = Thread.CurrentPrincipal.Identity;
                int userId = Int32.Parse(aes.TDescifrado(identity.Name.ToString()));

                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[3];
                string r = string.Empty;

                p[0] = new SqlParameter("@taskId", 0);
                p[1] = new SqlParameter("@taskuserId", userId);
                p[2] = new SqlParameter("@operationOpt", 0);

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.GetAllTasks(), p);
                r = JsonConvert.SerializeObject(ds.Tables[0]);

                return Json(new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = JsonConvert.DeserializeObject(r)
                });

            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Opción inválida")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.BadRequest.ToString()),
                        new JProperty("code", HttpStatusCode.BadRequest),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.BadRequest;
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

        /// <summary>
        /// Método para obtener el detalle de cada tarea
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Task/{taskId}")]
        public IHttpActionResult GetTaskByUser(string taskId)
        {
            try
            {
                AesSecurity aes = new AesSecurity();
                var identity = Thread.CurrentPrincipal.Identity;
                int userId = Int32.Parse(aes.TDescifrado(identity.Name.ToString()));

                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[3];
                string r = string.Empty;

                p[0] = new SqlParameter("@taskId", taskId);
                p[1] = new SqlParameter("@taskuserId", userId);
                p[2] = new SqlParameter("@operationOpt", 1);

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.GetAllTasks(), p);
                r = JsonConvert.SerializeObject(ds.Tables[0]);

                return Json(new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = JsonConvert.DeserializeObject(r)
                });

            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Opción inválida")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.BadRequest.ToString()),
                        new JProperty("code", HttpStatusCode.BadRequest),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.BadRequest;
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

        /// <summary>
        /// Método para agregar tarea
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register/Task")]
        public IHttpActionResult AddTask(Task task)
        {
            try
            {
                AesSecurity aes = new AesSecurity();
                var identity = Thread.CurrentPrincipal.Identity;
                int userId = Int32.Parse(aes.TDescifrado(identity.Name.ToString()));

                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[6];

                p[0] = new SqlParameter("@taskTitle", task.nombre);
                p[1] = new SqlParameter("@taskDesc", task.descripcion);
                p[2] = new SqlParameter("@taskStatus", 0);
                p[3] = new SqlParameter("@taskUser", userId);
                p[4] = new SqlParameter("@taskId", 0);
                p[5] = new SqlParameter("@operationOpt", 0);

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.AddTask(), p);

                return Json(new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Opción inválida")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.BadRequest.ToString()),
                        new JProperty("code", HttpStatusCode.BadRequest),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.BadRequest;
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

        /// <summary>
        /// Método para actualizar la tarea
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update/Task")]
        public IHttpActionResult UpdateTask(Task task)
        {
            try
            {
                AesSecurity aes = new AesSecurity();
                var identity = Thread.CurrentPrincipal.Identity;
                int userId = Int32.Parse(aes.TDescifrado(identity.Name.ToString()));

                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[6];

                p[0] = new SqlParameter("@taskTitle", task.nombre);
                p[1] = new SqlParameter("@taskDesc", task.descripcion);
                p[2] = new SqlParameter("@taskStatus", task.Estado);
                p[3] = new SqlParameter("@taskUser", userId);
                p[4] = new SqlParameter("@taskId", task.taskId);
                p[5] = new SqlParameter("@operationOpt", 1);

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.AddTask(), p);

                return Json(new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Opción inválida")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.BadRequest.ToString()),
                        new JProperty("code", HttpStatusCode.BadRequest),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.BadRequest;
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

        /// <summary>
        /// Método para eliminar tarea
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete/Task")]
        public IHttpActionResult DeleteTask(Task task)
        {
            try
            {
                AesSecurity aes = new AesSecurity();
                var identity = Thread.CurrentPrincipal.Identity;
                int userId = Int32.Parse(aes.TDescifrado(identity.Name.ToString()));

                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[6];

                p[0] = new SqlParameter("@taskTitle", task.nombre);
                p[1] = new SqlParameter("@taskDesc", task.descripcion);
                p[2] = new SqlParameter("@taskStatus", task.Estado);
                p[3] = new SqlParameter("@taskUser", userId);
                p[4] = new SqlParameter("@taskId", task.taskId);
                p[5] = new SqlParameter("@operationOpt", 2);

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.AddTask(), p);

                return Json(new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Opción inválida")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.BadRequest.ToString()),
                        new JProperty("code", HttpStatusCode.BadRequest),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.BadRequest;
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

        [HttpGet]
        [Route("Filtro/Task/{idF}")]
        public IHttpActionResult GetFilterTask(int idF)
        {
            try
            {
                AesSecurity aes = new AesSecurity();
                var identity = Thread.CurrentPrincipal.Identity;
                int userId = Int32.Parse(aes.TDescifrado(identity.Name.ToString()));

                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[3];
                string r = string.Empty;

                p[0] = new SqlParameter("@taskId", 0);
                p[1] = new SqlParameter("@taskuserId", userId);
                p[2] = new SqlParameter("@operationOpt", idF);

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.GetAllTasks(), p);
                r = JsonConvert.SerializeObject(ds.Tables[0]);

                return Json(new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = JsonConvert.DeserializeObject(r)
                });

            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Opción inválida")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.BadRequest.ToString()),
                        new JProperty("code", HttpStatusCode.BadRequest),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.BadRequest;
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

        /// <summary>
        /// Devuelve el catalogo de estados de la tarea
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Estados/Task")]
        public IHttpActionResult GetCatEstados()
        {
            try
            {
                AesSecurity aes = new AesSecurity();
                var identity = Thread.CurrentPrincipal.Identity;
                int userId = Int32.Parse(aes.TDescifrado(identity.Name.ToString()));

                ConexionDb c = new ConexionDb();
                DataSet ds = new DataSet();
                SqlParameter[] p = new SqlParameter[0];
                string r = string.Empty;

                ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.GetCatEstdo(), p);
                r = JsonConvert.SerializeObject(ds.Tables[0]);

                return Json(new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = JsonConvert.DeserializeObject(r)
                });
            }
            catch (Exception ex)
            {
                HttpResponseMessage r = new HttpResponseMessage();

                if (ex.Message == "Opción inválida")
                {
                    JObject jObject = new JObject(
                        new JProperty("message", HttpStatusCode.BadRequest.ToString()),
                        new JProperty("code", HttpStatusCode.BadRequest),
                        new JProperty("response", ex.Message)
                    );

                    var m = JsonConvert.SerializeObject(jObject);

                    r.StatusCode = HttpStatusCode.BadRequest;
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
