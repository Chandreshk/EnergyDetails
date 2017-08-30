using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientDLL;
using System.Data;

namespace ClientBLL
{
    public class ClientBAL
    {
        public DataSet GetHighChart(string Monthnumber,string BuilDID, string LocationID,int UserId)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetHighChart(Monthnumber, BuilDID, LocationID, UserId);
            }
            catch
            {
                throw;
            }
        }
        public DataSet Getfacicility()
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetfacilityData();
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetOrgfacilityData()
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetOrgfacilityData();
            }
            catch
            {
                throw;
            }
        }
        public DataSet UpdatedFacilites(DataTable dt)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.UpdatedFacilites(dt);
            }
            catch
            {
                throw;
            }
        }
        public DataSet BulkAssetExcell(DataTable dt)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.BulkAssetExcel(dt);
            }
            catch
            {
                throw;
            }
        }
        public DataSet SaveUpdateSetting(DataTable dt)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.SaveUpdateSetting(dt);
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetLoginDetails(string UserName, string Password)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetLogin(UserName, Password);
            }
            catch
            {
                throw;
            }
        }
        public DataSet UpdatedLocation(DataTable dt)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.UpdatedLocation(dt);
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetSiteDetails(string FacilityID, string LocationID,string UserId)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetSiteData(FacilityID, LocationID, UserId);
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetBulidingDetails(string FacilityID, string LocationID)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetBulidingDetails(FacilityID, LocationID);
            }
            catch
            {
                throw;
            }
        }
        public string DataShrink()
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.DataShrink();
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetUSerDetails()
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetUSerDetails();
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetsiteDetails(string FacilityID)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetSiteDetails(FacilityID);
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetUpdatedsiteDetails(string FacilityID)
        {
            ClientDAL objDal = new ClientDAL();
            try
            {
                return objDal.GetUpdatedSiteDetails(FacilityID);
            }
            catch
            {
                throw;
            }
        }

    }
}
