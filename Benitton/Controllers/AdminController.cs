
using Benitton.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benitton.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        
        Benitton_dbEntities context = new Benitton_dbEntities();

      
        DatabaseCon DbCon = new DatabaseCon();

        //public object HtmlUtility { get; private set; }




        // GET: Admin
        public ActionResult Index()
        {
            if (Session["loginType"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var countCat = context.tbl_category.Where(o => o.Isactive == true).Count();
            var CountSubCat = context.Tbl_subCategory.Where(o => o.Isactive == true).Count();
            var CountProduct = context.tbl_product.Where(o => o.Isactive == true).Count();
            var CountSaleSupport = context.sale_support.Count();
            var CountContact = context.contact_us.Count();
            var Countsubscribe = context.product_enq.Count();
            ViewBag.countCat = countCat;
            ViewBag.CountSubCat = CountSubCat;
            ViewBag.CountProduct = CountProduct;
            ViewBag.CountSaleSupport = CountSaleSupport;
            ViewBag.CountContact = CountContact;
            ViewBag.Countsubscribe = Countsubscribe;
            var data = context.contact_us.OrderByDescending(x => x.contact_id).ToList();
            return View(data);

        }


        [Route("~/Admin/Category/1/1")]
        public ActionResult Category()
        {
            //var data = context.tbl_category.Where(x=> x.Isactive == true).OrderByDescending(x => x.created_at).ToList();
            var data = context.tbl_category.OrderByDescending(x => x.created_at).ToList();
            return View(data);
        }

        [Route("~/Admin/AddCategory/1/1")]
        public ActionResult AddCategory()
        {
            //var data = context.Tbl_policy.ToList();
            return View();
        }


        [HttpPost]
       // [ValidateInput(false)]
        [Route("~/Admin/AddCategory/1/1")]
        public ActionResult AddCategory(string title, tbl_category tbl_category, HttpPostedFileBase image_file)
        {
            try
            {
                var data = context.tbl_category.Where(u => u.title == tbl_category.title.Trim()).FirstOrDefault();
                if (data != null)
                {
                    var errorresponse = new { status = 2, message = "Category should be unique!" };
                    return Json(errorresponse);
                }

                if (image_file != null)
                {
                    tbl_category.image_file = SavePolicyFile(image_file);
                }

                tbl_category.created_at = DateTime.Now;
                tbl_category.Isactive = true;
               
                context.tbl_category.Add(tbl_category);
                context.SaveChanges();
              //  RedirectToAction("Category");
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        [Route("~/Admin/UpdateCategory/{ID}/1/1")]
        public ActionResult UpdateCategory(int ID)
        {

            var data = context.tbl_category.Where(u => u.ID == ID).FirstOrDefault();
            return View(data);
            
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/UpdateCategory/1/1")]
        public ActionResult UpdateCategory(tbl_category tbl_category2, int Pid, HttpPostedFileBase image_file)
        {
            try
            {
                var data1 = context.tbl_category.Where(u => u.title == tbl_category2.title.Trim() && u.ID != Pid).FirstOrDefault();
                var data = context.tbl_category.FirstOrDefault(u => u.ID == Pid);
                if (data1 != null)
                {
                    var errorresponse = new { status = 2, message = "Category should be unique!" };
                    return Json(errorresponse);
                }

                if (image_file != null)
                {
                    data.image_file = SavePolicyFile(image_file);
                }
                data.title = tbl_category2.title;

                data.detail = tbl_category2.detail;
                //tbl_category.created_at = DateTime.Now;
                // tbl_category.Isactive = true;
                //context.tbl_category.Add(data);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [Route("~/Admin/SubCategory/1/1")]
        public ActionResult SubCategory()
        {

            var catdata = context.tbl_category.Where(x => x.Isactive == true).ToList();
            var subcatdata = context.Tbl_subCategory.OrderBy(x => x.serialorder).Take(10).ToList();
            //var productdata = context.tbl_product.ToList();

            ViewProducts model = new ViewProducts
            {
                categoryMd = catdata,
                subcategoryMd = subcatdata,
            };
            return View(model);
            //return View(data);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/SubCategory/1/1")]
        public ActionResult SubCategory(int CategoryID)
        {

            var catdata = context.tbl_category.Where(x => x.Isactive == true).ToList();
            var subcatdata = context.Tbl_subCategory.Where(x => x.CategoryID == CategoryID).OrderBy(x => x.serialorder).ToList();

            //var productdata = context.tbl_product.ToList();

            ViewProducts model = new ViewProducts
            {
                categoryMd = catdata,
                subcategoryMd = subcatdata,
            };
            return View(model);
        }

        [Route("~/Admin/UpdateSubcategory/{ID}/1/1")]
        public ActionResult UpdateSubcategory(int ID)
        {
            ViewProducts model = new ViewProducts();
            var datauserList = context.tbl_category.Where(u => u.Isactive == true).ToList();
            model.categoryMd = datauserList;
            var dataProduct = context.Tbl_subCategory.Where(u => u.ID == ID).FirstOrDefault();
            model.tbl_singleSubCategory = dataProduct;

            return View(model);
        }

        [Route("~/Admin/blog/1/1")]
        public ActionResult blog()
        {
            //var data = context.tbl_category.Where(x=> x.Isactive == true).OrderByDescending(x => x.created_at).ToList();
            var data = context.Tbl_blog.OrderByDescending(x => x.created_at).ToList();
            return View(data);
        }
        [Route("~/Admin/faq/1/1")]
        public ActionResult faq()
        {
            //var data = context.tbl_category.Where(x=> x.Isactive == true).OrderByDescending(x => x.created_at).ToList();
            var data = context.Tbl_faq.OrderByDescending(x => x.created_at).ToList();
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/Addfaq/1/1")]
        public ActionResult Addfaq(Tbl_faq Tbl_faq)
        {
            try
            {
                var data = context.Tbl_faq.Where(u => u.title == Tbl_faq.title.Trim()).FirstOrDefault();
                if (data != null)
                {
                    var errorresponse = new { status = 2, message = "faq should be unique!" };
                    return Json(errorresponse);
                }

                Tbl_faq.created_at = DateTime.Now;
                Tbl_faq.Isactive = true;
                context.Tbl_faq.Add(Tbl_faq);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [Route("~/Admin/Updatefaq/{ID}/1/1")]
        public ActionResult Updatefaq(int ID)
        {
            var data = context.Tbl_faq.Where(u => u.ID == ID).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/Updatefaq/1/1")]
        public ActionResult Updatefaq(Tbl_faq Tbl_faq2, int Pid)
        {
            try
            {
                var data1 = context.Tbl_faq.Where(u => u.title == Tbl_faq2.title.Trim() && u.ID != Pid).FirstOrDefault();
                var data = context.Tbl_faq.FirstOrDefault(u => u.ID == Pid);
                if (data1 != null)
                {
                    var errorresponse = new { status = 2, message = "Blog should be unique!" };
                    return Json(errorresponse);
                }
                data.title = Tbl_faq2.title;

                data.detail = Tbl_faq2.detail;

                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {

                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/Addblog/1/1")]
        public ActionResult Addblog(Tbl_blog Tbl_blog, HttpPostedFileBase image_file)
        {
            try
            {
                var data = context.Tbl_blog.Where(u => u.title == Tbl_blog.title.Trim()).FirstOrDefault();
                if (data != null)
                {
                    var errorresponse = new { status = 2, message = "blog should be unique!" };
                    return Json(errorresponse);
                }

                if (image_file != null)
                {
                    Tbl_blog.image_file = SaveBlogFile(image_file);
                }

                Tbl_blog.created_at = DateTime.Now;
                Tbl_blog.Isactive = true;
                Tbl_blog.display_on_mainpage = true;
                context.Tbl_blog.Add(Tbl_blog);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [Route("~/Admin/Updateblog/{ID}/1/1")]
        public ActionResult Updateblog(int ID)
        {

            var dataBlog = context.Tbl_blog.Where(u => u.ID == ID).FirstOrDefault();
            return View(dataBlog);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/Updateblog/1/1")]
        public ActionResult Updateblog(Tbl_blog Tbl_blog2, int Pid, HttpPostedFileBase image_file)
        {
            try
            {
                var data1 = context.Tbl_blog.Where(u => u.title == Tbl_blog2.title.Trim() && u.ID != Pid).FirstOrDefault();
                var data = context.Tbl_blog.FirstOrDefault(u => u.ID == Pid);
                if (data1 != null)
                {
                    var errorresponse = new { status = 2, message = "Blog should be unique!" };
                    return Json(errorresponse);
                }

                if (image_file != null)
                {
                    data.image_file = SaveBlogFile(image_file);
                }

                data.title = Tbl_blog2.title;

                data.detail = Tbl_blog2.detail;

                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {

                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }



        [Route("~/Admin/Products/1/1")]
        public ActionResult Products()
        {
            ViewBag.submitForm = "";
            ViewBag.catid = "";
            var catdata = context.tbl_category.Where(x => x.Isactive == true).ToList();
            var subcatdata = context.Tbl_subCategory.Where(x => x.Isactive == true).ToList();
            var subcatProductdata = context.tbl_product.Where(x => x.Isactive == true && x.Isdelete == false).OrderBy(x => x.serialorder).Take(10).ToList();

            ViewProducts model = new ViewProducts
            {
                categoryMd = catdata,
                subcategoryMd = subcatdata,
                ProductMd = subcatProductdata

            };
            return View(model);

        }


        [HttpPost]
         [ValidateInput(false)]
        [Route("~/Admin/Products/1/1")]
        public ActionResult Products(tbl_product tbl_product, HttpPostedFileBase pdf_image, HttpPostedFileBase Image_1, HttpPostedFileBase Image_2, HttpPostedFileBase Image_3, HttpPostedFileBase Image_4, HttpPostedFileBase Image_5, HttpPostedFileBase Image_6, HttpPostedFileBase Image_7, HttpPostedFileBase Image_8)
        {
            try
            {
                var data = context.tbl_product.Where(u => u.Title == tbl_product.Title.Trim()).FirstOrDefault();
                if (data != null)
                {
                    var errorresponse = new { status = 2, message = "Product should be unique!" };
                    return Json(errorresponse);
                }


                if (pdf_image != null)
                {
                    tbl_product.pdf_image = SavePdfFile(pdf_image);
                }

                if (Image_1 != null)
                {
                    tbl_product.Image_1 = SaveProductFile(Image_1);
                }
                if (Image_2 != null)
                {
                    tbl_product.Image_2 = SaveProductFile(Image_2);
                }
                if (Image_3 != null)
                {
                    tbl_product.Image_3 = SaveProductFile(Image_3);
                }
                if (Image_4 != null)
                {
                    tbl_product.Image_4 = SaveProductFile(Image_4);
                }
                if (Image_5 != null)
                {
                    tbl_product.Image_5 = SaveProductFile(Image_5);
                }
                if (Image_6 != null)
                {
                    tbl_product.Image_6 = SaveProductFile(Image_6);
                }
                if (Image_7 != null)
                {
                    tbl_product.Image_7 = SaveProductFile(Image_7);
                }
                if (Image_8 != null)
                {
                    tbl_product.Image_8 = SaveProductFile(Image_8);
                }


                //string decodedString = HttpUtility.HtmlDecode(tbl_product.Detail.ToString());
                // Clean HTML
                //string sanitizedHtmlText = HtmlUtility.SanitizeHtml(decodedString.ToString());

                // string encoded = HttpUtility.HtmlEncode(decodedString);

                tbl_product.Qty = 1;
                tbl_product.Price = 0;
                tbl_product.created_at = DateTime.Now;
                tbl_product.Isactive = true;
                tbl_product.display_on_home = false;
                context.tbl_product.Add(tbl_product);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
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

                var response = new { status = 0, message = e.Message };
                return Json(response);
            }
        }


        //[HttpPost]
        //[Route("~/Admin/Products/1/1")]
        //public ActionResult Products(int subCategoryID, int otherCatID)
        //{
        //    ViewBag.catid = subCategoryID;
        //    var subcatdata = context.Tbl_subCategory.Where(x => x.Isactive == true).ToList();
        //    var subcatProductdata = context.tbl_product.Where(x => x.SubCategoryID == subCategoryID && x.Isdelete == false).OrderBy(x => x.serialorder).ToList();

        //    if (otherCatID != 0)
        //    {
        //        subcatProductdata = context.tbl_product.Where(x => x.SubCategoryID == subCategoryID && x.Isdelete == false && x.otherCatID == otherCatID).OrderBy(x => x.serialorder).ToList();
        //    }

        //    ViewProducts model = new ViewProducts
        //    {
        //        subcategoryMd = subcatdata,
        //        ProductMd = subcatProductdata

        //    };
        //    return View(model);
        //}

        [Route("~/Admin/UpdateProduct/{ID}/1/1")]
        public ActionResult UpdateProduct(int ID)
        {
            ViewProducts model = new ViewProducts();

            var datauserList = context.tbl_category.Where(u => u.Isactive == true).ToList();
            model.categoryMd = datauserList;

            var dataSubcatList = context.Tbl_subCategory.Where(u => u.Isactive == true).ToList();
            model.subcategoryMd = dataSubcatList;

            var dataProduct = context.tbl_product.Where(u => u.ID == ID).FirstOrDefault();
            model.tbl_singleProduct = dataProduct;

            return View(model);

        }

        [Route("~/Admin/Menu/1/1")]
        public ActionResult Menu()
        {
            ViewBag.submitForm = "";
            var data = context.tbl_Menu.OrderByDescending(x => x.created_at).ToList();
            return View(data);
        }


        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/AddMenu/1/1")]
        public ActionResult AddMenuDetails(tbl_Menu tbl_Menu, HttpPostedFileBase image_file)
        {
            
            
            try
            {
                
                    if (image_file != null)
                    {
                        tbl_Menu.image_file = SaveProductFile(image_file);
                    }

                    tbl_Menu.title = tbl_Menu.title;
                    tbl_Menu.MenuOrder = tbl_Menu.MenuOrder;
                    tbl_Menu.Isactive = true;
                tbl_Menu.MenuOrder = 0;
                tbl_Menu.created_at = System.DateTime.Now;
                context.tbl_Menu.Add(tbl_Menu);
                context.SaveChanges();
                    // return RedirectToAction("BankDetail");
                    var successresponse = new { status = 1, message = "Success" };
                    return Json(successresponse);
               

            }
            catch (Exception Ex)
            {
                var response = new { status = 0, message = Ex.Message };
                return Json(response);
            }
        }

        [Route("~/Admin/UpdateMenu/{ID}/1/1")]
        public ActionResult UpdateMenu(int ID)
        {
            ViewProducts model = new ViewProducts();

            model.tbl_singleMenu = context.tbl_Menu.Where(u => u.ID == ID).FirstOrDefault();
           

            return View(model);
        }

        [HttpPost]
       // [ValidateInput(false)]
        [Route("~/Admin/UpdatemenuItem/1/1")]
        public ActionResult UpdatemenuItem(tbl_Menu tbl_Menu,int Pid, HttpPostedFileBase image_file)
        {
            try
            {
                var data = context.tbl_Menu.Where(u => u.ID == Pid).FirstOrDefault();
                
               

                if (image_file != null)
                {
                    data.image_file = SavePolicyFile(image_file);
                }

                data.title = tbl_Menu.title;
                


                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {

                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }


        [Route("~/Admin/AddMenu/1/1")]
        public ActionResult AddMenu()
        {
            ViewBag.Message = "";
            return View();
        }

        [Route("~/Admin/UserProfile/1/1")]
        public ActionResult UserProfile()
        {

            if (Session["Userid"].ToString() != "")
            {
                Redirect("~/Login");
            }
            var userid = (int)Session["Userid"];
            var data = context.tbl_admin.Where(u => u.id == userid).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/UserProfile/1/1")]
        public ActionResult UserProfile(tbl_admin tbl_admin1, int prid)
        {
            var userid = (int)Session["Userid"];
            var data = context.tbl_admin.FirstOrDefault(u => u.id == userid);
            try
            {
                if (data != null)
                {
                    // Tbl_bankdetail.created_at = DateTime.Now;
                    //data.id = userid;
                    data.name = tbl_admin1.name;
                    data.emailid = tbl_admin1.emailid;
                    data.mobile_no = tbl_admin1.mobile_no;
                    //data.Isactive = true;
                    context.SaveChanges();
                    // return RedirectToAction("BankDetail");
                    var successresponse = new { status = 1, message = "Success" };
                    return Json(successresponse);
                }
                else
                {
                    var successresponse = new { status = 2, message = "Please login again!" };
                    return Json(successresponse);
                }

            }
            catch (Exception Ex)
            {
                var response = new { status = 0, message = Ex.Message };
                return Json(response);
            }

        }
        [Route("~/Admin/contactusEnquiry/1/1")]
        public ActionResult contactusEnquiry()
        {
            var data = context.contact_us.OrderByDescending(x => x.created_at).ToList();
            return View(data);
        }

        [Route("~/Admin/ProductEnquiry/1/1")]
        public ActionResult ProductEnquiry()
        {
            var data = context.product_enq.OrderByDescending(x => x.created_at).ToList();
            return View(data);
        }

        public ActionResult Users()
        {
            //var userid = (int)Session["Userid"];
            //var data = context.tbl_admin.Where(u => u.id == userid).FirstOrDefault();
            return View();
        }
        [Route("~/Admin/ServiceSupport/1/1")]
        public ActionResult ServiceSupport()
        {
            var data = context.sale_support.OrderByDescending(x => x.created_at).ToList();
            return View(data);
        }
        [Route("~/Admin/Subscribe/1/1")]
        public ActionResult Subscribe()
        {
            var data = context.tbl_subscribe.OrderByDescending(x => x.CreatedAt).ToList();
            return View(data);
        }
        [Route("~/Admin/ChangePassword/1/1")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("~/Admin/ChangePassword/1/1")]
        public ActionResult ChangePassword(string opwd, string npwd, string conpass)
        {
            var userid = (int)Session["Userid"];

            var data = context.tbl_admin.Where(u => u.id == userid && u.password == opwd).FirstOrDefault();

            if (data != null)
            {
                if (npwd != conpass)
                {
                    var successresponse = new { status = 2, message = "Old password and new password not matched!" };
                    return Json(successresponse);
                }
                else
                {
                    data.password = npwd;
                    context.SaveChanges();

                    var successresponse = new { status = 1, message = "Success" };
                    return Json(successresponse);
                }

            }

            var response = new { status = 2, message = "Old password not matched!" };
            return Json(response);
        }
        [Route("~/Admin/AddSubCategory/1/1")]
        public ActionResult AddSubCategory()
        {
            var data = context.tbl_category.Where(u => u.Isactive == true).ToList();
            return View(data);
        }


        public ActionResult AddProduct()
        {
            var data = context.tbl_category.Where(u => u.Isactive == true).ToList();
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/AddSubCategory/1/1")]
        public ActionResult AddSubCategory(Tbl_subCategory Tbl_subCategory, HttpPostedFileBase image_file)
        {
            try
            {
                var data = context.Tbl_subCategory.Where(u => u.title == Tbl_subCategory.title.Trim()).FirstOrDefault();
                if (data != null)
                {
                    var errorresponse = new { status = 2, message = "Sub Category should be unique!" };
                    return Json(errorresponse);
                }


                if (image_file != null)
                {
                    Tbl_subCategory.image_file = SaveSubCatFile(image_file);
                }
                Tbl_subCategory.display_on_home = true;
                Tbl_subCategory.created_at = DateTime.Now;
                Tbl_subCategory.Isactive = true;
                context.Tbl_subCategory.Add(Tbl_subCategory);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/UpdateSubCategory/1/1")]
        public ActionResult UpdateSubCategory(Tbl_subCategory Tbl_subCategory2, int Pid, Tbl_subCategory Tbl_subCategory, HttpPostedFileBase image_file)
        {
            try
            {
                var data1 = context.Tbl_subCategory.Where(u => u.title == Tbl_subCategory2.title.Trim() && u.ID != Pid).FirstOrDefault();
                var data = context.Tbl_subCategory.FirstOrDefault(u => u.ID == Pid);
                if (data1 != null)
                {
                    var errorresponse = new { status = 2, message = "Sub Category should be unique!" };
                    return Json(errorresponse);
                }

                if (image_file != null)
                {
                    data.image_file = SavePolicyFile(image_file);
                }

                data.title = Tbl_subCategory2.title;
                data.CategoryID = Tbl_subCategory2.CategoryID;
                data.detail = Tbl_subCategory2.detail;

                //data.display_on_home = true;
                // context.Tbl_subCategory.Add(Tbl_subCategory2);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {

                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/UpdateProduct/1/1/1")]
        public ActionResult UpdateProduct(tbl_product tbl_product2, int Pid, HttpPostedFileBase pdf_image, HttpPostedFileBase home_image, HttpPostedFileBase Image_1, HttpPostedFileBase Image_2, HttpPostedFileBase Image_3, HttpPostedFileBase Image_4, HttpPostedFileBase Image_5, HttpPostedFileBase Image_6, HttpPostedFileBase Image_7, HttpPostedFileBase Image_8)
        {
            try
            {
                var data1 = context.tbl_product.Where(u => u.Title == tbl_product2.Title.Trim() && u.ID != Pid).FirstOrDefault();
                var data = context.tbl_product.FirstOrDefault(u => u.ID == Pid);
                if (data1 != null)
                {
                    var errorresponse = new { status = 2, message = "Product should be unique!" };
                    return Json(errorresponse);
                }
                if (pdf_image != null)
                {
                    data.pdf_image = SavePdfFile(pdf_image);
                }

                if (home_image != null)
                {
                    data.home_image = SaveProductFile(home_image);
                }

                if (Image_1 != null)
                {
                    data.Image_1 = SaveProductFile(Image_1);
                }

                if (Image_2 != null)
                {
                    data.Image_2 = SaveProductFile(Image_2);
                }
                if (Image_3 != null)
                {
                    data.Image_3 = SaveProductFile(Image_3);
                }
                if (Image_4 != null)
                {
                    data.Image_4 = SaveProductFile(Image_4);
                }
                if (Image_5 != null)
                {
                    data.Image_5 = SaveProductFile(Image_5);
                }
                if (Image_6 != null)
                {
                    data.Image_6 = SaveProductFile(Image_6);
                }
                if (Image_7 != null)
                {
                    data.Image_7 = SaveProductFile(Image_7);
                }
                if (Image_8 != null)
                {
                    data.Image_8 = SaveProductFile(Image_8);
                }

                data.Title = tbl_product2.Title;
                data.CategoryID = tbl_product2.CategoryID;
                data.SubCategoryID = tbl_product2.SubCategoryID;
                data.video_iframe = tbl_product2.video_iframe;
                data.Detail = tbl_product2.Detail;
                data.optional_ddetail = tbl_product2.optional_ddetail;
                data.salient_features = tbl_product2.salient_features;
                data.specifications = tbl_product2.specifications;
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {

                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        [Route("~/Admin/AboutOurvalues/1/1")]
        public ActionResult AboutOurvalues()
        {
            var data = context.about_ourvalues.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            return View(data);
        }
        [Route("~/Admin/AddAboutOurvalues/1/1")]
        public ActionResult AddAboutOurvalues()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/AddAboutOurvalues/1/1")]
        public ActionResult AddAboutOurvalues(about_ourvalues about_ourvalues, HttpPostedFileBase image_file)
        {
            try
            {
                var data = context.about_ourvalues.Where(u => u.ID == about_ourvalues.ID).FirstOrDefault();
                if (image_file != null)
                {
                    about_ourvalues.image_file = SavePolicyFile(image_file);
                }
                about_ourvalues.title = about_ourvalues.title;
                about_ourvalues.sort_description = about_ourvalues.sort_description;
                about_ourvalues.Isactive = true;
                about_ourvalues.created_at = DateTime.Now;
                context.about_ourvalues.Add(about_ourvalues);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        [Route("~/Admin/AboutOurvision/1/1")]
        public ActionResult AboutOurvision()
        {
            about_ourvision data = new about_ourvision();
            if (data.ID == 0)
            {
                data = context.about_ourvision.FirstOrDefault();
            }
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/AboutOurvision/1/1")]
        public ActionResult AboutOurvision(about_ourvision about_ourvision, HttpPostedFileBase image_1, HttpPostedFileBase image_2, int ID)
        {
            var data = context.about_ourvision.FirstOrDefault();
            try
            {
                if (image_1 != null)
                {
                    data.image_1 = imagesFile(image_1);

                }
                if (image_2 != null)
                {
                    data.image_2 = imagesFile(image_2);

                }

                data.title1 = about_ourvision.title1;
                data.title2 = about_ourvision.title2;
                data.sort_description1 = about_ourvision.sort_description1;
                data.sort_description2 = about_ourvision.sort_description2;
                data.created_at = DateTime.Now;
                context.SaveChanges();
                var successresponse = new { status = 1, message = "Success" };
                return Json(successresponse);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [Route("~/Admin/AboutCertification/1/1")]
        public ActionResult AboutCertification()
        {
            var data = context.about_certification.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            return View(data);
        }

        [Route("~/Admin/AddAboutCertification/1/1")]
        public ActionResult AddAboutCertification()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]

        [Route("~/Admin/AddAboutCertification/1/1")]
        public ActionResult AddAboutCertification(about_certification about_certification, HttpPostedFileBase image_file)
        {
            try
            {
                //var data = context.about_certification.Where(u => u.ID == about_certification.ID).FirstOrDefault();

                if (image_file != null)
                {
                    about_certification.image_file = SaveAboutBannerFile(image_file);
                }
                about_certification.Isactive = true;
                about_certification.created_at = DateTime.Now;
                context.about_certification.Add(about_certification);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }


            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }


        [Route("~/Admin/AboutTestimonil/1/1")]
        public ActionResult AboutTestimonil()
        {
            var data = context.tbl_testimonial.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            return View(data);
        }


        [Route("~/Admin/AddTestimonil/1/1")]
        public ActionResult AddTestimonil()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]

        [Route("~/Admin/AddTestimonil/1/1")]
        public ActionResult AddTestimonil(tbl_testimonial tbl_testimonial, HttpPostedFileBase image_file)
        {
            try
            {

                if (image_file != null)
                {
                    tbl_testimonial.image_file = SaveTestimonialFile(image_file);
                }
                tbl_testimonial.Isactive = true;
                tbl_testimonial.created_at = DateTime.Now;
                context.tbl_testimonial.Add(tbl_testimonial);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }



        [Route("~/Admin/Banner/1/1")]
        public ActionResult Banner()
        {
            var data = context.Tbl_banner.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            return View(data);
        }

        [Route("~/Admin/AddBanner/1/1")]
        public ActionResult AddBanner()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/AddBanner/1/1")]
        public ActionResult AddBanner(Tbl_banner tbl_Banner, HttpPostedFileBase image_file)
        {
            try
            {
                //var data = context.Tbl_banner.Where(u => u.ID == tbl_Banner.ID).FirstOrDefault();

                if (image_file != null)
                {
                    tbl_Banner.image_file = SaveBannerFile(image_file);
                }
                tbl_Banner.Isactive = true;
                tbl_Banner.created_at = DateTime.Now;
                context.Tbl_banner.Add(tbl_Banner);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }


            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [Route("~/Admin/AfterSale/1/1")]
        public ActionResult AfterSale()
        {
            var data = context.Tbl_saleSupportBannes.Where(x => x.Isactive == true).OrderByDescending(x => x.ID).ToList();
            return View(data);
        }
        [Route("~/Admin/AddAfterSale/1/1")]
        public ActionResult AddAfterSale()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/AddAfterSale/1/1")]
        public ActionResult AddAfterSale(Tbl_saleSupportBannes Tbl_saleSupportBannes, HttpPostedFileBase image_file)
        {
            try
            {
                //var data = context.Tbl_saleSupportBannes.Where(u => u.ID == Tbl_saleSupportBannes.ID).FirstOrDefault();

                if (image_file != null)
                {
                    Tbl_saleSupportBannes.image_file = imagesFile(image_file);
                }
                Tbl_saleSupportBannes.Isactive = true;
                Tbl_saleSupportBannes.created_at = DateTime.Now;
                context.Tbl_saleSupportBannes.Add(Tbl_saleSupportBannes);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }


            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [Route("~/Admin/AddAboutUs/1/1")]
        public ActionResult AddAboutUs()
        {
            var data = context.about_us.FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        //[ValidateInput(false)]
        [Route("~/Admin/AddAboutUs/1/1")]
        public ActionResult AddAboutUs(about_us about_Us2, HttpPostedFileBase image_file1, HttpPostedFileBase image_file2, int ID)
        {
            var data = context.about_us.Where(u => u.ID == ID).FirstOrDefault();
            try
            {
                if (image_file1 != null)
                {
                    data.about_image_home = imagesFile(image_file1);

                }
                if (image_file2 != null)
                {
                    data.about_image = imagesFile(image_file2);

                }

                data.sort_description_home = about_Us2.sort_description_home;
                data.full_description = about_Us2.full_description;
                data.created_at = DateTime.Now;
                //context.about_us.Add(about_Us);
                context.SaveChanges();
                var successresponse = new { status = 1, message = "Success" };
                return Json(successresponse);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [Route("~/Admin/AddContactus/1/1")]
        public ActionResult AddContactus()
        {
            var data = context.contact_detail.FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/AddContactus/1/1")]
        public ActionResult AddContactus(contact_detail contact_detail2, int ID)
        {
            try
            {
                var data = context.contact_detail.Where(u => u.ID == ID).FirstOrDefault();
                data.sort_description = contact_detail2.sort_description;
                data.contact_no = contact_detail2.contact_no;
                data.email = contact_detail2.email;
                data.created_at = DateTime.Now;
                data.office_address = contact_detail2.office_address;
                //context.contact_detail.Add(contact_detail);
                context.SaveChanges();

                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [Route("~/Admin/SaveSubCatFile/1/1")]
        public string SaveSubCatFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/subCat");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/subCat/" + file.FileName;
            return path1;
        }
        [Route("~/Admin/SavePolicyFile/1/1")]
        public string SavePolicyFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/category");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/category/" + file.FileName;
            return path1;
        }
        [Route("~/Admin/SavePdfFile/1/1")]
        public string SavePdfFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/pdf");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);
            string path1 = "assets/pdf/" + file.FileName;
            return path1;
        }

        [Route("~/Admin/SaveProductFile/1/1")]
        public string SaveProductFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/Product");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/Product/" + file.FileName;
            return path1;
        }
        [Route("~/Admin/SaveBannerFile/1/1")]
        public string SaveBannerFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/Banner");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/Banner/" + file.FileName;
            return path1;
        }
        [Route("~/Admin/SaveAboutBannerFile/1/1")]
        public string SaveAboutBannerFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/aboutcertificate");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/aboutcertificate/" + file.FileName;
            return path1;
        }
        public string SaveTestimonialFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/testimonial");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/testimonial/" + file.FileName;
            return path1;
        }

        [Route("~/Admin/SaveBlogFile/1/1")]
        public string SaveBlogFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/blog");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/blog/" + file.FileName;
            return path1;
        }


        public string imagesFile(HttpPostedFileBase file)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/images");
            string filePath = directory + "\\" + file.FileName;
            file.SaveAs(filePath);

            string path1 = "assets/images/" + file.FileName;
            return path1;
        }

        [Route("~/Admin/DeleteCategory/{ID}/Del/1/1")]
        public ActionResult DeleteCategory(int ID)
        {
            try
            {

                var cateRect = context.tbl_category.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    cateRect.Isactive = false;
                    context.SaveChanges();

                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("Category");
                }
                else
                {
                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("Category");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Category");
            }
        }

        [Route("~/Admin/DeleteSubCategory/{ID}/Del/1/1")]
        public ActionResult DeleteSubCategory(int ID)
        {
            try
            {

                var cateRect = context.Tbl_subCategory.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    cateRect.Isactive = false;
                    context.SaveChanges();

                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("SubCategory");
                }
                else
                {
                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("SubCategory");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("SubCategory");
            }
        }
        [Route("~/Admin/DeleteAddAboutCertification/{ID}/Del/1/1")]
        public ActionResult DeleteAddAboutCertification(int ID)
        {
            try
            {

                var cateRect = context.about_certification.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    cateRect.Isactive = false;
                    context.SaveChanges();

                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("AboutCertification");
                }
                else
                {
                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("AboutCertification");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("AboutCertification");
            }
        }

        [Route("~/Admin/DeleteTestimonil/{ID}/Del/1/1")]
        public ActionResult DeleteTestimonil(int ID)
        {
            try
            {
                var cateRect = context.tbl_testimonial.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    cateRect.Isactive = false;
                    context.SaveChanges();
                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("AboutTestimonil");
                }
                else
                {
                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("AboutTestimonil");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("AboutTestimonil");
            }
        }


        [Route("~/Admin/DeleteBanner/{ID}/Del/1/1")]
        public ActionResult DeleteBanner(int ID)
        {
            try
            {

                var cateRect = context.Tbl_banner.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    cateRect.Isactive = false;
                    context.SaveChanges();

                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("Banner");
                }
                else
                {
                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("Banner");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Banner");
            }
        }
        [Route("~/Admin/DeleteAddAfterSale/{ID}/Del/1/1")]
        public ActionResult DeleteAddAfterSale(int ID)
        {
            try
            {

                var cateRect = context.Tbl_saleSupportBannes.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    cateRect.Isactive = false;
                    context.SaveChanges();

                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("AfterSale");
                }
                else
                {
                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("AfterSale");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("AfterSale");
            }
        }
        [Route("~/Admin/DeleteProductPermenet/{ID}/Del/1/1")]
        public ActionResult DeleteProductPermenet(int ID)
        {
            try
            {

                var cateRect = context.tbl_product.FirstOrDefault(x => x.ID == ID);

                if (cateRect != null)
                {
                    cateRect.Isactive = false;
                    cateRect.Isdelete = true;
                    context.SaveChanges();

                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("Products");
                }
                else
                {
                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("Products");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Products");
            }
        }
        [Route("~/Admin/DeleteProduct/{ID}/Del/1/1")]
        public ActionResult DeleteProduct(int ID)
        {
            try
            {
                //var cartitems = context.tbl_cart.Where(x => x.PId == ID).ToList();
                //if (cartitems != null && cartitems.Any())
                //{
                //    context.tbl_cart.RemoveRange(cartitems);
                //    context.SaveChanges();
                //}

                var cateRect = context.tbl_product.FirstOrDefault(x => x.ID == ID);
                //var imgPath_1 = cateRect.Image_1;
                //var imgPath_2 = cateRect.Image_2;
                //var imgPath_3 = cateRect.Image_3;
                //var imgPath_4 = cateRect.Image_4;
                if (cateRect != null)
                {
                    cateRect.Isactive = false;
                    context.SaveChanges();


                    //string fullPathImg_1 = Server.MapPath("~/" + imgPath_1);
                    //string fullPathImg_2 = Server.MapPath("~/" + imgPath_2);
                    //string fullPathImg_3 = Server.MapPath("~/" + imgPath_3);
                    //string fullPathImg_4 = Server.MapPath("~/" + imgPath_4);
                    //if (System.IO.File.Exists(fullPathImg_1))
                    //{
                    //    System.IO.File.Delete(fullPathImg_1);
                    //}
                    //if (System.IO.File.Exists(fullPathImg_2))
                    //{
                    //    System.IO.File.Delete(fullPathImg_2);
                    //}
                    //if (System.IO.File.Exists(fullPathImg_3))
                    //{
                    //    System.IO.File.Delete(fullPathImg_3);
                    //}
                    //if (System.IO.File.Exists(fullPathImg_4))
                    //{
                    //    System.IO.File.Delete(fullPathImg_4);
                    //}
                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("Products");
                }
                else
                {
                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("Products");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Products");
            }
        }


        [Route("~/DeleteRecord/{ID}/{tablename}/{ColumnName}")]
        public ActionResult DeleteRecord(int ID, string tablename, string ColumnName)
        {
            try
            {
                // var data = context.tbl_category.Where(x =>x.co).FirstOrDefault();

                SqlConnection con = new SqlConnection(context.Database.Connection.ConnectionString);
                string query = "delete from " + tablename + "  where " + ColumnName + " = " + ID + " ";

                //string query2 = "select * " + tablename + "  where " + ColumnName + " = " + ID + " ";
                //SqlCommand cmd = new SqlCommand(query2, con);

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                {

                    TempData["msgSuccess"] = "Record deleted successfully!";
                    return RedirectToAction("Banner");
                }
                else
                {

                    TempData["msgErr"] = "Getting something error!";
                    return RedirectToAction("Banner");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Banner");
            }
        }

        [Route("~/Admin/DeletePermanentContactEnq/{ID}/Del/1/1")]
        public ActionResult DeletePermanentContactEnq(int ID)
        {
            try
            {
                var cateRect = context.contact_us.FirstOrDefault(x => x.contact_id == ID);
                if (cateRect != null)
                {
                    context.contact_us.Remove(cateRect);
                    context.SaveChanges();
                    TempData["msgSuccess"] = "Record deleted successfully!";
                }
                else
                {
                    TempData["msgErr"] = "Record not foun!";
                }
                return RedirectToAction("contactusEnquiry");
            }
            catch (Exception ex)
            {
                return RedirectToAction("contactusEnquiry");
            }
        }

        [Route("~/Admin/DeletePermanentAboutOurvalues/{ID}/Del/1/1")]
        public ActionResult DeletePermanentAboutOurvalues(int ID)
        {
            try
            {
                var cateRect = context.about_ourvalues.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    context.about_ourvalues.Remove(cateRect);
                    context.SaveChanges();
                    TempData["msgSuccess"] = "Record deleted successfully!";
                }
                else
                {
                    TempData["msgErr"] = "Record not foun!";
                }
                return RedirectToAction("AboutOurvalues/1/1");
            }
            catch (Exception ex)
            {
                return RedirectToAction("contactusEnquiry");
            }
        }


        [Route("~/Admin/DeletePermanentProductEnq/{ID}/Del/1/1")]
        public ActionResult DeletePermanentProductEnq(int ID)
        {
            try
            {
                var cateRect = context.product_enq.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    context.product_enq.Remove(cateRect);
                    context.SaveChanges();
                    TempData["msgSuccess"] = "Record deleted successfully!";
                }
                else
                {
                    TempData["msgErr"] = "Record not foun!";
                }
                return RedirectToAction("ProductEnquiry");
            }
            catch (Exception ex)
            {
                return RedirectToAction("contactusEnquiry");
            }
        }

        [Route("~/Admin/DeletePermanentContactDashboar/{ID}/Del/1/1")]
        public ActionResult DeletePermanentContactDashboar(int ID)
        {
            try
            {
                var cateRect = context.contact_us.FirstOrDefault(x => x.contact_id == ID);
                if (cateRect != null)
                {
                    context.contact_us.Remove(cateRect);
                    context.SaveChanges();
                    TempData["msgSuccess"] = "Record deleted successfully!";
                }
                else
                {
                    TempData["msgErr"] = "Record not foun!";
                }
                return RedirectToAction("");
            }
            catch (Exception ex)
            {
                return RedirectToAction("");
            }
        }
        [Route("~/Admin/DeletePermanentSaleSupport/{ID}/Del/1/1")]
        public ActionResult DeletePermanentSaleSupport(int ID)
        {
            try
            {
                var cateRect = context.sale_support.FirstOrDefault(x => x.complaint_id == ID);
                var imgPath = cateRect.attachment;
                if (cateRect != null)
                {
                    context.sale_support.Remove(cateRect);
                    context.SaveChanges();


                    string fullPathImg = Server.MapPath("~/" + imgPath);
                    if (System.IO.File.Exists(fullPathImg))
                    {
                        System.IO.File.Delete(fullPathImg);
                    }
                    TempData["msgSuccess"] = "Record deleted successfully!";
                }
                else
                {
                    TempData["msgErr"] = "Record not foun!";
                }
                return RedirectToAction("ServiceSupport");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ServiceSupport");
            }
        }
        [Route("~/Admin/DeletePermanentSubscribe/{ID}/Del/1/1")]
        public ActionResult DeletePermanentSubscribe(int ID)
        {
            try
            {
                var cateRect = context.tbl_subscribe.FirstOrDefault(x => x.ID == ID);
                if (cateRect != null)
                {
                    context.tbl_subscribe.Remove(cateRect);
                    context.SaveChanges();
                    TempData["msgSuccess"] = "Record deleted successfully!";
                }
                else
                {
                    TempData["msgErr"] = "Record not foun!";
                }
                return RedirectToAction("Subscribe");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Subscribe");
            }
        }



        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/RemoveProductImage/1/1")]
        public ActionResult RemoveProductImage(tbl_product tbl_product2)
        {
            try
            {
                var data = context.tbl_product.FirstOrDefault(u => u.ID == tbl_product2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                if (tbl_product2.Image_1 == "Image_1")
                {
                    data.Image_1 = null;
                }
                else if (tbl_product2.Image_1 == "Image_2")
                {
                    data.Image_2 = null;
                }
                else if (tbl_product2.Image_1 == "Image_3")
                {
                    data.Image_3 = null;
                }
                else if (tbl_product2.Image_1 == "Image_4")
                {
                    data.Image_4 = null;
                }
                else if (tbl_product2.Image_1 == "Image_5")
                {
                    data.Image_5 = null;
                }
                else if (tbl_product2.Image_1 == "Image_6")
                {
                    data.Image_6 = null;
                }
                else if (tbl_product2.Image_1 == "Image_7")
                {
                    data.Image_7 = null;
                }
                else if (tbl_product2.Image_1 == "Image_8")
                {
                    data.Image_8 = null;
                }

                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }



        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/DisplayProductHomePage/1/1")]
        public ActionResult DisplayProductHomePage(tbl_product tbl_product2)
        {
            try
            {
                var data = context.tbl_product.FirstOrDefault(u => u.ID == tbl_product2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.display_on_home = tbl_product2.display_on_home;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }



        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/otherCatPage/1/1")]
        public ActionResult otherCatPage(tbl_product tbl_product2)
        {
            try
            {
                var data = context.tbl_product.FirstOrDefault(u => u.ID == tbl_product2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.otherCatID = tbl_product2.otherCatID;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }



        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/setorderProduct/1/1")]
        public ActionResult setorderProduct(tbl_product tbl_product2)
        {
            try
            {
                var data = context.tbl_product.FirstOrDefault(u => u.ID == tbl_product2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.serialorder = tbl_product2.serialorder;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/setorderSubcat/1/1")]
        public ActionResult setorderSubcat(Tbl_subCategory Tbl_subCategory2)
        {
            try
            {
                var data = context.Tbl_subCategory.FirstOrDefault(u => u.ID == Tbl_subCategory2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.serialorder = Tbl_subCategory2.serialorder;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/SwitchCategory/1/1")]
        public ActionResult SwitchCategory(tbl_category tbl_category2)
        {
            try
            {
                var data = context.tbl_category.FirstOrDefault(u => u.ID == tbl_category2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.Isactive = tbl_category2.Isactive;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }



        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/ActiveDeactivePro/1/1")]
        public ActionResult ActiveDeactivePro()
        {
            try
            {
                int ID = Convert.ToInt32(Request["ID"]);
                string tablename = Request["tablename"];
                string ColumnName = Request["ColumnName"];
                string IsActive = Request["IsActive"];




                SqlConnection con = new SqlConnection(context.Database.Connection.ConnectionString);
                string query = "update  " + tablename + " set IsActive=" + IsActive + " where " + ColumnName + " = " + ID + " ";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                {

                    var response = new { status = 1, message = "Success" };
                    return Json(response);
                }
                else
                {
                    var response = new { status = 2, message = "Getting something error!" };
                    return Json(response);

                    //TempData["msgErr"] = "Getting something error!";
                    //return RedirectToAction("Banner");
                }

            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/SwitchSubCategory/1/1")]
        public ActionResult SwitchSubCategory(Tbl_subCategory Tbl_subCategory2)
        {
            try
            {
                var data = context.Tbl_subCategory.FirstOrDefault(u => u.ID == Tbl_subCategory2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.Isactive = Tbl_subCategory2.Isactive;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/SwitchProduct/1/1")]
        [Route("~/Admin/SwitchProduct/1/1")]
        public ActionResult SwitchProduct(tbl_product tbl_product2)
        {
            try
            {
                var data = context.tbl_product.FirstOrDefault(u => u.ID == tbl_product2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.Isactive = tbl_product2.Isactive;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/SwitchBlog/1/1")]
        public ActionResult SwitchBlog(Tbl_blog Tbl_blog2)
        {
            try
            {
                var data = context.Tbl_blog.FirstOrDefault(u => u.ID == Tbl_blog2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.Isactive = Tbl_blog2.Isactive;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        [Route("~/Admin/switchFaq/1/1")]
        public ActionResult switchFaq(Tbl_faq Tbl_faq2)
        {
            try
            {
                var data = context.Tbl_faq.FirstOrDefault(u => u.ID == Tbl_faq2.ID);
                if (data == null)
                {
                    var errorresponse = new { status = 2, message = "Record not found!" };
                    return Json(errorresponse);
                }
                data.Isactive = Tbl_faq2.Isactive;
                context.SaveChanges();
                var response = new { status = 1, message = "Success" };
                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new { status = 0, message = ex.Message };
                return Json(response);
            }
        }







    }
}