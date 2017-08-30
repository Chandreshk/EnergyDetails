using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientDLL;
using System.Data;
using System.Data.SqlClient;

namespace ClientDLL
{
    public class ClientDAL
    {
        public DataSet GetHighChart(string Monthnumber, string BuilDID, string LocationID,int UserId)
        {
            try
            {

                if (Monthnumber == "Energy")
                {
                    SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@MonthNumber", Monthnumber),
                    new SqlParameter("@BuilAutBinID", BuilDID),
                     new SqlParameter("@LocationID", LocationID),
                     new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetDetailsCampared, P);
                }
               else if (Monthnumber == "Gas")
                {
                    SqlParameter[] P = new SqlParameter[] {
                     new SqlParameter("@LocationID", LocationID),
                     new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetGasCampared, P);

                }
                else if (Monthnumber == "EnergyYearly")
                {
                    SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@MonthNumber", Monthnumber),
                    new SqlParameter("@BuilAutBinID", BuilDID),
                     new SqlParameter("@LocationID", LocationID),
                     new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetDetailsCamparedYearly, P);
                }
                else if (Monthnumber == "EnergyYearlyIntensity")
                {
                    SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@MonthNumber", Monthnumber),
                    new SqlParameter("@BuilAutBinID", BuilDID),
                     new SqlParameter("@LocationID", LocationID),
                      new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetDetailsIntensity, P);
                }
                else if (Monthnumber == "DailyColumn")
                {
                    SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@MonthNumber", Monthnumber),
                    new SqlParameter("@BuilAutBinID", BuilDID),
                      new SqlParameter("@LocationID", LocationID),
                        new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetDetailsDailyColumnGraph, P);
                }
                else if (Monthnumber == "Weather")
                {
                    SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@MonthNumber", Monthnumber),
                    new SqlParameter("@BuilAutBinID", BuilDID),
                      new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetWetherAdjustedReport, P);
                }
                else if (Monthnumber == "PeekDemand")
                {
                    SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@MonthNumber", Monthnumber),
                    new SqlParameter("@BuilAutBinID", BuilDID),
                     new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.getMonthlyPeekDemand, P);
                }
                else if (Monthnumber == "MaxPeekDemand")
                {
                    SqlParameter[] P = new SqlParameter[] {
                      new SqlParameter("@LocationID", LocationID),
                     new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.getMonthlySumPeak, P);
                }
                else if (Monthnumber == "MonthlyGas")
                {
                    SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@BuilAutBinID", BuilDID),
                    new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.getMonthlyGas, P);
                }
                else
                {
                    SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@MonthNumber", Monthnumber),
                    new SqlParameter("@BuilAutBinID", BuilDID),
                     new SqlParameter("@UserId", UserId),
                };
                    return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetHighChart, P);
                }
            }

            catch
            {
                throw;
            }
        }

        public DataSet GetfacilityData()
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {

                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetFacilityData, P);
            }

            catch
            {
                throw;
            }
        }
        public DataSet GetOrgfacilityData()
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {

                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetOrgFacilityData, P);
            }

            catch
            {
                throw;
            }
        }
        public DataSet UpdatedFacilites(DataTable dt)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                     new SqlParameter("@AddFacilites", dt)

                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.AddNewFacilites, P);
            }

            catch
            {
                throw;
            }
        }
        public DataSet BulkAssetExcel(DataTable dt)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                     new SqlParameter("@BulkExcelUpload", dt)

                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.BulkExcelDataImport, P);
            }

            catch (Exception ex)
            {
                throw;
            }
        }
        public DataSet SaveUpdateSetting(DataTable dt)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                     new SqlParameter("@IZDateSettingdt", dt)

                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.SetDateSetting, P);
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet GetLogin(string UserName, string Password)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                new SqlParameter("@UserName", UserName),
                 new SqlParameter("@Password", Password)

                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetLogin, P);
            }

            catch
            {
                throw;
            }
        }
        public DataSet UpdatedLocation(DataTable dt)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                     new SqlParameter("@AddLocation", dt)

                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.AddNewLocation, P);
            }

            catch (Exception ex)
            {
                throw;
            }
        }
        public DataSet GetSiteData(string FacilityID, string LocationID,string UserId)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                new SqlParameter("@FacilityID", FacilityID),
                 new SqlParameter("@LocationID", LocationID),
                  new SqlParameter("@UserId", UserId)

                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetSiteData, P);
            }

            catch(Exception ex)
            {
                throw;
            }
        }
        public DataSet GetBulidingDetails(string FacilityID, string LocationID)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@FacilityID", FacilityID),
                    new SqlParameter("@LocationID", LocationID),
                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetBuildingDetails, P);
            }

            catch
            {
                throw;
            }
        }
        public string DataShrink()
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                };
                return DataLib.GetScalarStringStoredProcData(DataLib.Connection.ConnectionString, SpKeys.DataShrink, P);
            }

            catch
            {
                throw;
            }
        }
        public DataSet GetUSerDetails()
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                    //new SqlParameter("@FacilityID", FacilityID),
                    //new SqlParameter("@LocationID", LocationID),
                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetEmailUSerDetails, P);
            }

            catch
            {
                throw;
            }
        }
        public DataSet GetSiteDetails(string FacilityID)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@FacilityID", FacilityID),
                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.GetSiteDetails, P);
            }

            catch
            {
                throw;
            }
        }
        public DataSet GetUpdatedSiteDetails(string FacilityID)
        {
            try
            {
                SqlParameter[] P = new SqlParameter[] {
                    new SqlParameter("@FacilityID", FacilityID),
                };
                return DataLib.GetStoredProcData(DataLib.Connection.ConnectionString, SpKeys.UpdatedSiteDetails, P);
            }

            catch
            {
                throw;
            }
        }
    }
}
