using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using testarchApi.Connection;

namespace testarchApi.Security
{
    public class AesSecurity
    {
        AesCryptoServiceProvider crypt_prov;

        public AesSecurity()
        {
            ConexionDb c = new ConexionDb();
            DataSet ds = new DataSet();
            SqlParameter[] sqlParam = new SqlParameter[0];
            string key = string.Empty;
            string iv = string.Empty;
            int keySize = 32;
            int ivSize = 16;

            ds = c.ExecuteCommand(c.getConnectionSQL(), QueryCollection.GetAesParams(), sqlParam);
            iv = ds.Tables[0].Rows[0].Field<string>("IV");
            key = ds.Tables[1].Rows[0].Field<string>("Key");

            byte[] k = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
            byte[] i = Convert.FromBase64String(iv);

            Array.Resize(ref k, keySize);
            Array.Resize(ref i, ivSize);

            crypt_prov = new AesCryptoServiceProvider();

            crypt_prov.BlockSize = 128;
            crypt_prov.KeySize = 256;
            crypt_prov.IV = i;
            crypt_prov.Key = k;
            crypt_prov.Mode = CipherMode.CBC;
            crypt_prov.Padding = PaddingMode.PKCS7;
        }

        public String TCifrado(string t)
        {
            ICryptoTransform transform = crypt_prov.CreateEncryptor();
            string eText = string.Empty;

            byte[] ebytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(t), 0, t.Length);

            eText = Convert.ToBase64String(ebytes);

            return eText;
        }

        public String TDescifrado(string t)
        {
            ICryptoTransform transform = crypt_prov.CreateDecryptor();
            string dText = string.Empty;

            byte[] ebytes = Convert.FromBase64String(t);
            byte[] dbytes = transform.TransformFinalBlock(ebytes, 0, ebytes.Length);

            dText = ASCIIEncoding.ASCII.GetString(dbytes);

            return dText;
        }
    }
}