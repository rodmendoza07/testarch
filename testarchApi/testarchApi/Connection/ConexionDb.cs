using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace testarchApi.Connection
{
    public class ConexionDb
    {
        public SqlConnection getConnectionSQL(string connConfig = "SqlStringConn")
        {
            SqlConnection conSql = new SqlConnection();
            try
            {
                conSql.ConnectionString = ConfigurationManager.ConnectionStrings[connConfig].ToString();
                return conSql;

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }//getConectionSQL

        public DataSet ExecuteConsult(SqlConnection conection, string nameSP)
        {

            DataSet datatable = new DataSet();
            SqlCommand comand = new SqlCommand();
            try
            {
                comand.CommandTimeout = 120;
                comand.Connection = conection;
                comand.CommandText = nameSP;
                comand.CommandType = CommandType.Text;
                comand.Connection.Open();
                SqlDataAdapter sqladapter = new SqlDataAdapter(comand);
                sqladapter.Fill(datatable);

                return datatable;
            }

            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                conection.Close();
            }
        }

        public DataSet ExecuteDataSet(SqlConnection conection, string nameSP)
        {
            DataSet dataset = new DataSet();
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandTimeout = 120;
                command.Connection = conection;
                command.CommandText = nameSP;
                command.CommandType = CommandType.Text;
                command.Connection.Open();
                SqlDataAdapter sqladapter = new SqlDataAdapter(command);
                sqladapter.Fill(dataset);
                return dataset;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                conection.Close();
            }

        }//ExecuteDataSet

        public DataSet ExecuteCommand(SqlConnection conection, string SProcedure, SqlParameter[] sqlParam)
        {
            SqlCommand comand = new SqlCommand();
            DataSet dataset = new DataSet();
            try
            {
                comand.CommandTimeout = 120;
                comand.Connection = conection;
                comand.CommandText = SProcedure;
                comand.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter param in sqlParam)
                {
                    comand.Parameters.Add(param);
                }
                comand.Connection.Open();
                SqlDataAdapter sqladapter = new SqlDataAdapter(comand);
                sqladapter.Fill(dataset);

                return dataset;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            finally
            {
                conection.Close();
            }
        }//ExecuteCommand
        public DataSet ExecuteCommand(SqlConnection conection, string SProcedure, List<SqlParameter> sqlParam)
        {
            SqlCommand comand = new SqlCommand();
            DataSet dataset = new DataSet();
            try
            {
                comand.CommandTimeout = 120;
                comand.Connection = conection;
                comand.CommandText = SProcedure;
                comand.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter param in sqlParam)
                {
                    comand.Parameters.Add(param);
                }
                comand.Connection.Open();
                SqlDataAdapter sqladapter = new SqlDataAdapter(comand);
                sqladapter.Fill(dataset);

                return dataset;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conection.Close();
            }
        }//ExecuteCommand
        internal DataSet ExecuteCommand(SqlConnection sqlConnection, string v)
        {
            throw new NotImplementedException();
        }
    }
}