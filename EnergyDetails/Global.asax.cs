using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EnergyDetails
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //}
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Timers.Timer myTimer = new System.Timers.Timer();
            // Set the Interval to 5 seconds (5000 milliseconds).
            myTimer.Interval = 1000;
            myTimer.AutoReset = true;
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
        }

        public void myTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            // use your mailer code
            //clsScheduleMail objScheduleMail = new clsScheduleMail();
            //objScheduleMail.SendScheduleMail();
            DateTime now = DateTime.Now;
            string time = now.ToString("T");
            TimeSpan duration = DateTime.Parse("4:15:26 PM").Subtract(DateTime.Parse(time));
            if (time == "11:53:00 AM")
            {
                //SendScheduleMail();   RedirectToAction("EnergyLevel", "GoogleMap");
                //HttpContext.Current.RewritePath("EnergyLevel/GoogleMap");
                //RouteData routeData = new RouteData();
                //routeData.Values.Add("EnergyLevel", "GoogleMap");

                //Server.Transfer("ReportShedual.aspx");
                //SendScheduleMail();
            }
        }
        public void SendScheduleMail()
        {

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("chandreshk@inzerotech.com");
                mail.To.Add("chandreshk@inzerotech.com");
                mail.Subject = "Test Mail";
                mail.Body = "<b>Please find attached energy report</b>";
                mail.IsBodyHtml = true;
                mail.Attachments.Add(new Attachment(Server.MapPath("~/pdfs/Intensity.pdf")));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("chandreshk@inzerotech.com", "inzerotech");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
