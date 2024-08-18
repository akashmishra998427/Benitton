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

namespace Benitton.Database_Access_Layer
{
    public class Db
    {
        public string query, querypro = string.Empty;
        public DataTable dt = null;
        public SqlDataAdapter sda = null;
        public SqlCommand cmd = null;
        public DataSet ds = null;
        public SqlConnection com = null;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
            

            public DataSet get_Category()
            {
                try { 
                    SqlCommand com = new SqlCommand("select * from tbl_category where Isactive=1", con);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                   da.Fill(ds);
                    return ds;
                }
                catch (Exception ex)
                {
                    var response = new { status = 0, message = ex.Message };
                    return Json(response);
                }
            }

            private DataSet Json(object response)
            {
                throw new NotImplementedException();
            }

        
        public DataSet get_AllCategory(int catid)
        {
            try
            {
                SqlCommand com = new SqlCommand("select * from tbl_category where Isactive=1 and MenuID="+ catid + "", con);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        public DataSet get_subCategory(int catid)
            {
                SqlCommand com = new SqlCommand("select ID,title from Tbl_subCategory where  CategoryId=@catid AND Isactive=1  ORDER BY serialorder", con);
                com.Parameters.AddWithValue("catid", catid);
            //com.Parameters.AddWithValue("catid", catid);
            SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;

            }
            public DataSet get_productByid(int subcatid)
            {
                SqlCommand com = new SqlCommand("select ID,title from tbl_product where  SubCategoryID=@subcatid AND Isactive=1 AND Isdelete = 0 ORDER BY serialorder", con);
                com.Parameters.AddWithValue("subcatid", subcatid);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
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


    }
}