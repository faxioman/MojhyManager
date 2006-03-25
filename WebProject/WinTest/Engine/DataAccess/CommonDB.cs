// Classe contenente le funzioni generiche per l'accesso al DB

using System.Data.OleDb;
using System.Data;
using System;
using Npgsql;

namespace Mojhy.DataAccess
{

    /// <summary>
    /// Summary description for CommonDB
    /// </summary>
    public class CommonDB
    {

        private NpgsqlConnection l_objConnection = new NpgsqlConnection();
        
        public void DBConnect()
        {
            string strConnectionString;
            strConnectionString = System.Configuration.ConfigurationManager.AppSettings["myConnection"];
            l_objConnection.ConnectionString = strConnectionString;
            l_objConnection.Open();
        }

        public void DBClose()
        {
            l_objConnection.Close();
        }

        public NpgsqlDataReader GetDataReader(string strSQL)
        {
            try
            {
                if ((l_objConnection.State != ConnectionState.Open))
                {
                    DBConnect();
                }

                NpgsqlCommand cmd = new NpgsqlCommand(strSQL, l_objConnection);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataSet(string strSQL, string strName)
        {
            try
            {
                DataSet ds = new DataSet();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(strSQL, l_objConnection);
                da.Fill(ds, strName);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RunCommand(string strSQL, bool IgnoreError)
        {
            NpgsqlCommand Cmd = new NpgsqlCommand(strSQL, l_objConnection);
            try
            {
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (!IgnoreError)
                {
                    throw ex;
                }
            }
        }

        public void RunCommand(string strSQL)
        {
            RunCommand(strSQL, true);
        }

        public void Close()
        {
            
        }

        public string FormatField(object varIn, bool IsString, bool UseComma)
        {
            string strOut;
            strOut = varIn.ToString();
            if ((strOut.IndexOf("\'") > -1))
            {
                strOut = strOut.Replace("\'", "\'\'");
            }
            if (IsString)
            {
                strOut = ("\'"
                            + (strOut + "\'"));
            }
            if (UseComma)
            {
                strOut = (strOut + ", ");
            }
            return strOut;
        }

        public int GetField(string SQL, string FieldName)
        {
            int Out = -1;
            NpgsqlDataReader DR = GetDataReader(SQL);
                        
            while (DR.Read())
            {
                Out = (int)DR[FieldName];
            }
            DR.Close();
            return Out;
        }
    }

}