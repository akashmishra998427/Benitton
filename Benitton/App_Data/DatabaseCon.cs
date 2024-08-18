using Antlr.Runtime.Tree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace Benitton.App_code
{
    public class DatabaseCon
    {

        static string query, querypro = string.Empty;
        static DataTable dt = null;
        static SqlDataAdapter sda = null;
        static SqlCommand cmd = null;
        static DataSet ds = null;
        static SqlConnection con = null;
        static SqlConnection com = null;

        static DatabaseCon()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        }

        public DataSet get_Category()
        {
            SqlCommand com = new SqlCommand("select ID,title from tbl_category where Isactive='1'", con);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet get_subCategory(int catid)
        {
            SqlCommand com = new SqlCommand("select ID,title from tbl_category where where CategoryId=@catid and Isactive='1'", con);
            com.Parameters.AddWithValue("catid", catid);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }
        public DataSet get_productByid(int subcatid)
        {
            SqlCommand com = new SqlCommand("select ID,title from tbl_category where where CategoryId=@subcatid and Isactive='1'", con);
            com.Parameters.AddWithValue("subcatid", subcatid);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static DataTable GetData(string dataString)
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

        public static int RunCmd(string Query)
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

        public static object DataTableToJSON(DataTable table)
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

        public static string Decrypt(string cipherText)
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