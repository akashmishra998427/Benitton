using System;
using Benitton.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Benitton.Controllers
{
    ////[RoutePrefix("Login")]
    ////[Route("{action=index}")] //default action
    //[RouteArea("Login")]
    //[RoutePrefix("Index")]
    //[Route("{action}")]
    //[RoutePrefix("Login")]
    //[Route("{Index}")]
    public class LoginController : Controller
    {
        Benitton_dbEntities context = new Benitton_dbEntities();
        // GET: Login


        //[Route("~/Login/{Index}")]
        public ActionResult Index()
        {
            var LoginType = Session["loginType"]?.ToString();
            if (!string.IsNullOrEmpty(LoginType) && LoginType =="Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpPost]
       // [Route("~/Login/Index")]
       // [Route("~/Admin/Index/1/1")]
        public ActionResult Index(string username, string password)
        {
            if (username.ToString().Trim() =="" && password.ToString().Trim() == "") {
                var message = "Invalid login detail!";
                @ViewBag.Message = message;
                return View();
            } else { 
            var data = context.tbl_admin.Where(u => u.emailid == username && u.password == password).FirstOrDefault();

                //var  message = "";
            if (data != null)
            {
                if (data.emailid == username)
                {
                
                    Session["Userid"] = data.id;
                    Session["emailid"] = data.emailid;

                    Session["name"] = data.name;
                    FormsAuthentication.RedirectFromLoginPage(data.emailid, true);
                    if (data.user_type == "Admin")
                    {
                        Session["loginType"] = "Admin";
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        var message = "Invalid login detail!";
                        @ViewBag.Message = message;
                        return View();
                    }
                }
                else
                {
                    var message = "Invalid Emailid!";
                    @ViewBag.Message = message;
                    return View();

                }


            }
            else
            {
                //var message = "Account has not been activated.";
                var message = "Invalid login detail!";
                @ViewBag.Message = message;
                return View();
            }
        }
            return View();
        }
        [Route("~/Login/ForgetPassword/1/1")]
        public ActionResult ForgetPassword()
        {
            return View();

        }
        [HttpPost]
        [Route("~/Login/ForgetPassword/1/1")]
        public ActionResult ForgetPassword(string sendTo)
        {

            try
            {
                var getRec = context.tbl_admin.FirstOrDefault(x => x.emailid == sendTo.ToString().Trim());
                if (getRec != null)
                {
                    var pass = getRec.password;

                    string subject = "Forget Password ";
                    string body = "forget password is : " + pass;

                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress("sales@vcqru.com", "VCQRU");
                    msg.To.Add(sendTo);
                    msg.IsBodyHtml = true;
                    msg.Subject = subject;
                    msg.Body = body;
                    SmtpClient sc = new SmtpClient();

                    sc.Host = "smtp.gmail.com";
                    sc.EnableSsl = true; // changed for vcqru mail domain
                    sc.UseDefaultCredentials = false;// changed for vcqru mail domain
                    sc.Credentials = new NetworkCredential("sales@vcqru.com", "iaqesbfqqugepseh");
                    sc.Port = 587;// changed for vcqru mail domain
                    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                    sc.Send(msg);
                      var message = "Password send successfully!";
                     @ViewBag.Message = message;
                      return View();
                   
                }
                else
                {
                      var message = "Invalid Email Id!";
                     @ViewBag.Message = message;
                      return View("ForgetPassword");
                   
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ForgetPassword");
            }


        }
        [Route("~/Login/Logout/1/1")]
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }
    }
}