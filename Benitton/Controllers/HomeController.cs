using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Benitton.Models;
using Newtonsoft.Json;

namespace Benitton.Controllers
{

    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        Benitton_dbEntities context = new Benitton_dbEntities();
        // DatabaseCon DbCon = new DatabaseCon();
        Database_Access_Layer.Db dblayer = new Database_Access_Layer.Db();
        //string domainName = "https://ajashy.vcqru.com/";
        //string sendToAdmin = "itdepartment@ajashy.com";
        //string sendtomailBcc = "a.khajuria@ajashy.com";
        //string sendtomailCc = "info@ajashy.com";
        //// service Form
        //string sendOtherMailServ = "service.support@ajashy.com";
        //// contact 
        //string sendMailOtherContact = "info@ajashy.com";
        //// proEnq 
        //string sendMailOtherProenq = "info@ajashy.com";
        ////career
        //string sendMailToCareer = "s.jain@ajashy.com";

        string domainName = "http://benitton.vcqru.com/";
        string sendToAdmin = "NA";
        string sendtomailBcc = "NA";
        string sendtomailCc = "NA";
        // service Form
        string sendOtherMailServ = "NA";
        // contact 
        string sendMailOtherContact = "NA";
        // proEnq 
        string sendMailOtherProenq = "NA";
        //career
        string sendMailToCareer = "NA";

