using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testarchApi.Models
{
    public class Task
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public string Estado { get; set; }
        public string taskId { get; set; }
    }
}