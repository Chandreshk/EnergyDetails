using System;
using System.Data;
using System.Web.Mvc;
using ClientBLL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Mail;
using HiQPdf;
using System.Configuration;
using System.Web;
using System.Data.OleDb;
using EnergyDetails.Models;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace EnergyDetails.Controllers
{

    public class EnergyLevelController : Controller
    {
        bool Flage = false;
        // GET: EnergyLevel
        public ActionResult Login()
        {
            Session.Abandon();
            return View();
        }
        public ActionResult GoogleMap()
        {
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                //    if (Session["UserID"].ToString() != null)
                //{
                DataSet ds = new DataSet();
                string markers = "";
                ClientBAL objBAL = new ClientBAL();
                DataRow dr;
                DataTable dt = new DataTable("Facilities");
                dt.Clear();
                dt.Columns.Add("FacilitiesID");
                dt.Columns.Add("FacilityName");
                dt.Columns.Add("Address1");
                dt.Columns.Add("City");
                dt.Columns.Add("StateID");
                dt.Columns.Add("RegionID");
                dt.Columns.Add("CountryID");
                dt.Columns.Add("PostalCode");
                dt.Columns.Add("Latitud");
                dt.Columns.Add("Longitude");
                dt.Columns.Add("BuildingAvailabe");
                try
                {
                    ds = objBAL.GetOrgfacilityData();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        try
                        {


                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                string url = "https://maps.googleapis.com/maps/api/geocode/xml?key=AIzaSyAOKtZOo1UZ2VYHj6BrprF6KHWsT0-Sne8&address=" + ds.Tables[0].Rows[i]["City"] + "%=" + ds.Tables[0].Rows[i]["PostalCode"] + "&sensor=true";
                                WebRequest request = WebRequest.Create(url);
                                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                                {
                                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                    {
                                        DataSet dsResult = new DataSet();
                                        dsResult.ReadXml(reader);
                                        DataTable dtCoordinates = new DataTable();
                                        dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                                new DataColumn("Address", typeof(string)),
                                new DataColumn("Latitude",typeof(string)),
                                new DataColumn("Longitude",typeof(string)) });
                                        foreach (DataRow row in dsResult.Tables["result"].Rows)
                                        {
                                            try
                                            {


                                                if (dsResult.Tables.Contains("geometry"))
                                                {
                                                    string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                                                    if (geometry_id == "0")
                                                    {
                                                        DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                                                        dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                                                        Int32 Rcount = Convert.ToInt32(dt.Rows.Count);
                                                        dr = dt.NewRow();
                                                        dr["FacilitiesID"] = ds.Tables[0].Rows[i]["FacilityID"];
                                                        dr["FacilityName"] = ds.Tables[0].Rows[i]["FacilityName"];
                                                        dr["Address1"] = ds.Tables[0].Rows[i]["Address1"];
                                                        dr["City"] = row["formatted_address"];
                                                        dr["StateID"] = ds.Tables[0].Rows[i]["StateID"];
                                                        dr["RegionID"] = ds.Tables[0].Rows[i]["RegionID"];
                                                        dr["CountryID"] = ds.Tables[0].Rows[i]["CountryID"];
                                                        dr["PostalCode"] = ds.Tables[0].Rows[i]["PostalCode"];
                                                        dr["Latitud"] = location["lat"];
                                                        dr["Longitude"] = location["lng"];
                                                        dr["BuildingAvailabe"] = ds.Tables[0].Rows[i]["BuildingAvailabe"];
                                                        dt.Rows.Add(dr);

                                                    }

                                                }
                                            }
                                            catch (Exception ex)
                                            {


                                            }
                                        }
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            
                        }
                        ds = objBAL.UpdatedFacilites(dt);
                    }
                    //else
                    //{
                    //ViewBag.Markers = "'NoData'";
                    //RedirectToAction("EnergyLevel", "GoogleMap");
                    ds = objBAL.Getfacicility();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        markers = "[";
                        for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                        {
                            markers += "{";
                            markers += string.Format("'title': '{0}',", ds.Tables[0].Rows[i]["FacilityName"]);
                            markers += string.Format("'lat': '{0}',", ds.Tables[0].Rows[i]["Latitud"]);
                            markers += string.Format("'lng': '{0}',", ds.Tables[0].Rows[i]["Longitude"]);
                            markers += string.Format("'FacilityID': '{0}'", ds.Tables[0].Rows[i]["FacilitiesID"]);
                            markers += string.Format(",'SiteData': '{0}'", ds.Tables[0].Rows[i]["BuildingAvailabe"]);
                            markers += "},";
                        }
                        markers += "];";
                        ViewBag.Markers = markers;
                    }
                    else
                    {
                        ViewBag.Markers = "'NoData'";
                        return RedirectToAction("Login", "EnergyLevel");
                    }

                    // }
                }
                catch (Exception ex)
                {
                    markers += "];";
                    ViewBag.Markers = markers;
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
            }
            return View();
        }
        public ActionResult SiteLocation()
        {
            //if (Session["UserID"].ToString() != null && Session["UserID"].ToString() != "")
            //{
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                string FacilityID = Request.QueryString["FacilityID"];
                DataSet ds = new DataSet();
                string markers = "";
                ClientBAL objBAL = new ClientBAL();
                DataRow dr;
                DataTable dt = new DataTable("Location");
                dt.Clear();
                dt.Columns.Add("LocationID");
                dt.Columns.Add("ParentLocationID");
                dt.Columns.Add("LocationName");
                dt.Columns.Add("LocationType");
                dt.Columns.Add("Description");
                dt.Columns.Add("City");
                dt.Columns.Add("StateID");
                dt.Columns.Add("PostalCode");
                dt.Columns.Add("CountryID");
                dt.Columns.Add("FacilityID");
                dt.Columns.Add("FacilityName");
                dt.Columns.Add("Latitud");
                dt.Columns.Add("Longitude");
                dt.Columns.Add("BuildingAvailabe");
                try
                {
                    ds = objBAL.GetsiteDetails(FacilityID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                        {
                            if (ds.Tables[0].Rows[i]["PostalCode"].ToString() != null && ds.Tables[0].Rows[i]["PostalCode"].ToString() != "")
                            {
                                //string url = "http://maps.google.com/maps/api/geocode/xml?address=" + ds.Tables[0].Rows[i]["PostalCode"] + "&sensor=false";
                                string url = "https://maps.googleapis.com/maps/api/geocode/xml?key=AIzaSyDkK_LN5Ddk1MRX-Hnwca1iS3krmXRys_M&address=" + ds.Tables[0].Rows[i]["LocationName"] + "%=" + ds.Tables[0].Rows[i]["PostalCode"] + "&sensor=true";
                                //string url = "http://maps.google.com/maps/api/geocode/xml?address=" + ds.Tables[0].Rows[i]["LocationName"] + "%=" + ds.Tables[0].Rows[i]["PostalCode"] + "&sensor=false";
                                WebRequest request = WebRequest.Create(url);
                                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                                {
                                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                    {
                                        DataSet dsResult = new DataSet();
                                        dsResult.ReadXml(reader);
                                        DataTable dtCoordinates = new DataTable();
                                        dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                                        new DataColumn("Address", typeof(string)),
                                        new DataColumn("Latitude",typeof(string)),
                                        new DataColumn("Longitude",typeof(string)) });
                                        foreach (DataRow row in dsResult.Tables["result"].Rows)
                                        {
                                            string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                                            if (geometry_id == "0")
                                            {
                                                DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                                                dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                                                Int32 Rcount = Convert.ToInt32(dt.Rows.Count);
                                                dr = dt.NewRow();
                                                dr["LocationID"] = ds.Tables[0].Rows[i]["LocationID"];
                                                dr["ParentLocationID"] = ds.Tables[0].Rows[i]["ParentLocationID"];
                                                dr["LocationName"] = ds.Tables[0].Rows[i]["LocationName"];

                                                dr["LocationType"] = ds.Tables[0].Rows[i]["LocationType"];
                                                dr["Description"] = ds.Tables[0].Rows[i]["Description"];
                                                dr["City"] = row["formatted_address"];
                                                dr["StateID"] = ds.Tables[0].Rows[i]["StateID"];
                                                dr["PostalCode"] = ds.Tables[0].Rows[i]["PostalCode"];
                                                dr["CountryID"] = ds.Tables[0].Rows[i]["CountryID"];
                                                dr["FacilityID"] = ds.Tables[0].Rows[i]["FacilityID"];
                                                dr["PostalCode"] = ds.Tables[0].Rows[i]["PostalCode"];
                                                dr["FacilityName"] = ds.Tables[0].Rows[i]["FacilityName"];
                                                dr["Latitud"] = location["lat"];
                                                dr["Longitude"] = location["lng"];
                                                dr["BuildingAvailabe"] = ds.Tables[0].Rows[i]["BuildingAvailabe"];
                                                dt.Rows.Add(dr);

                                            }
                                        }
                                    }

                                }
                            }
                        }
                        ds = objBAL.UpdatedLocation(dt);
                    }
                    ds = objBAL.GetUpdatedsiteDetails(FacilityID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        markers = "[";
                        for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                        {
                            markers += "{";
                            markers += string.Format("'title': '{0}',", ds.Tables[0].Rows[i]["LocationName"]);
                            markers += string.Format("'lat': '{0}',", ds.Tables[0].Rows[i]["Latitud"]);
                            markers += string.Format("'lng': '{0}',", ds.Tables[0].Rows[i]["Longitude"]);
                            // markers += string.Format("'description': '{0}'", row["formatted_address"]);
                            markers += string.Format("'LocationID': '{0}'", "?FacilityID=" + ds.Tables[0].Rows[i]["FacilityID"] + "&LocationID=" + ds.Tables[0].Rows[i]["LocationID"]);
                            markers += string.Format(",'SiteData': '{0}'", ds.Tables[0].Rows[i]["BuildingAvailabe"]);
                            markers += "},";
                        }
                        markers += "];";
                        ViewBag.Markers = markers;
                    }
                    else
                    {
                        ViewBag.Markers = "'NoData'";
                        RedirectToAction("EnergyLevel", "Login");
                    }
                }
                catch (Exception ex)
                {
                    RedirectToAction("EnergyLevel", "Login");
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
            return View();
        }
        public ActionResult Setting()
        {
           
            return View();
        }
        public string ImageConvertBit(string filePath)
        {
            string ByteString = "";
            System.Drawing.Image img = System.Drawing.Image.FromFile(filePath);
            using (System.IO.MemoryStream m = new System.IO.MemoryStream())
            {
                img.Save(m, img.RawFormat);
                byte[] imageBytes = m.ToArray();

                // Convert byte[] to Base64 String
                ByteString = Convert.ToBase64String(imageBytes);


            }
            filePath = "data:image/jpeg;base64," + ByteString;
            return filePath;
        }
        public ActionResult HighChart()
        {
            //if (Session["UserID"].ToString() != null && Session["UserID"].ToString() != "")
            //{
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                DataSet ds = new DataSet();
                string ByteString = "";
                ClientBAL objBAL = new ClientBAL();
                string FacilityID = Request.QueryString["FacilityID"];
                string LocationID = Request.QueryString["LocationID"];

                Session["FacilityID"] = FacilityID;
                Session["LocationID"] = LocationID;
                string filePath = ConfigurationManager.AppSettings["ImagePath"].ToString();
                string LogoImagePath = Server.MapPath("~/Content/image/proteus.png");


                ViewBag.LogoImage = ImageConvertBit(filePath);
                ViewBag.LogoImagePath = ImageConvertBit(LogoImagePath);
                try
                {
                    string UserId = Session["UserID"].ToString();
                    ViewBag.UserName = Session["UserName"].ToString();

                    ds = objBAL.GetSiteDetails(FacilityID, LocationID, UserId);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.FacilityName = ds.Tables[0].Rows[0]["FacilityName"].ToString();
                        ViewBag.SiteName = ds.Tables[0].Rows[0]["SiteName"].ToString();
                    }
                    else
                    {
                        ViewBag.FacilityName = "";
                        ViewBag.SiteName = "";
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if ("MonthlyTrend" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.MonthlyTrend = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("ProteusEnergyAnalytics" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.ProteusEnergyAnalytics = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("HomePage" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.HomePage = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("DailyEnergyConsumption" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.DailyEnergyConsumption = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("MonthlyEnergyConsumption" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.MonthlyEnergyConsumption = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("MonthlyEnergyExpenses" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.MonthlyEnergyExpenses = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("MonthlyEnergyIntensity" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.MonthlyEnergyIntensity = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("YearlyEnergyConsumption" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.YearlyEnergyConsumption = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("YearlyEnergyExpenses" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.YearlyEnergyExpenses = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("YearlyEnergyIntensity" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.YearlyEnergyIntensity = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("WeatherAdjustedEnergyConsumption" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.WeatherAdjustedEnergyConsumption = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("SendReport" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.SendReport = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("MonthlyPeekDemand" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.MonthlyPeekDemand = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("DailyDataShrink" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.DailyDataShrink = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("ImportData" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.ImportData = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("summary" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.Summary = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("EmailIdcannotBlank" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.EmailIdcannotBlank = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("Reportsentsuccessfully" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.Reportsentsuccessfully = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("Filecannotempty" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.Filecannotempty = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("SuccessfulimportExcel" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                Session["SuccessfulimportExcel"] = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("Datashrinksuccessful" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.Datashrinksuccessful = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("EnterEmail" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.EnterEmail = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("InvalidEmail" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.InvalidEmail = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("WelcomeUser" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.WelcomeUser = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("Setting" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.Setting = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("Nofileselected" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                Session["Nofileselected"] = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("MonthlyGasConsumption" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.MonthlyGasConsumption = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("Save" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.Save = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("PleaseSelectMaximum30Days" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.PleaseSelectMaximum30Days = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("PleaseSelectMaximum12Month" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.PleaseSelectMaximum12Month = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("From" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.From = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("To" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.To = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("SettingUpdateSuccessfully" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.SettingUpdateSuccessfully = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("SorryServerError" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.SorryServerError = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }
                            if ("FromDateshoulbelessthanToDate" == ds.Tables[1].Rows[i]["TitleName"].ToString())
                            {
                                ViewBag.FromDateshoulbelessthanToDate = ds.Tables[1].Rows[i]["TargetName"].ToString();
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Login", "EnergyLevel");
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
            return View();
        }
        public ActionResult TranslateLanguage(string textvalue, string Languageto)
        {
            string translation = "";
            string appId = "A70C584051881A30549986E65FF4B92B95B353A5";//go to http://msdn.microsoft.com/en-us/library/ff512386.aspx to obtain AppId.
                                                                      // string textvalue = "Translate this for me";
            string from = "en";
            //string to = "es";

            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?appId=" + appId + "&text=" + textvalue + "&from=" + from + "&to=" + Languageto;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            WebResponse response = null;
            try
            {
                response = httpWebRequest.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                    translation = (string)dcs.ReadObject(stream);

                    //ltTranslatetxt.Text = "<b>The translated text is: </b>'" + translation + "'.";
                }
            }
            catch (WebException e)
            {
                translation = textvalue;
                // ProcessWebException(e, "Failed to translate");
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
            return Json(translation, JsonRequestBehavior.AllowGet);
        }
        public void myTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            // use your mailer code
            //clsScheduleMail objScheduleMail = new clsScheduleMail();
            //objScheduleMail.SendScheduleMail();
            DateTime now = DateTime.Now;
            string time = now.ToString("T");
            TimeSpan duration = DateTime.Parse("4:15:26 PM").Subtract(DateTime.Parse(time));

            if (time == "5:54:00 PM")
            {
                //string  BuilDID = Session["BuilDIDShedual"].ToString();

                RedirectToAction("EnergyLevel", "HighChart");
                GetHighChart("ShadulTime", "", "");
                // SendScheduleMail();
            }
        }
        public ActionResult GetHighChart(string Monthnumber, string BuilDID, string LocationID)
        {


            DataSet ds = new DataSet();
            ClientBAL objBAL = new ClientBAL();
            string jsonData = "";
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                //if (Session["UserID"].ToString() != null && Session["UserID"].ToString() != "")
                //{
                try
                {
                    if (Monthnumber == "ShadulTime")
                    {
                        Monthnumber = "DailyColumn";
                        BuilDID = "ShadulTime";
                        //LocationID = Session["LocationIDShadual"].ToString();
                    }
                    if (Monthnumber == "DailyColumn")
                    {
                        Session.Add("MonthnumberShadul", Monthnumber);
                        Session["BuilDIDShedual"] = BuilDID;
                        Session["LocationIDShadual"] = LocationID;
                    }
                    // Monthnumber = Session["MonthnumberShadul"].ToString();
                    //Session["Monthnumber"] = Monthnumber;
                    if (BuilDID == "Test" || Flage == true && Session["BuilDID"].ToString() != null)
                    {
                        BuilDID = Session["BuilDID"].ToString();
                        Flage = false;
                    }
                    if (LocationID == "")
                    {
                        // LocationID = Session["LocationID"].ToString();

                    }
                    else
                    {
                        Session["BuilDID"] = BuilDID;
                        //Session["LocationID"] = LocationID;
                    }
                    int UserId= Convert.ToInt32(Session["UserID"].ToString());
                    ds = objBAL.GetHighChart(Monthnumber, BuilDID, LocationID, UserId);
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                        JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                        serializerSettings.Converters.Add(new DataTableConverter());
                        jsonData = JsonConvert.SerializeObject(ds, Formatting.None, serializerSettings);
                    //}
                    //else
                    //{
                    //    jsonData = "Fail";
                    //}
                }
                catch (Exception ex)
                {
                    jsonData = "Fail";
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLoginDetails(string UserName, string Password)
        {
            DataSet ds = new DataSet();
            ClientBAL objBAL = new ClientBAL();
            ServiceResponse resp = new ServiceResponse();
            string jsonData = "";
            //if (Session["UserID"].ToString() != null && Session["UserID"].ToString() != "")
            //{
            try
            {
                //string FileName = "TestPdfFile";
                // string pass = ProteusMMX.ProteusCryptographer.Decrypt("sWUHf6dpJVoc7uLbTmw6Sg==");
                /*========== Write details into pdfs file and saving it into folder "pdfs(at the root of application )"*==========*/
                ds = objBAL.GetLoginDetails(UserName, ProteusMMX.ProteusCryptographer.Encrypt(Password));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["UserID"] = ds.Tables[0].Rows[0]["UserID"].ToString();
                    Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                    serializerSettings.Converters.Add(new DataTableConverter());
                    jsonData = JsonConvert.SerializeObject(ds, Formatting.None, serializerSettings);
                    resp.success = true;
                    resp.result = "Success";
                }
                else
                {
                    resp.result = "Fail";
                }
            }
            catch
            {
                resp.result = "Fail";
            }
            //}
            //else
            //{
            //    return RedirectToAction("Login", "EnergyLevel");
            //    //RedirectToAction("EnergyLevel", "Login");
            //}
            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExporttoPDf(string HighchartData, string FileName, string EmailId, string Subject)
        {
            DataSet ds = new DataSet();
            ClientBAL objBAL = new ClientBAL();
            string jsonData = "";
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                //if (Session["UserID"].ToString() != null && Session["UserID"].ToString() != "")
                //{
                try
                {
                    //string FileName = "TestPdfFile";

                    /*========== Write details into pdfs file and saving it into folder "pdfs(at the root of application )"*==========*/
                    WritePdf(HighchartData, FileName, EmailId, Subject);
                    jsonData = "Sucesses";
                }
                catch (Exception ex)
                {
                    //  string filePath = @"C:\Error.txt";
                    string filePath = Server.MapPath("~/Error/Error.txt");

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                        writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                        jsonData = "Fail";
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetfacilityData()
        {
            DataSet ds = new DataSet();
            ClientBAL objBAL = new ClientBAL();
            string jsonData = "";
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                //if (Session["UserID"].ToString() != null && Session["UserID"].ToString() != "")
                //{
                try
                {
                    ds = objBAL.Getfacicility();
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                    serializerSettings.Converters.Add(new DataTableConverter());
                    jsonData = JsonConvert.SerializeObject(ds, Formatting.None, serializerSettings);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Login", "EnergyLevel");
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSitebuildingDetails(string FacilityID, string LocationID)
        {
            DataSet ds = new DataSet();
            ClientBAL objBAL = new ClientBAL();
            string jsonData = "";
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                //if (Session["UserID"].ToString() != null && Session["UserID"].ToString() != "")
                //{
                try
                {
                    ds = objBAL.GetBulidingDetails(FacilityID, LocationID);
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                    serializerSettings.Converters.Add(new DataTableConverter());
                    jsonData = JsonConvert.SerializeObject(ds, Formatting.None, serializerSettings);

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Login", "EnergyLevel");
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DailyDataShrink()
        {
            DataSet ds = new DataSet();
            ClientBAL objBAL = new ClientBAL();
            ServiceResponse resp = new ServiceResponse();
            string jsonData = "";
            string success; ;
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                //if (Session["UserID"].ToString() != null && Session["UserID"].ToString() != "")
                //{
                try
                {
                    success = objBAL.DataShrink();
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                    serializerSettings.Converters.Add(new DataTableConverter());
                    jsonData = JsonConvert.SerializeObject(success, Formatting.None, serializerSettings);
                    resp.success = true;
                    resp.result = jsonData;
                }
                catch (Exception ex)
                {
                    resp.msg = ex.Message;
                    resp.success = false;
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        public void WritePdf(string MainBody, string filename, string EmailId, string Subject)
        {
            try
            {
                string UserName = Session["UserName"].ToString();
                string smtpPassword = string.Empty;
                string smtpUser = string.Empty;
                string SmtpHost = string.Empty;
                string SmtpPort = string.Empty;
                DataSet ds = new DataSet();
                ClientBAL objBAL = new ClientBAL();
                ds = objBAL.GetUSerDetails();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //  objPS.Row = i;

                    if (ds.Tables[0].Rows[i]["Name"].ToString() == "SmtpAccountUser")
                    {
                        smtpUser = ProteusMMX.ProteusCryptographer.Decrypt(ds.Tables[0].Rows[i]["Value"].ToString());
                    }
                    else if (ds.Tables[0].Rows[i]["Name"].ToString() == "SmtpAccountPassword")
                    {
                        smtpPassword = ProteusMMX.ProteusCryptographer.Decrypt(ds.Tables[0].Rows[i]["Value"].ToString());
                    }
                    else if (ds.Tables[0].Rows[i]["Name"].ToString() == "SmtpHost")
                    {
                        SmtpHost = ds.Tables[0].Rows[i]["Value"].ToString();
                    }
                    else if (ds.Tables[0].Rows[i]["Name"].ToString() == "SmtpPort")
                    {
                        SmtpPort = ds.Tables[0].Rows[i]["Value"].ToString();
                    }
                }
                string tableHeader = "";
                string tableFooter = "";
                //DateTime now = DateTime.Now;
                //string date = now.GetDateTimeFormats('d')[0];
                //string time = now.GetDateTimeFormats('t')[0];
                //DateTime utcTime = DateTime.Now.ToUniversalTime();
                DateTime utcDate = DateTime.SpecifyKind(Convert.ToDateTime(DateTime.Now), DateTimeKind.Utc);

                tableHeader = "<html xmlns='http://www.w3.org/1999/xhtml'><head><p style='margin-left:33%;margin-top:10%;font-size:300%;'>Proteus Energy Analytics</p><title></title></head><body></br></br></br></br></br></br></br></br></br></br>";
                tableFooter = "<div style='margin-left:33%;'><span><b>Generated by :" + UserName + "</b><br><b>Generated on :" + utcDate + "</b></span></div></body></html>";
                string mBody = tableHeader + MainBody + tableFooter;
                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                string pdfFile = null;
                pdfFile = Server.MapPath("~/pdfs/" + filename + ".pdf");
                if (System.IO.File.Exists(pdfFile))
                {
                    System.IO.File.Delete(pdfFile);
                }
                htmlToPdfConverter.ConvertHtmlToFile(mBody, "", pdfFile);
                string[] MultipleEmail = EmailId.Split(',', ';');
                for (int i = 0; i <= MultipleEmail.Length - 1; i++)
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(smtpUser);
                        mail.To.Add(MultipleEmail[i]);
                        mail.Subject = Subject;
                        mail.Body = "<b>Please find attached energy report</b>";
                        mail.IsBodyHtml = true;
                        mail.Attachments.Add(new Attachment(Server.MapPath("~/pdfs/" + filename + ".pdf")));

                        using (SmtpClient smtp = new SmtpClient(SmtpHost, Convert.ToInt32(SmtpPort)))
                        {
                            smtp.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string filePath = Server.MapPath("~/Error/Error.txt");
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }
        [HttpPost]
        public ActionResult UploadFiles(string BuildingID)
        {
            DataTable dst = new DataTable();
            ClientBAL objBAL = new ClientBAL();
            ServiceResponse resp = new ServiceResponse();
            string jsonData = "";
            string success = "";
            string pageId = BuildingID;
            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        HttpFileCollectionBase files = Request.Files;
                        string fileExtension = "";
                        string fileLocation = "";
                        if (Request.Files.Count > 0)
                        {
                            for (int u = 0; u < files.Count; u++)
                            {
                                //fileLocation = Path.Combine(Server.MapPath("~/Uploads/"));
                                string filename = Path.GetFileName(Request.Files[u].FileName);

                                HttpPostedFileBase file = files[u];
                                string fname;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = file.FileName;
                                }
                                fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                                if (System.IO.File.Exists(fname))
                                {

                                    System.IO.File.Delete(fname);
                                }

                                file.SaveAs(fname);
                                fileLocation = fname;

                                //string test = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                                // //file.SaveAs(fname);
                                fileExtension = System.IO.Path.GetExtension(Request.Files[u].FileName);
                                Flage = true;
                                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                                {
                                    if (System.IO.File.Exists(fileLocation))
                                    {
                                        System.IO.File.Delete(fileLocation);
                                    }
                                    Request.Files[u].SaveAs(fileLocation);
                                    string excelConnectionString = string.Empty;
                                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                    //connection String for xls file format.
                                    if (fileExtension == ".xls" || fileExtension == ".csv")
                                    {
                                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                    }
                                    //connection String for xlsx file format.
                                    else if (fileExtension == ".xlsx")
                                    {
                                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                    }
                                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                                    excelConnection.Open();
                                    DataTable dt = new DataTable();
                                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    if (dt == null)
                                    {
                                        return null;
                                    }
                                    String[] excelSheets = new String[dt.Rows.Count];
                                    int t = 0;
                                    //excel data saves in temp file here.
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        excelSheets[t] = row["TABLE_NAME"].ToString();
                                        t++;
                                    }
                                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                                    {
                                        dataAdapter.Fill(dst);
                                        DataColumn Col = dst.Columns.Add("BuildingAutomationBindingID", System.Type.GetType("System.String"));
                                        Col.SetOrdinal(3);
                                        dst.Columns["BuildingAutomationBindingID"].Expression = pageId;
                                        excelConnection.Close();
                                    }
                                }
                                if (fileExtension == ".csv")
                                {
                                    string filepath = fileLocation;
                                    try
                                    {
                                        DataTable dt = new DataTable();
                                        {
                                            DataTable dtDataSource = new DataTable();
                                            string[] fileContent = System.IO.File.ReadAllLines(filepath);
                                            string[] columns = fileContent[0].Split(',');
                                            for (int i = 0; i < columns.Length; i++)
                                            {
                                                dtDataSource.Columns.Add(columns[i]);
                                            }
                                            for (int i = 1; i < fileContent.Length; i++)
                                            {
                                                string[] rowData = fileContent[i].Split(',');
                                                dtDataSource.Rows.Add(rowData);
                                            }
                                            DataColumn Col = dtDataSource.Columns.Add("BuildingAutomationBindingID", System.Type.GetType("System.String"));
                                            Col.SetOrdinal(2);
                                            dtDataSource.Columns["BuildingAutomationBindingID"].Expression = pageId;
                                            dst = dtDataSource;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                    // }
                                }
                                if (fileExtension == ".pdf")
                                {
                                    PdfReader reader = new PdfReader(fileLocation);
                                    // PdfReader pdfReader = new PdfReader((HttpContext.Current.Server.MapPath("~/Pdf/billconvert.pdf")));
                                    int intPageNum = reader.NumberOfPages;
                                    string[] words;
                                    string line;
                                    string text;
                                    DataTable dt = new DataTable();
                                    DataRow dr = null;
                                    dt.Columns.Add(new DataColumn("MeterID", typeof(string)));
                                    dt.Columns.Add(new DataColumn("MeterType", typeof(Int32)));
                                    dt.Columns.Add(new DataColumn("BuildingAutomationBindingID", typeof(Int32)));
                                    dt.Columns.Add(new DataColumn("UnitID", typeof(Int32)));
                                    dt.Columns.Add(new DataColumn("PreviousReading", typeof(decimal)));
                                    dt.Columns.Add(new DataColumn("PreviousTimestamp", typeof(DateTime)));
                                    dt.Columns.Add(new DataColumn("CurrentValue", typeof(decimal)));
                                    dt.Columns.Add(new DataColumn("CurrentTimestamp", typeof(DateTime)));
                                    dt.Columns.Add(new DataColumn("RateValue", typeof(decimal)));
                                    dt.Columns.Add(new DataColumn("DaysBill", typeof(Int32)));
                                    // dt.Columns.Add(new DataColumn("DaysBill", typeof(Int32)));
                                    dr = dt.NewRow();
                                    for (int i = 1; i <= intPageNum; i++)
                                    {
                                        string MeterId = "", PreviousReading = "", CurrentValue = "", PreviousTimestamp = "", CurrentTimestamp = "",DaysBill="";
                                        text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                                        words = text.Split('\n');
                                        int index = Array.IndexOf(words, 11);
                                        bool flage = false;
                                        for (int j = 0, len = words.Length; j < len; j++)
                                        {
                                            string[] meterids;
                                            //if (j == 35 && i == 1)
                                            //{
                                            //string MeterId = words[j];
                                            if (j > 30 && i == 1)
                                            {
                                                meterids = words[j].Split(' ');
                                                for (int k = 0; k < meterids.Length; k++)
                                                {
                                                    if (meterids[k] == "Meter" && j > 30)
                                                    {
                                                        for (int p = 0; p < meterids.Length; p++)
                                                        {
                                                            if (p == 3)
                                                            {
                                                                MeterId = meterids[p];
                                                            }
                                                            if (p == 10)
                                                            {
                                                                DaysBill = meterids[p];
                                                            }
                                                        }
                                                    }

                                                    if (meterids[k] == "Actual" && j > 30)
                                                    {
                                                        for (int q = 0; q < meterids.Length; q++)
                                                        {
                                                           
                                                            if (q == 4&& flage==false)
                                                            {
                                                                flage = true;
                                                                string[] PreRedingDate = meterids[q].Split(new string[] { "....................." }, StringSplitOptions.None);
                                                                 CurrentTimestamp = PreRedingDate[0].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                                                if (PreRedingDate[1].Length < 3)
                                                                {
                                                                    CurrentValue = meterids[5].TrimStart(". ".ToCharArray()).TrimEnd(". ".ToCharArray());
                                                                }
                                                                else
                                                                {
                                                                    CurrentValue = PreRedingDate[1].TrimStart(". ".ToCharArray()).TrimEnd(". ".ToCharArray());
                                                                }
                                                                break;
                                                            }
                                                           else if (flage == true)
                                                            {
                                                                string[] PreRedingDate = meterids[3].Split(new string[] { "....................." }, StringSplitOptions.None);
                                                                PreviousTimestamp = PreRedingDate[0].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                                                if (PreRedingDate.Length > 1)
                                                                {
                                                                    PreviousReading = meterids[4].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                                                }
                                                                dt.Rows.Add(MeterId, 28, BuildingID, 16, PreviousReading, DateTime.Parse(PreviousTimestamp), 0, DateTime.Parse(PreviousTimestamp), 0.5000, DaysBill);
                                                                dt.Rows.Add(MeterId, 28, BuildingID, 16, CurrentValue, DateTime.Parse(CurrentTimestamp), 0, DateTime.Parse(CurrentTimestamp), 0.5000, DaysBill);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                           
                                            //}
                                            //if (j == 37 && i == 1)
                                            //{
                                            //     meterids = words[j].Split(' ');
                                            //    for (int q = 0; q < meterids.Length; q++)
                                            //    {
                                            //        if (q == 3)
                                            //        {
                                            //            string[] PreRedingDate = meterids[q].Split(new string[] { "....................." }, StringSplitOptions.None);
                                            //            PreviousTimestamp = PreRedingDate[0].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                            //            PreviousReading = PreRedingDate[1].TrimStart(". ".ToCharArray()).TrimEnd(". ".ToCharArray());
                                            //        }
                                            //        if (q == 4)
                                            //        {
                                            //            if (meterids[q] != "State")
                                            //            {
                                            //                PreviousReading = meterids[q];
                                            //            }
                                            //            //dt.Rows.Add(MeterId, 29, BuildingID, 16, PreviousReading, PreviousTimestamp, 0, PreviousTimestamp, 0.5000);
                                            //            dt.Rows.Add(MeterId, 28, BuildingID, 16, PreviousReading, PreviousTimestamp, 0, PreviousTimestamp, 0.5000);
                                            //            dt.Rows.Add(MeterId, 28, BuildingID, 16, CurrentValue, CurrentTimestamp, 0, CurrentTimestamp, 0.5000);
                                            //            //dt.Rows.Add(MeterId, 29, BuildingID, 16, CurrentValue, CurrentTimestamp, 0, CurrentTimestamp, 0.5000);
                                            //        }
                                            //    }
                                            ////}
                                            ////if (j == 36 && i == 1)
                                            ////{
                                            //    meterids = words[j].Split(' ');
                                            //    for (int l = 0; l < meterids.Length; l++)
                                            //    {
                                            //        if (l == 4)
                                            //        {
                                            //            string[] PreRedingDate = meterids[l].Split(new string[] { "....................." }, StringSplitOptions.None);
                                            //            CurrentTimestamp = PreRedingDate[0].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                            //            if (PreRedingDate.Length > 1)
                                            //            {
                                            //                CurrentValue = PreRedingDate[1].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                            //            }
                                            //        }
                                            //    }
                                            // }
                                        }

                                        //for (int j = 0, len = words.Length; j < len; j++)
                                        //{
                                        //    if (j == 24 && i == 1)
                                        //    {
                                        //        //string MeterId = words[j];
                                        //        string[] meterids = words[j].Split(' ');
                                        //        for (int k = 0; k < meterids.Length; k++)
                                        //        {
                                        //            if (k == 3)
                                        //            {

                                        //                MeterId = meterids[k];
                                        //            }
                                        //        }
                                        //    }
                                        //    if (j == 26 && i == 1)
                                        //    {
                                        //        string[] meterids = words[j].Split(' ');
                                        //        for (int q = 0; q < meterids.Length; q++)
                                        //        {
                                        //            if (q == 3)
                                        //            {
                                        //                string[] PreRedingDate = meterids[q].Split(new string[] { "....................." }, StringSplitOptions.None);
                                        //                PreviousTimestamp = PreRedingDate[0].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                        //                PreviousReading = PreRedingDate[1].TrimStart(". ".ToCharArray()).TrimEnd(". ".ToCharArray());
                                        //            }
                                        //            if (q == 4)
                                        //            {
                                        //                if (meterids[q] != "State")
                                        //                {
                                        //                    PreviousReading = meterids[q];
                                        //                }
                                        //                //dt.Rows.Add(MeterId, 29, BuildingID, 16, PreviousReading, PreviousTimestamp, 0, PreviousTimestamp, 0.5000);
                                        //                dt.Rows.Add(MeterId, 29, BuildingID, 16, PreviousReading, PreviousTimestamp, 0, PreviousTimestamp, 0.5000);
                                        //                //dt.Rows.Add(MeterId, 29, BuildingID, 16, CurrentValue, CurrentTimestamp, 0, CurrentTimestamp, 0.5000);
                                        //            }
                                        //        }
                                        //    }
                                        //    if (j == 25 && i == 1)
                                        //    {
                                        //        string[] meterids = words[j].Split(' ');
                                        //        for (int l = 0; l < meterids.Length; l++)
                                        //        {
                                        //            if (l == 4)
                                        //            {
                                        //                string[] PreRedingDate = meterids[l].Split(new string[] { "....................." }, StringSplitOptions.None);
                                        //                CurrentTimestamp = PreRedingDate[0].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                        //                if (PreRedingDate.Length > 1)
                                        //                {
                                        //                    CurrentValue = PreRedingDate[1].TrimEnd(". ".ToCharArray()).TrimStart(". ".ToCharArray());
                                        //                }


                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }

                                    dst = dt;
                                }
                                if (fileExtension.ToString().ToLower().Equals(".xml"))
                                {
                                    System.Xml.XmlTextReader xmlreader = new System.Xml.XmlTextReader(fileLocation);
                                    // DataSet ds = new DataSet();
                                    dst.ReadXml(xmlreader);
                                    xmlreader.Close();
                                }
                                DataSet ds = new DataSet();
                                ds = objBAL.BulkAssetExcell(dst);
                                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                                serializerSettings.Converters.Add(new DataTableConverter());
                                jsonData = JsonConvert.SerializeObject(ds, Formatting.None, serializerSettings);
                                resp.success = true;
                                resp.result = jsonData;
                                if (ds.Tables[0].Rows[0]["RowEffrected"].ToString() == "1")
                                {
                                    success = Session["SuccessfulimportExcel"].ToString();
                                }
                                else
                                {
                                    success = Session["SuccessfulimportExcel"].ToString();
                                }
                            }
                        }
                        return Json(success);
                    }
                    catch (Exception ex)
                    {
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json(Session["Nofileselected"].ToString());
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
        }
        private static DataTable RemoveEmptyRows(DataTable source)
        {
            DataTable dt1 = source.Clone(); //copy the structure 
            for (int i = 0; i <= source.Rows.Count - 1; i++) //iterate through the rows of the source
            {
                DataRow currentRow = source.Rows[i];  //copy the current row 
                foreach (var colValue in currentRow.ItemArray)//move along the columns 
                {
                    if (!string.IsNullOrEmpty(colValue.ToString())) // if there is a value in a column, copy the row and finish
                    {
                        dt1.ImportRow(currentRow);
                        break; //break and get a new row     
                        source.Rows[i - 1].Delete();
                    }
                }
            }
            return dt1;
        }
        private static DataTable ProcessCSV(string fileName)
        {
            //Set up our variables
            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            // work out where we should split on comma, but not in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Set the filename in to our stream
            StreamReader sr = new StreamReader(fileName);

            //Read the first line and split the string at , with our regular expression in to an array
            line = sr.ReadLine();
            strArray = r.Split(line);

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

            //Read each line in the CVS file until it’s empty
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();

                //add our current value to our data row
                row.ItemArray = r.Split(line);
                dt.Rows.Add(row);
            }

            //Tidy Streameader up
            sr.Dispose();

            //return a the new DataTable
            return dt;

        }

        public ActionResult SaveUpdateReportSetting(string ReportTypeDaily, string FromDateDaily, string toDateDaily,string ReportTypeMonth, string FromDateMonthly, string toDateMonthly,string ReportTypeYearly, string FromDateYearly, string toDateYearly)
        {
            DataSet ds = new DataSet();
            ClientBAL objBAL = new ClientBAL();
            string jsonData = "";
            ServiceResponse resp = new ServiceResponse();

            if (!string.IsNullOrEmpty(Session["UserID"] as string))
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("UserID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("ReportType", typeof(string)));               
                dt.Columns.Add(new DataColumn("FromDate", typeof(string)));
                dt.Columns.Add(new DataColumn("EndDate", typeof(string)));
                
                dr = dt.NewRow();
                try
                {
                    dt.Rows.Add(Session["UserID"].ToString(), ReportTypeDaily, FromDateDaily, toDateDaily);
                    dt.Rows.Add(Session["UserID"].ToString(), ReportTypeMonth, FromDateMonthly, toDateMonthly);
                    dt.Rows.Add(Session["UserID"].ToString(), ReportTypeYearly,FromDateYearly, toDateYearly);
                    DataSet dst = new DataSet();
                    dst = objBAL.SaveUpdateSetting(dt);
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                    serializerSettings.Converters.Add(new DataTableConverter());
                    jsonData = JsonConvert.SerializeObject(ds, Formatting.None, serializerSettings);
                    resp.success = true;
                    resp.result = jsonData;
                    jsonData = "Sucesses";
                }
                catch (Exception ex)
                {
                    //  string filePath = @"C:\Error.txt";
                    string filePath = Server.MapPath("~/Error/Error.txt");

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                        writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                        jsonData = "Fail";
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "EnergyLevel");
                //RedirectToAction("EnergyLevel", "Login");
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}