        public ActionResult Index()
        
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];

            var aboutus = context.about_us.FirstOrDefault();
            
            
            var Bannerlistdata = context.Tbl_banner.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            var catdata = context.tbl_category.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            var subktdata = context.Tbl_subCategory.Where(x => x.display_on_home == true && x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            var depdata = context.tbl_product.Where(x => x.display_on_home == true && x.Isactive == true && x.Isdelete == false).OrderBy(x => x.serialorder).ToList();
            var certificationData = context.about_certification.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            var MenuData = context.tbl_Menu.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            ViewProducts model = new ViewProducts
            {
                tbl_singleAboutus = aboutus,
                tbl_bannerList = Bannerlistdata,
                categoryMd = catdata,
                subcategoryMd = subktdata,
                ProductMd = depdata,
                MenuMd = MenuData,
                certificationList = certificationData
            };
            return View(model);
        }

        public void getSubMenu(int catid)
        {
            DataSet dataSet = dblayer.get_subCategory(catid);
            List<Subcategory> subcatmenulist = new List<Subcategory>();
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                subcatmenulist.Add(new Subcategory
                {
                    SubCatId = Convert.ToInt32(dr["ID"]),
                    SubCategoryName = dr["title"].ToString()
                });

            }
            Session["subMenu"] = subcatmenulist;
        }
        public void getSubMenuProduct(int subcatid)
        {
            DataSet dataSet = dblayer.get_productByid(subcatid);
            List<Product> subcaProducttmenulist = new List<Product>();
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                subcaProducttmenulist.Add(new Product
                {
                    ProductId = Convert.ToInt32(dr["ID"]),
                    ProductName = dr["title"].ToString()
                });

            }
            Session["subProductMenu"] = subcaProducttmenulist;
        }

        public void getAllCategory(int subcatid)
        {
            DataSet dataSet = dblayer.get_AllCategory(subcatid);
            List<Category> subcaProducttmenulist = new List<Category>();
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                subcaProducttmenulist.Add(new Category
                {
                    CatId = Convert.ToInt32(dr["ID"]),
                    CatName = dr["title"].ToString(),
                    image_file = dr["image_file"].ToString()
                });

            }
            Session["AllCategory"] = subcaProducttmenulist;
        }

        public void getAllSubCategory(int subcatid)
        {
            DataSet dataSet = dblayer.get_subCategory(subcatid);
            List<Product> subcaProducttmenulist = new List<Product>();
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                subcaProducttmenulist.Add(new Product
                {
                    ProductId = Convert.ToInt32(dr["ID"]),
                    ProductName = dr["title"].ToString()
                });

            }
            Session["AllSubCategory"] = subcaProducttmenulist;
        }


        [Route("~/Catalogy/{title}")]
        public ActionResult Catalogy(string title)
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Title = title.Replace("-", " ");
            ViewBag.Message = title.Replace("-", " ");
            string titleNew = title.Replace("-", " ");
            var data = context.tbl_category.Select(p => new { p.ID, p.title,p.detail }).Where(u => u.title == title.Replace("-", " ")).FirstOrDefault();
            ViewBag.CatId = data.ID;
            ViewBag.Message = data.detail;

            SqlConnection sqlConnection = new SqlConnection(context.Database.Connection.ConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select b.ID as SubId, a.title as CatTitle,a.detail as CatDetail, b.title as Title,b.image_file as Image_1 from tbl_category as a, Tbl_subCategory as b WHERE a.ID=b.CategoryID  and a.title='" + titleNew + "'", sqlConnection);
            DataSet ds1 = new DataSet();
            sqlDataAdapter.Fill(ds1);

            if (ds1.Tables[0].Rows.Count == 0)
                return Redirect("~/CategoryProduct/"+ data.ID + "/0");

            return View(ds1.Tables[0]);
        }

        [Route("~/CategoryProduct/{CategoryId}/{SubCategoryId}")]
        public ActionResult CategoryProduct(int CategoryId, int SubCategoryId)
        {
            DataSet ds = dblayer.get_subCategory(CategoryId);
            
            
            

            string Qry = "select * from tbl_product where CategoryID="+ CategoryId + " and SubCategoryID="+ SubCategoryId + " and Isactive=1";
            if(SubCategoryId==0)
                Qry = "select * from tbl_product where CategoryID=" + CategoryId + " and Isactive=1";

            SqlConnection sqlConnection = new SqlConnection(context.Database.Connection.ConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(Qry, sqlConnection);
            DataSet ds1 = new DataSet();
            sqlDataAdapter.Fill(ds1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.Category = ds.Tables[0];

                var data = context.Tbl_subCategory.Select(p => new { p.ID, p.title, p.detail, p.CategoryID }).Where(u => u.CategoryID == CategoryId).FirstOrDefault();
                ViewBag.CatId = data.ID;
                ViewBag.Title = data.title;

                ViewBag.Message = data.detail;
            }
            else
            {
                var data = context.tbl_category.Select(p => new { p.ID, p.title,p.detail }).Where(u => u.ID == CategoryId).FirstOrDefault();
                ViewBag.CatId = data.ID;
                ViewBag.Title = data.title;

                ViewBag.Message = data.detail;

            }
            return View(ds1.Tables[0]);
        }

        [Route("~/dealership")]
        public ActionResult Dealership()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Dealerships";
            ViewBag.CatId = 27;
            var subktdata = context.Tbl_subCategory.Where(x => x.display_on_home == true && x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            var depdata = context.tbl_product.Where(x => x.display_on_home == true && x.Isactive == true && x.Isdelete == false).OrderByDescending(x => x.ID).ToList();

            ViewProducts model = new ViewProducts
            {
                subcategoryMd = subktdata,
                ProductMd = depdata
            };

            return View(model);
        }

        [HttpGet]
        [Route("~/search")]
        public ActionResult search()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Search.";
            var data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.otherCatID == 40).OrderByDescending(x => x.ID).ToList();

            return View(data);
        }

        [HttpPost]
        [Route("~/search")]
        public ActionResult search(string SearchName = "")
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            if (SearchName == "")
            {
                ViewBag.Message = "Search Products";
            }
            else
            {
                ViewBag.Message = SearchName;
            }

            //var subktdata = context.Tbl_subCategory.Where(x => x.display_on_home == true && x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            var data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.Title.Contains(SearchName)).OrderByDescending(x => x.ID).ToList();
            if (data.Count < 1)
            {
                var subktdata = context.Tbl_subCategory.Where(x => x.Isactive == true && x.title.Contains(SearchName)).ToList();
                if (subktdata.Count > 0)
                {
                    var subcatid = subktdata[0].ID.ToString();
                    data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.SubCategoryID.ToString() == subcatid).OrderByDescending(x => x.ID).ToList();

                }
                if (subktdata.Count < 1)
                {
                    if (SearchName.Contains("road"))
                    {
                        data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.otherCatID == 1).OrderByDescending(x => x.ID).ToList();
                    }
                    else if (SearchName.Contains("snow"))
                    {
                        data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.otherCatID == 2).OrderByDescending(x => x.ID).ToList();
                    }
                    else if (SearchName.Contains("green"))
                    {
                        data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.otherCatID == 3).OrderByDescending(x => x.ID).ToList();
                    }
                    else if (SearchName.Contains("cons"))
                    {
                        data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.otherCatID == 4).OrderByDescending(x => x.ID).ToList();
                    }
                }
            }
            if (data.Count < 1)
            {
                string[] words = SearchName.Split(' ');
                SearchName = words[0];

                data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.Title.Contains(SearchName)).OrderByDescending(x => x.ID).ToList();
                if (data.Count < 1 && words.Count() > 1)
                {
                    SearchName = words[1];
                    data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.Title.Contains(SearchName)).OrderByDescending(x => x.ID).ToList();

                }

            }
            return View(data);
        }

        [Route("~/refurbished-equipment")]
        public ActionResult refurbishedequipment()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Refurbished Equipment";
            ViewBag.SubCatTitle = "refurbished-rquipment";
            ViewBag.CatId = 31;
            //var subktdata = context.Tbl_subCategory.Where(x => x.display_on_home == true && x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            var data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.CategoryID == 31).OrderBy(x => x.serialorder).ToList();
            return View(data);
        }

        [Route("~/Product/{title}")]
        public ActionResult Product(string title)
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            // ViewBag.Message = title;
            //ViewBag.CatTitle = d1.Rows[0]["SubCategoryID"].ToString();
            ViewBag.OtherCatTitle = "";
            title = title.Replace(" ", "-");
            ViewBag.subcatid = "";

            ViewBag.Message = title.Replace("-", " ");
            var data = context.tbl_product.Select(p => new { p.ID, p.SubCategoryID, p.CategoryID, p.Title }).Where(u => u.Title == title.Replace("-", " ")).FirstOrDefault();
            DataSet ds1 = new DataSet();
            DataTable d1;
            DataTable d2;
            try
            {

                if (data.ID.ToString() != "")
                {
                    d1 = dblayer.GetData("SELECT a.*,b.title as subcatName FROM tbl_product as a, Tbl_subCategory as b WHERE a.SubCategoryID=b.ID and a.ID=" + data.ID);
                    d1.TableName = "Table1";
                    int subCatID = Convert.ToInt32(d1.Rows[0]["SubCategoryID"].ToString());
                    ViewBag.subcatid = subCatID;

                    ViewBag.SubCatTitle = d1.Rows[0]["subcatName"].ToString();

                    d2 = dblayer.GetData("SELECT * FROM tbl_product WHERE SubCategoryID = " + subCatID + " AND Isactive = 1 AND Isdelete = 0 order by created_at desc");
                    d2.TableName = "Table2";

                    ds1.Tables.Add(d1.Copy());
                    ds1.Tables.Add(d2.Copy());
                    // var breadurl = "";
                    //var breadtitle = "";
                    var otherCatID = "";
                    var otherTitle = "";
                    if (d1.Rows[0]["otherCatID"] != null)
                    {
                        otherCatID = d1.Rows[0]["otherCatID"].ToString();
                    }

                    if (subCatID.ToString() == "4") //16 ref, 4=GF gordni
                    {

                        if (otherCatID == "1")
                        {
                            otherTitle = "Road-Line";
                        }
                        if (otherCatID == "2")
                        {
                            otherTitle = "Snow-Line";
                        }
                        if (otherCatID == "3")
                        {
                            otherTitle = "Green-Line";
                        }
                        if (otherCatID == "4")
                        {
                            otherTitle = "Construction-Line";
                        }
                        ViewBag.OtherCatTitle = otherTitle;
                        //breadurl = "Dealership/" + d1.Rows[0]["subcatName"] + otherTitle;
                        // breadtitle = subcatname;
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(ds1);
        }

        [Route("~/GF-Gordini/{title}")]
        public ActionResult GFGordini(string title)
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            int otherCatID = 0;
            if (title == "Road-Line")
            {
                otherCatID = 1;
            }
            if (title == "Snow-Line")
            {
                otherCatID = 2;
            }
            if (title == "Green-Line")
            {
                otherCatID = 3;
            }
            if (title == "Construction-Line")
            {
                otherCatID = 4;
            }

            ViewBag.Message = "GF Gordini";
            ViewBag.SubCatTitle = title.Replace("-", " "); var data = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false && x.SubCategoryID == 4 && x.otherCatID == otherCatID).OrderBy(x => x.serialorder).ToList();
            return View(data);
        }

        [Route("~/{categoryTitle}/{title}")]
        public ActionResult SubCategory(string categoryTitle, string title)
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = title.Replace("-", " ");
            ViewBag.CatTitle = categoryTitle;
            //var data = context.tbl_product.Select(p => new { p.ID, p.SubCategoryID, p.CategoryID, p.Title }).Where(u => u.Title == title.Replace("-", " ")).FirstOrDefault();
            var data = context.tbl_category.Select(p => new { p.ID, p.title }).Where(u => u.title == categoryTitle.Replace("-", " ")).FirstOrDefault();
            if (data == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                ViewBag.CatId = data.ID;

                string titleNew = title.Replace("-", " ");
                SqlConnection sqlConnection = new SqlConnection(context.Database.Connection.ConnectionString);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select a.title as SubCatTitle,a.detail as subCatDetail, b.Title,b.home_image, b.Image_1 from Tbl_subCategory as a, tbl_product as b WHERE a.ID=b.SubCategoryID and a.title='" + titleNew + "' AND b.Isactive =1 AND b.Isdelete = 0", sqlConnection);
                DataSet ds1 = new DataSet();
                sqlDataAdapter.Fill(ds1);
                return View(ds1.Tables[0]);
            }
        }

        [Route("~/ProductDetail/{CategoryID}/{title}")]
        public ActionResult ProductDetail(int CategoryID,  string title)
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            // ViewBag.Message = title;
            title = title.Replace(" ", "-");
            ViewBag.Message = title.Replace("-", " ");
            ViewBag.Title = title.Replace("-", " ");
            //ViewBag.CatTitle = categoryTitle;
            //ViewBag.SubCatTitle = titleSubcat;
            var data = context.tbl_product.Select(p => new { p.ID, p.SubCategoryID, p.CategoryID, p.Title }).Where(u => (u.Title == title.Replace("-", " ")) && (u.CategoryID == CategoryID)).FirstOrDefault();

            DataSet ds1 = new DataSet();
            DataTable d1;
            DataTable d2;
            try
            {
                if (data.ID.ToString() != "")
                {
                    d1 = dblayer.GetData("SELECT a.*,b.title as subcatName FROM tbl_product as a, Tbl_subCategory as b WHERE a.SubCategoryID=b.ID and a.ID=" + data.ID);
                    d1.TableName = "Table1";
                    int subCatID = Convert.ToInt32(d1.Rows[0]["SubCategoryID"].ToString());

                    d2 = dblayer.GetData("SELECT * FROM tbl_product WHERE SubCategoryID = " + subCatID + " AND Isactive = 1 AND Isdelete = 0 order by created_at desc");
                    d2.TableName = "Table2";


                    ds1.Tables.Add(d1.Copy());
                    ds1.Tables.Add(d2.Copy());
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(ds1);


        }

        [Route("~/tafe-power")]
        public ActionResult TafePower()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "tafe-power";
            return View();
        }

        [Route("~/blog")]
        public ActionResult Blog()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Blogs.";
            var Bloglistdata = context.Tbl_blog.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            return View(Bloglistdata);
        }

        [Route("~/blog/{title}")]
        public ActionResult BlogDetail(string title)
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = title.Replace("-", " ");
            DataSet ds1 = new DataSet();
            DataTable d1;
            DataTable d2;
            try
            {
                if (title.ToString() != "")
                {
                    d1 = dblayer.GetData("SELECT * FROM Tbl_blog WHERE title= '" + ViewBag.Message + "'");
                    d1.TableName = "Table1";

                    d2 = dblayer.GetData("SELECT * FROM Tbl_blog WHERE Isactive = 1  order by created_at desc");
                    d2.TableName = "Table2";


                    ds1.Tables.Add(d1.Copy());
                    ds1.Tables.Add(d2.Copy());
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(ds1);
        }
        [Route("~/moba")]
        public ActionResult moba()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Moba.";
            return View();
        }

        [Route("~/about-us")]
        public ActionResult About()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "About.";

            var about_us = context.about_us.FirstOrDefault();
            var about_ourvision = context.about_ourvision.FirstOrDefault();
            var about_ourvalues = context.about_ourvalues.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            var about_certification = context.about_certification.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();

            Aboutus model = new Aboutus
            {
                about_usSingleRec = about_us,
                about_ourvisionSinglrRec = about_ourvision,
                ourvaluesList = about_ourvalues,
                certificationList = about_certification
            };

            return View(model);
        }
        [Route("~/terms-of-use")]
        public ActionResult TermOfUse()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "About.";
            return View();
        }
        [Route("~/privacy-policy")]
        public ActionResult PrivacyPolicy()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "About.";
            return View();
        }
        [Route("~/about-us/certifications")]
        public ActionResult Certifications()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Certifications";
            return View();
        }
        [Route("~/about-us/company-policies")]
        public ActionResult CompanyPolicy()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Company Policy";
            return View();
        }

        [Route("~/careers")]
        public ActionResult careers()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "careers";
            return View();
        }

        [Route("~/contact-us")]
        public ActionResult Contact()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Contact Us.";

            var data = context.contact_detail.FirstOrDefault();
            return View(data);
        }

        [Route("~/faq")]
        public ActionResult Faq()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "FAQ.";
            var data = context.Tbl_faq.Where(x => x.Isactive == true).ToList();
            return View(data);
        }
        [HttpPost]
      //  [ValidateInput(false)]
        [Route("~/AddProductEnq")]
        public ActionResult AddProductEnq(product_enq product_enq)
        {

            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Product Detail";
            try
            {
                var response = new { status = 0, message = "" };


                 if (!Regex.IsMatch(product_enq.contact_name, @"^[a-zA-Z ]{2,40}"))
                {
                    response = new { status = 2, message = "Please enter valid name!" };
                    return Json(response);
                }
                else if (!Regex.IsMatch(product_enq.email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{2,7}"))
                {
                    response = new { status = 2, message = "Please enter valid email address!" };
                    return Json(response);
                }

                else
                {
                    

                    product_enq.organisation_name = product_enq.organisation_name;
                    product_enq.contact_name = product_enq.contact_name;
                    product_enq.mobile = product_enq.mobile;
                    product_enq.email = product_enq.email;
                    product_enq.message_detail = product_enq.message_detail;
                    product_enq.state = product_enq.state;
                    product_enq.product_title = product_enq.product_title;
                    product_enq.product_url = product_enq.product_url;
                    product_enq.created_at = DateTime.Now;
                    context.product_enq.Add(product_enq);
                    context.SaveChanges();
                    //ViewBag.Message = "Success.";
                    // return RedirectToAction("Contact");

                    // mail -----------
                    string subject = "Enquiry Received - Ajashy " + product_enq.product_title + "";
                    string sendTo = product_enq.email;


                    string bodyTopUser = "<p><b>Dear " + product_enq.contact_name + ",</b></p><p><b>Thank you for showing interest in " + product_enq.product_title + ".</b></p><p>Your details are as captured below:<b></b><br></p>";
                    string bodyBottomUser = "<p><br><b>We are processing your request and shall get back to you soon. </b></p><p><b>Regards,</b></p><p><b>Ajashy Marketing Team </b></p>";
                    string body = "";
                    body += "<table style = 'border-collapse: collapse;'><tbody> " +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Organisation Name <th> " +
                   "<td style = 'color: #6c757d;'> " + product_enq.organisation_name + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Contact Name <th> " +
                   "<td style = 'color: #6c757d;'> " + product_enq.contact_name + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Mobile Number <th> " +
                   "<td style = 'color: #6c757d;'> " + product_enq.mobile + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Email ID <th> " +
                   "<td style = 'color: #6c757d;'> " + product_enq.email + " <td> " +
                   "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Product Name <th> " +
                   "<td style = 'color: #6c757d;'> " + product_enq.product_title + " <td> " +
                   "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Product Url <th> " +
                   "<td style = 'color: #6c757d;'> " + product_enq.product_url + " <td> " +
                   "</tr></tbody></table>";
                   // sendMailCustomeFunction(subject, bodyTopUser + body + bodyBottomUser, sendTo, "", "");
                  
                   // sendMailCustomeFunction(subject, body, sendToAdmin + "," + sendMailOtherProenq, sendtomailBcc, "");
                   
                    //   return View();
                    response = new { status = 1, message = "Success" };
                }
                return Json(response);
            }

            catch (DbEntityValidationException e)
            {


                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;

                //ViewBag.Message = ex.Message;
                var response = new { status = 0, message = e.Message };
                return Json(response);
            }
            // return View("Contact");

        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Home/AddContact")]
        public ActionResult AddContact(contact_us contact_Us, HttpPostedFileBase image_file)
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "Contact Us";
            try
            {
                var response = new { status = 0, message = "" };
                //CaptchaResponse response2 = ValidateCaptcha(Request["g-recaptcha-response"]);

                //if (response2.Success == false)
                //{
                //    response = new { status = 2, message = "Please select captcha!" };
                //    return Json(response);
                //}

               
                if (!Regex.IsMatch(contact_Us.contact_name, @"^[a-zA-Z ]{2,40}"))
                {
                    response = new { status = 2, message = "Please enter valid name!" };
                    return Json(response);
                }
                else if (!Regex.IsMatch(contact_Us.email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{2,7}"))
                {
                    response = new { status = 2, message = "Please enter valid email address!" };
                    return Json(response);
                }

                else
                {

                    if(image_file != null)
                    {
                        contact_Us.organisation_name = SaveContact_EnqFile(image_file);
                    }
                    ;



                    contact_Us.organisation_name = contact_Us.organisation_name;
                    contact_Us.contact_name = contact_Us.contact_name;
                    contact_Us.mobile = contact_Us.mobile;
                    contact_Us.email = contact_Us.email;
                    contact_Us.message_detail = contact_Us.message_detail;
                    contact_Us.created_at = DateTime.Now;
                    context.contact_us.Add(contact_Us);
                    context.SaveChanges();

                    // mail -----------
                    string subject = "Thanks for Contacting Ajashy Engineering";
                    string sendTo = contact_Us.email;

                    string bodyTopUser = "<p><b>Dear " + contact_Us.contact_name + ",</b></p><p><b>Thank you for contacting us.</b></p><p>Your details are as captured below:<b></b><br></p>";
                    string bodyBottomUser = "<p><br><b>We are processing your request and shall get back to you soon.</b></p><p><b>Regards,</b></p><p><b>Ajashy Engineering Sales Pvt. Ltd.</b></p>";

                    string body = "";
                    body += "<table style = 'border-collapse: collapse;'><tbody> " +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Organisation Name <th> " +
                   "<td style = 'color: #6c757d;'> " + contact_Us.organisation_name + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Contact Name <th> " +
                   "<td style = 'color: #6c757d;'> " + contact_Us.contact_name + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Mobile Number <th> " +
                   "<td style = 'color: #6c757d;'> " + contact_Us.mobile + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Email ID <th> " +
                   "<td style = 'color: #6c757d;'> " + contact_Us.email + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Enquiry for  <th> " +
                   "<td style = 'color: #6c757d;'> " + contact_Us.message_detail + " <td> " +
                   "</tr></tbody></table>";
                  //  sendMailCustomeFunction(subject, bodyTopUser + body + bodyBottomUser, sendTo, "", "");
                    //sendMailCustomeFunction(subject, body, sendToAdmin+",info@ajashy.com", sendtomailBcc, sendtomailCc="");
                  //  sendMailCustomeFunction(subject, body, sendToAdmin + "," + sendMailOtherContact, sendtomailBcc, "");

                    //close mail ------------


                    //   return View();
                    response = new { status = 1, message = "Success" };
                }
                return Json(response);
            }

            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;


                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }

            // return View("Contact");

        }

        public string SaveContact_EnqFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/Contact_Enq");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/Contact_Enq/" + file.FileName;
            return path1;
        }

        public static CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Home/AddCareer")]
        public ActionResult AddCareer(tbl_career tbl_career, HttpPostedFileBase attach_resume)
        {

            try
            {
                var response = new { status = 0, message = "" };
                //CaptchaResponse response2 = ValidateCaptcha(Request["g-recaptcha-response"]);

                //if (!response2.Success)
                //{
                //    response = new { status = 2, message = "Please select captcha!" };
                //    return Json(response);
                //}
                //if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))


                if (tbl_career.full_name == null || tbl_career.mobile == null || tbl_career.email == null || tbl_career.enquiry_for == null || tbl_career.attach_resume == null)
                {
                    response = new { status = 2, message = "Please fill all mandatory fields!" };
                    return Json(response);
                }
                else if (!Regex.IsMatch(tbl_career.full_name, @"^[a-zA-Z ]{2,30}"))
                {
                    response = new { status = 2, message = "Please enter valid name!" };
                    return Json(response);
                }
                else if (!Regex.IsMatch(tbl_career.email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{2,7}"))
                {
                    response = new { status = 2, message = "Please enter valid email address!" };
                    return Json(response);
                }
                else
                {

                    if (attach_resume != null)
                    {
                        tbl_career.attach_resume = SavecarresumeFile(attach_resume);
                    }
                    tbl_career.full_name = tbl_career.full_name;
                    tbl_career.mobile = tbl_career.mobile;
                    tbl_career.email = tbl_career.email;
                    tbl_career.enquiry_for = tbl_career.enquiry_for;
                    tbl_career.msg_detail = tbl_career.msg_detail;
                    tbl_career.created_at = DateTime.Now;
                    context.tbl_career.Add(tbl_career);
                    context.SaveChanges();

                    // mail -----------
                    string subject = "Career Enquiry ";
                    string sendTo = tbl_career.email;
                    string userBodyTop = "<p><b>Dear " + tbl_career.full_name + ",</b></p><p><b>Thank you for your interest in Ajashy Engineering Sales Pvt Ltd.</b></p><p><b>We will look into your profile and shall get back to you if your qualifications and experience align with any openings with us.</b></p>";
                    string userBodyBottom = "<p><b>Regards,</b></p><p><b>Human Resource Department</b></p>";


                    string body = "";
                    body += "<table style = 'border-collapse: collapse;'><tbody> " +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Name <th> " +
                   "<td style = 'color: #6c757d;'> " + tbl_career.full_name + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Contact No <th> " +
                   "<td style = 'color: #6c757d;'> " + tbl_career.mobile + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Email ID <th> " +
                   "<td style = 'color: #6c757d;'> " + tbl_career.email + " <td> " +
                   "</tr>" +
                   "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                   "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Applying For <th> " +
                   "<td style = 'color: #6c757d;'>" + tbl_career.enquiry_for + "<td> " +
                   "</tr>";
                    if (tbl_career.attach_resume != null)
                    {
                        body += "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                        "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Upload File<th> " +
                        "<td style = 'color: #6c757d;'> <a href='" + domainName + tbl_career.attach_resume + "' target='_blank'>Upload File</a> <td> " +
                        "</tr>";
                    }

                    body += "</tbody></table>";

                    //if (tbl_career.attach_resume != null) { body += "<p><b>Upload File : </b><a href='~/" + tbl_career.attach_resume + "' target='_blank'>Upload File</a></p>"; }

                    sendMailCustomeFunction(subject, body, sendMailToCareer, "", sendtomailCc);

                    //sendMailCustomeFunction(subject, userBodyTop + userBodyBottom, "s.jain@ajashy.com", sendtomailBcc = "", sendtomailCc);
                    sendMailCustomeFunction(subject, userBodyTop + userBodyBottom, sendTo, "", "");
                    //close mail ------------

                    response = new { status = 1, message = "Success" };
                    return Json(response);
                }

            }

            catch (Exception ex)
            {

                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }

        }


        public ActionResult faq()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "faq.";
            return View();
        }
        [Route("~/testimonial")]
        public ActionResult testimonials()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "testimonials.";
            var data = context.tbl_testimonial.Where(x => x.Isactive == true).OrderByDescending(x => x.created_at).ToList();
            return View(data);
        }

        [Route("~/after-sales-support")]
        public ActionResult AfterSalesSupport()
        {
            DataSet ds = dblayer.get_Category();
            ViewBag.Category = ds.Tables[0];
            ViewBag.Message = "after sales support.";

            var data = context.Tbl_saleSupportBannes.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            return View(data);
            //return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Home/AddAfterSalesSupport")]
        public ActionResult AddAfterSalesSupport(sale_support sale_Support, HttpPostedFileBase[] image_file)
        {
            try
            {

                var response = new { status = 0, message = "" };
                //CaptchaResponse response2 = ValidateCaptcha(Request["g-recaptcha-response"]);

                //if (!response2.Success)
                //{
                //    response = new { status = 2, message = "Please select captcha!" };
                //    return Json(response);
                //}

                if (sale_Support.equipment_description == null || sale_Support.equipment_make == null || sale_Support.email == null || sale_Support.urgency_level == null || sale_Support.meter_radio == null || sale_Support.mobile == null || sale_Support.problem_type == null)
                {
                    response = new { status = 2, message = "Please fill all mandatory fields!" };
                }
                else if (!Regex.IsMatch(sale_Support.meter_number, @"^[0-9]{2,40}"))
                {
                    response = new { status = 2, message = "Please enter valid meter reading!" };
                    return Json(response);
                }
                else if (!Regex.IsMatch(sale_Support.contact_person, @"^[a-zA-Z ]{2,40}"))
                {
                    response = new { status = 2, message = "Please enter contact person name!" };
                    return Json(response);
                }
                else if (!Regex.IsMatch(sale_Support.email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{2,7}"))
                {
                    response = new { status = 2, message = "Please enter valid email address!" };
                    return Json(response);
                }
                else
                {
                    string path1Images = "";
                    if (image_file != null)
                    {
                        //sale_Support.attachment = SavePolicyFile(image_file);

                        foreach (HttpPostedFileBase file in image_file)
                        {
                            //Checking file is available to save.  
                            if (file != null)
                            {

                                Random r = new Random();
                                int nextRandom = r.Next(10000);
                                // as per the updated question, you want at least two digits
                                int nextRandomTwoDigits = r.Next(999) + 10;
                                string unqidd = Guid.NewGuid().ToString();
                                unqidd = unqidd.Substring(unqidd.Length - 2);
                                string DtTime = DateTime.Now.ToString("ddss") + nextRandomTwoDigits.ToString() + unqidd.ToString();
                                var InputFileName = Path.GetFileName(file.FileName).Replace(" ", "-");

                                if (InputFileName.Length > 15)
                                {
                                    InputFileName = InputFileName.Substring(InputFileName.Length - 15);
                                }

                                var ServerSavePath = Path.Combine(Server.MapPath("~/assets/SalesEnq/") + DtTime + InputFileName);
                                //Save file to server folder  
                                file.SaveAs(ServerSavePath);
                                path1Images += "assets/SalesEnq/" + DtTime + InputFileName + ",";

                            }

                        }
                    }


                    path1Images = path1Images.TrimEnd(',');

                    sale_Support.equipment_description = sale_Support.equipment_description;
                    sale_Support.equipment_model = sale_Support.equipment_model;
                    sale_Support.equipment_make = sale_Support.equipment_make;
                    sale_Support.status = sale_Support.status;
                    sale_Support.serial_no = sale_Support.serial_no;
                    sale_Support.address = sale_Support.address;
                    sale_Support.customer_company = sale_Support.customer_company;
                    sale_Support.mobile = sale_Support.mobile;
                    sale_Support.contact_person = sale_Support.contact_person;
                    sale_Support.nature_problem = sale_Support.nature_problem;
                    sale_Support.email = sale_Support.email;
                    sale_Support.urgency_level = sale_Support.urgency_level;
                    sale_Support.attachment = path1Images;
                    sale_Support.created_at = DateTime.Now;
                    sale_Support.meter_radio = sale_Support.meter_radio;
                    sale_Support.meter_number = sale_Support.meter_number;
                    sale_Support.position = sale_Support.position;
                    sale_Support.problem_type = sale_Support.problem_type;
                    sale_Support.complaint_number = sale_Support.mobile;
                    context.sale_support.Add(sale_Support);
                    var abxb = context.SaveChanges();

                    int ticketno = sale_Support.complaint_id;
                    string addzerodynamin = "";
                    if (ticketno.ToString().Length == 1)
                    {
                        addzerodynamin = "00000";
                    }
                    else if (ticketno.ToString().Length == 2)
                    {
                        addzerodynamin = "0000";
                    }
                    else if (ticketno.ToString().Length == 3)
                    {
                        addzerodynamin = "000";
                    }
                    else if (ticketno.ToString().Length == 4)
                    {
                        addzerodynamin = "00";
                    }
                    else if (ticketno.ToString().Length == 5)
                    {
                        addzerodynamin = "0";
                    }
                    else if (ticketno.ToString().Length > 5)
                    {
                        addzerodynamin = "";
                    }

                    string ticketNumber = DateTime.Now.Year + addzerodynamin + sale_Support.complaint_id;
                    var data = context.sale_support.FirstOrDefault(u => u.complaint_id == sale_Support.complaint_id);

                    data.ticket_number = ticketNumber.ToString();
                    context.SaveChanges();

                    // mail -----------
                    string subject = "New Service Support Ticket Raised- Ajashy Engineering Sales";
                    string sendTo = sale_Support.email;


                    string subjectAdmin = "After Service Support ticket:";
                    string admBodyTop = "<p><b>New Service support ticket is generated with the below details.</b></p><p><b></b>Ticket Number - " + ticketNumber + "</p>";
                    string userBodyTop = "<p><b>Dear " + sale_Support.contact_person + ",</b></p><p><b>Thanks for contacting us!</b></p><p>Your service support Ticket is generated with Ticket Number - " + ticketNumber + ".<b></b></p><p><b>Your details are as captured below:</b><br></p>";
                    string userBodyBottom = "<p><b>We shall get back to you shortly.</b></p><p><b>You can follow up on your tickets by calling us or on Whatsapp at <a href='+91 9311666632'></a> +91 9311666632 or writing to us at <a href='mailto:service.support@ajashy.com'>service.support@ajashy.com</a></b></p>";
                    string body = "";
                    body += "<table style = 'border-collapse: collapse;'><tbody> " +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Equipment Description <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.equipment_description + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Equipment Model <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.equipment_model + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Equipment Make <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.equipment_make + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Meter <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.meter_radio + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Meter Number <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.meter_number + " <td> " +
                    "</tr>" +

                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Serial No <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.serial_no + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Site Address <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.address + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Customer Company <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.customer_company + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Contact Number <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.complaint_number + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Contact Person <th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.contact_person + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Nature of Problem<th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.nature_problem + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Email ID<th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.email + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Action Required<th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.position + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'> " +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Equipment Status<th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.problem_type + " <td> " +
                    "</tr>" +
                    "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                    "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Urgency Level<th> " +
                    "<td style = 'color: #6c757d;'> " + sale_Support.urgency_level + " <td> " +
                    "</tr>";

                    if (sale_Support.attachment != null)
                    {
                        var arrImgPath = path1Images.Split(',');
                        foreach (var file in arrImgPath)
                        {
                            //Checking file is available to save.  
                            if (file.Length > 5)
                            {

                                string filpath = domainName + file;
                                body += "<tr style = 'text-align: left; border: 1px solid #6c757d;'>" +
                              "<th style = 'border: 1px solid #6c757d;text-wrap: nowrap;padding: 8px;'> Upload File<th> " +
                              "<td style = 'color: #6c757d;'> <a href='" + filpath + "' target='_blank'>Upload File</a> <td> " +
                              "</tr>";
                            }
                        }

                    }

                    body += "</tbody></table><br><br>";
                    sendMailCustomeFunction(subject, userBodyTop + body + userBodyBottom, sendTo, "", "");
                    //sendMailCustomeFunction(subjectAdmin, admBodyTop+body, sendToAdmin+",service.support@ajashy.com", sendtomailBcc, sendtomailCc);
                    sendMailCustomeFunction(subjectAdmin, admBodyTop + body, sendToAdmin + "," + sendOtherMailServ, sendtomailBcc, sendtomailCc);
                    //close mail ------------

                    response = new { status = 1, message = "Success" };
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }

        }
        //[Route("~/dealerships")]
        //public ActionResult Dealerships()
        //{
        //    DataSet ds = dblayer.get_Category();
        //    ViewBag.Category = ds.Tables[0];
        //    ViewBag.Message = "Dealerships";
        //    return View();
        //}
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Home/Subscribe")]
        public ActionResult Subscribe(string Email)
        {
            try
            {
                var response = new { status = 0, message = "" };
                if (Email != "")
                {
                    var data = context.tbl_subscribe.Where(u => u.Emailid == Email).FirstOrDefault();

                    if (data != null)
                    {
                        response = new { status = 2, message = "You are already subscribed!" };
                        return Json(response);
                    }
                    else if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{2,7}"))
                    {
                        response = new { status = 2, message = "Invalid email address!" };
                        return Json(response);
                    }
                    else
                    {
                        tbl_subscribe obj = new tbl_subscribe();
                        obj.Emailid = Email;
                        obj.CreatedAt = DateTime.Now;
                        context.tbl_subscribe.Add(obj);
                        context.SaveChanges();

                        // mail -----------
                        string subject = "Subscribe Mail ";
                        string sendTo = Email;
                        string body = "";

                        body += "<div class='container' style='background-color:#f8f9fa;width: 550px;padding: 20px; margin: 0 auto;'> " +
                       "<h3 style = 'margin: 20px 0;'> Email Newsletters:</h3> " +
                       "<p> Thank you for subscribing to our blogs.You can expect to learn more about our existing and new products, updates" +
                       "about our services and details about the ever changing technology and our industry.</p>" +
                       "<p> We will also look forward to your inputs about what you would like to read in our next blogs and to your valuable feedback.</p>" +
                       "<p> Happy Reading!! </p>" +
                       "</div> ";

                        sendMailCustomeFunction(subject, body, sendToAdmin, "", "");
                        sendMailCustomeFunction(subject, body, sendTo, "", "");
                        //close mail ------------
                        response = new { status = 1, message = "Thanks for subscribing!" };
                        return Json(response);
                    }
                }
                else
                {
                    response = new { status = 0, message = "Please enter mail id!" };
                    return Json(response);
                }

                //  return RedirectToAction("Index");
            }

            catch (Exception ex)
            {

                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }

        }

        [Route("~/E-Catalogue")]
        public ActionResult ECatalogue()
        {
            //DataSet ds = dblayer.get_Category();
            //ViewBag.Category = ds.Tables[0];
            //ViewBag.Message = "testimonials.";
            //var data = context.tbl_testimonial.Where(x => x.Isactive == true).OrderByDescending(x => x.created_at).ToList();
            // return View(data);

            return View();
        }

        [Route("~/our-product/{CategoryId}")]
        public ActionResult ourproduct(int CategoryId)
        {

            
            
            var data = context.tbl_Menu.Select(p => new { p.ID, p.title }).Where(u => u.ID == CategoryId).FirstOrDefault();
            ViewBag.CatId = data.ID;
          
            ViewBag.Title = data.title;
            


            getAllCategory(CategoryId);

            

                return View();
        }
        public string SavePolicyFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/SalesEnq");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/SalesEnq/" + file.FileName;
            return path1;
        }

        public string SavecarresumeFile(HttpPostedFileBase file)
        {
            //string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/carresume");
            //string filePath = directory + "\\" + file.FileName;
            string filePath = "";
            Random r = new Random();
            int nextRandom = r.Next(10000);
            // as per the updated question, you want at least two digits
            int nextRandomTwoDigits = r.Next(999) + 10;
            string unqidd = Guid.NewGuid().ToString();
            unqidd = unqidd.Substring(unqidd.Length - 2);
            string DtTime = DateTime.Now.ToString("ddss") + nextRandomTwoDigits.ToString() + unqidd.ToString();
            var InputFileName = Path.GetFileName(file.FileName).Replace(" ", "-");

            if (InputFileName.Length > 15)
            {
                InputFileName = InputFileName.Substring(InputFileName.Length - 15);
            }

            var ServerSavePath = Path.Combine(Server.MapPath("~/assets/carresume/") + DtTime + InputFileName);
            //Save file to server folder  
            file.SaveAs(ServerSavePath);
            string path1 = "assets/carresume/" + DtTime + InputFileName;
            return path1;
        }



        public string sendMailCustomeFunction(string subjectstr, string bodystr, string sendtomail, string sendtomailBcc, string sendtomailCc)
        {
            MailMessage msg = new MailMessage();
            //msg.From = new MailAddress("sales@vcqru.com", "VCQRU");
            msg.From = new MailAddress("itdepartment@ajashy.com", "Ajashy");
            msg.To.Add(sendtomail);
            if (sendtomailBcc.Length > 5)
            {
                msg.Bcc.Add(sendtomailBcc);
            }
            if (sendtomailCc.Length > 5)
            {
                msg.Bcc.Add(sendtomailCc);
            }
            msg.IsBodyHtml = true;
            msg.Subject = subjectstr;
            msg.Body = bodystr;
            SmtpClient sc = new SmtpClient();

            sc.Host = "smtp.gmail.com";
            sc.EnableSsl = true; // changed for vcqru mail domain
            sc.UseDefaultCredentials = false;// changed for vcqru mail domain
            sc.Credentials = new NetworkCredential("sales@vcqru.com", "iaqesbfqqugepseh");
            sc.Port = 587;// changed for vcqru mail domain
            sc.DeliveryMethod = SmtpDeliveryMethod.Network;
            sc.Send(msg);
            return "";
        }


    }
}