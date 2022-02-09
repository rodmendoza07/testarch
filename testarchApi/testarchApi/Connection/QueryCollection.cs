using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testarchApi.Connection
{
    public class QueryCollection
    {
        public static string GetAesParams()
        {
            return "testarchdb.dbo.sp_getAesParams";
        }
        public static string getApiJwtParams()
        {
            return "testarchdb.dbo.sp_getJWTParams";
        }
        public static string GetInfoUser()
        {
            return "testarchdb.dbo.sp_getInfoUser";
        }
        public static string AddUsers()
        {
            return "testarchdb.dbo.sp_registerUser";
        }
    }
}