using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Extensions;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Library
{
    public class DBRepository
    {
        string dbPath;
        public DBRepository()
        {
            dbPath = Path.Combine(System.Environment.GetFolderPath
                                  (System.Environment.SpecialFolder.Personal), "ormdemo.db");
        }

        public bool CreateTable()
        {
            try
            {
                using (var cn = new SQLiteConnection(dbPath))
                {
                    cn.CreateTable<Spools>();
                    cn.CreateTable<SpoolItem>();
                    cn.CreateTable<UserInfo>();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region UserInfo

        public async Task<bool> RefreshUserInfoAsync()
        {
            try
            {
                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/UserImage";
                UserInfo lstObjs = await npcc_services.inf_CallWebServiceAsync<UserInfo, string>(inf_method.Get, url);
                using (var cn = new SQLiteConnection(dbPath))
                {
                    cn.DeleteAll<UserInfo>();
                    cn.Insert(lstObjs);
                }

                return true;
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
                return false;
            }
        }

        public UserInfo GetUserInfo()
        {
            try
            {
                UserInfo lstObjs;
                using (var cn = new SQLiteConnection(dbPath))
                {
                    lstObjs = cn.Table<UserInfo>().FirstOrDefault();
                }

                return lstObjs;
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
                return null;
            }
        }

        #endregion

        #region Spools

        public async Task<bool> RefreshSpoolAsync()
        {
            try
            {
                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/pendinglist";
                List<Spools> lstObjs = await npcc_services.inf_CallWebServiceAsync<List<Spools>, string>(inf_method.Get, url);
                using (var cn = new SQLiteConnection(dbPath))
                {
                    cn.DeleteAll<Spools>();
                    cn.InsertAllWithChildren(lstObjs);
                }
                return true;
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
                return false;
            }
        }

        public List<Spools> GetSpools()
        {
            try
            {
                List<Spools> Spools;
                using (var cn = new SQLiteConnection(dbPath))
                {
                    Spools = cn.GetAllWithChildren<Spools>();
                }

                return Spools;
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
                return null;
            }
        }

        public Spools GetSpool(int id)
        {
            //var emp = cn.Get<Spools>(id);
            Spools spl;
            using (var cn = new SQLiteConnection(dbPath))
            {
                spl = cn.Table<Spools>().Where(x => x.Id == id).FirstOrDefault();
            }
            return spl;
        }

        public bool UpdateSpool(int id, Spools nSpool)
        {
            try
            {
                using (var cn = new SQLiteConnection(dbPath))
                {
                    Spools spl = cn.Get<Spools>(id);
                    spl.cSpoolNo = nSpool.cSpoolNo;
                    spl.cStationName = nSpool.cStationName;
                    spl.icon = nSpool.icon;
                    cn.Update(spl);
                }
                return true;
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
                return false;
            }

        }

        public bool DeleteSpool(int id)
        {
            try
            {
                using (var cn = new SQLiteConnection(dbPath)) {
                    var spl = cn.Get<Spools>(id);
                    cn.Delete(spl);
                }
                return true;
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
                return false;
            }
        }

        #endregion

    }
}
