using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using ClientBLL;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Data;

namespace EnergyDetails
{
    public partial class ReportShedual : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //[WebMethod()]
        //public void GetData(string name)
        //{

        //}
        //public DataSet GetHighChart(string Monthnumber, string BuilDID, string LocationID)
        //{
        //    DataSet ds = new DataSet();
        //    ClientBAL objBAL = new ClientBAL();
        //    string jsonData = "";
        //    try
        //    {
        //        if (Monthnumber == "ShadulTime")
        //        {
        //            Monthnumber = "DailyColumn";
        //            BuilDID = "ShadulTime";
        //            //LocationID = Session["LocationIDShadual"].ToString();
        //        }
        //        //if (Monthnumber == "DailyColumn")
        //        //{
        //        //    Session.Add("MonthnumberShadul", Monthnumber);
        //        //    Session["BuilDIDShedual"] = BuilDID;
        //        //    Session["LocationIDShadual"] = LocationID;
        //        //}
        //        // Monthnumber = Session["MonthnumberShadul"].ToString();
        //        //Session["Monthnumber"] = Monthnumber;
        //        //if (BuilDID == "Test" || Flage == true && Session["BuilDID"].ToString() != null)
        //        //{
        //        //    BuilDID = Session["BuilDID"].ToString();
        //        //    Flage = false;
        //        //}
        //        if (LocationID == "")
        //        {
        //            // LocationID = Session["LocationID"].ToString();

        //        }
        //        else
        //        {
        //            Session["BuilDID"] = BuilDID;
        //            //Session["LocationID"] = LocationID;
        //        }
        //        ds = objBAL.GetHighChart(Monthnumber, BuilDID, LocationID);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
        //            serializerSettings.Converters.Add(new DataTableConverter());
        //            jsonData = JsonConvert.SerializeObject(ds, Formatting.None, serializerSettings);
        //        }
        //        else
        //        {
        //            jsonData = "Fail";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonData = "Fail";
        //    }

        //    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //}
    }
}