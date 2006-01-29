// Classe contenente le funzioni generiche per l'accesso al DB

namespace Mojhy.DataAccess
{

    using System.Data.OleDb;
    using System.Data;
    using Npgsql;

    /// <summary>
    /// Summary description for CommonDB
    /// </summary>
    public class CommonDB
    {

        private NpgsqlConnection m_objConnection = new NpgsqlConnection();
        
        public void DBConnect()
        {
            string strConnectionString;
            strConnectionString = System.Configuration.ConfigurationSettings.AppSettings["myConnection"];
            m_objConnection.ConnectionString = strConnectionString;            
            m_objConnection.Open();
        }

        public void DBClose()
        {
            m_objConnection.Close();
        }

        public NpgsqlDataReader GetDataReader(string strSQL)
        {
            try
            {
                if ((m_objConnection.State != ConnectionState.Open))
                {
                    DBConnect();
                }

                NpgsqlCommand cmd = new NpgsqlCommand(strSQL, m_objConnection);
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
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(strSQL, m_objConnection);
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
            NpgsqlCommand Cmd = new NpgsqlCommand(strSQL, DBConnect.cn);
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
            return RunCommand(strSQL, true);
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

        public int GetID(string Table, string IDField)
        {
            int Out;
            string SQL = ("SELECT TOP 1 "
                        + (IDField + (" FROM "
                        + (Table + (" ORDER BY "
                        + (IDField + " DESC "))))));
            NpgsqlDataAdapter DR = GetDataReader(SQL);
            while (DR.Read)
            {
                Out = DR.Item[IDField];
            }
            DR.Close();
            return Out;
        }
    }

}