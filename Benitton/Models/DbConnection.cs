
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Benitton.Models
{
    public class DatabaseCon
    {

        public string query, querypro = string.Empty;
        public DataTable dt = null;
        public SqlDataAdapter sda = null;
        public SqlCommand cmd = null;
        public DataSet ds = null;
        public SqlConnection con = new SqlConnection();
        public SqlConnection com = null;

        public DatabaseCon()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        }

        public DataTable GetData(string dataString)
        {
            try
            {
                dt = new DataTable();
                query = dataString;
                sda = new SqlDataAdapter(query, con);
                ds = new DataSet();
                sda.SelectCommand.CommandTimeout = 480;
                sda.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public int RunCmd(string Query)
        {
            int d = 0;
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand(Query, con);
                d = Convert.ToInt32((object)com.ExecuteNonQuery());

                return d;
            }
            catch (Exception ex)
            {
                return d;
            }
            finally
            {
                con.Close();
            }
        }

        public object DataTableToJSON(DataTable table)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "JAY2VEER4PBNI99212";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {

                return "0";
            }
        }

    }
}