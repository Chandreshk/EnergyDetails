using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ClientDLL
{
    public class DataLib
    {
        public DataLib()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public enum Connection
        {
            ConnectionString
            // Add more here (check the comma's!)
        }

        public static string GetConnectionString(Connection connType)
        {

            string retValue = "";
            switch (connType)
            {
                case Connection.ConnectionString:
                    {
                        retValue = ConfigurationManager.AppSettings["SqlConnect"].ToString();
                        break;
                    }
            }
            return retValue;
        }
        public static DataSet GetStoredProcData(Connection connType, string strSP, SqlParameter[] arrSPParams)
        {
            string connStr = GetConnectionString(connType);
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                //Prepare the Command
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = strSP;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Add stored procedure parameters to Command
                    if (arrSPParams != null)
                    {
                        foreach (SqlParameter param in arrSPParams)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.SelectCommand = cmd;
                        //DO NOT open the connection, allow the fill command to do this
                        adapter.Fill(ds);
                        adapter.SelectCommand = null;
                    }
                }
            }
            return ds;
        }
        public static int ExecuteStoredProcData(Connection connType, string strSP, SqlParameter[] arrSPParams)
        {
            string connStr = GetConnectionString(connType);
            int RowsAffected = 0;


            using (SqlConnection con = new SqlConnection(connStr))
            {
                //Prepare the Command
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = 10000;
                    cmd.Connection = con;
                    cmd.CommandText = strSP;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //Add stored procedure parameters to Command
                    if (arrSPParams != null)
                    {
                        foreach (SqlParameter param in arrSPParams)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }
                    try
                    {
                        if (con.State == ConnectionState.Open) con.Close(); con.Open();
                        RowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open) con.Close();
                    }

                }
            }
            return RowsAffected;
        }
        public static string GetScalarStringStoredProcData(Connection connType, string strSP, SqlParameter[] arrSPParams)
        {
            // This funtion returns Single String Type value 
            // August 11, 2008  

            string connStr = string.Empty;
            string retString = string.Empty;
            connStr = GetConnectionString(connType);


            using (SqlConnection con = new SqlConnection(connStr))
            {
                //Prepare the Command
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = strSP;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Add stored procedure parameters to Command
                    if (arrSPParams != null)
                    {
                        foreach (SqlParameter param in arrSPParams)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }
                    if (con.State == ConnectionState.Open) con.Close(); con.Open();
                    retString = Convert.ToString(cmd.ExecuteScalar());
                    //retString = arrSPParamsOut[0].Value.ToString();
                    con.Close();
                }
            }
            return retString;
        }
    }
}
