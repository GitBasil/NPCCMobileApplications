using System;
using System.Collections.Generic;
using System.IO;
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

        public bool DropTable()
        {
            try
            {
                using (var cn = new SQLiteConnection(dbPath))
                {
                    cn.DropTable<Spools>();
                    cn.DropTable<SpoolItem>();
                    cn.DropTable<SpoolJoints>();
                    cn.DropTable<UserInfo>();
                }

                return true;
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
                return false;
            }
        }


        #region UserInfo

        public async Task<bool> RefreshUserInfoAsync()
        {
            try
            {
                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/UserInfo";
                UserInfo lstObjs = await npcc_services.inf_CallWebServiceAsync<UserInfo, string>(inf_method.Get, url);
                using (var cn = new SQLiteConnection(dbPath))
                {
                    cn.CreateTable<UserInfo>();
                    cn.DeleteAll<UserInfo>();
                    cn.Insert(lstObjs);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

        public async Task<bool> RefreshSpoolAsync(inf_assignment_type assignment_Type)
        {
            try
            {
                string url;
                List<Spools> lstObjs;
                inf_spool_list_type obj_spool_list_type = new inf_spool_list_type();
                DBRepository dBRepository = new DBRepository();
                UserInfo user = dBRepository.GetUserInfo();
                obj_spool_list_type.group = user.group;

                using (var cn = new SQLiteConnection(dbPath))
                {
                    cn.CreateTable<SpoolJoints>();
                    cn.CreateTable<SpoolItem>();
                    cn.CreateTable<Spools>();
                }
                switch (assignment_Type)
                {
                    case inf_assignment_type.Pending:
                        Console.WriteLine("####################");
                        obj_spool_list_type.assignment_type = inf_assignment_type.Pending;

                        url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/SpoolsList";
                        lstObjs = await npcc_services.inf_CallWebServiceAsync<List<Spools>, inf_spool_list_type>(inf_method.Post, url, obj_spool_list_type);
                        Console.WriteLine("####################" + lstObjs.Count + "#########" + obj_spool_list_type.group);
                        using (var cn = new SQLiteConnection(dbPath))
                        {
                            cn.DeleteAll(cn.GetAllWithChildren<Spools>(x => x.cStatus == "P"));
                            cn.InsertAllWithChildren(lstObjs);
                        }

                        break;
                    case inf_assignment_type.UnderFabrication:
                        obj_spool_list_type.assignment_type = inf_assignment_type.UnderFabrication;

                        url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/SpoolsList";
                        lstObjs = await npcc_services.inf_CallWebServiceAsync<List<Spools>, inf_spool_list_type>(inf_method.Post, url, obj_spool_list_type);
                        using (var cn = new SQLiteConnection(dbPath))
                        {
                            cn.DeleteAll(cn.GetAllWithChildren<Spools>(x => x.cStatus == "F"));
                            cn.InsertAllWithChildren(lstObjs);
                        }

                        break;
                    case inf_assignment_type.UnderWelding:
                        obj_spool_list_type.assignment_type = inf_assignment_type.UnderWelding;

                        url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/SpoolsList";
                        lstObjs = await npcc_services.inf_CallWebServiceAsync<List<Spools>, inf_spool_list_type>(inf_method.Post, url, obj_spool_list_type);
                        using (var cn = new SQLiteConnection(dbPath))
                        {
                            cn.DeleteAll(cn.GetAllWithChildren<Spools>(x => x.cStatus == "W"));
                            cn.InsertAllWithChildren(lstObjs);
                        }

                        break;
                    case inf_assignment_type.Completed:
                        obj_spool_list_type.assignment_type = inf_assignment_type.Completed;

                        url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/SpoolsList";
                        lstObjs = await npcc_services.inf_CallWebServiceAsync<List<Spools>, inf_spool_list_type>(inf_method.Post, url, obj_spool_list_type);
                        using (var cn = new SQLiteConnection(dbPath))
                        {
                            cn.DeleteAll(cn.GetAllWithChildren<Spools>(x => x.cStatus == "C"));
                            cn.InsertAllWithChildren(lstObjs);
                        }

                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("########"+ ex.Message + "############");
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
                return false;
            }
        }

        public List<Spools> GetSpools(inf_assignment_type assignment_Type)
        {
            try
            {
                using (var cn = new SQLiteConnection(dbPath))
                {
                    cn.CreateTable<SpoolJoints>();
                    cn.CreateTable<SpoolItem>();
                    cn.CreateTable<Spools>();
                }

                Console.WriteLine("############S#################");
                List<Spools> lstObjs;
                switch (assignment_Type)
                {
                    case inf_assignment_type.Pending:
                        using (var cn = new SQLiteConnection(dbPath))
                        {
                            lstObjs = cn.GetAllWithChildren<Spools>(x => x.cStatus == "P");
                        }
                        Console.WriteLine("############P#################");
                        return lstObjs;
                    case inf_assignment_type.UnderFabrication:
                        using (var cn = new SQLiteConnection(dbPath))
                        {
                            lstObjs = cn.GetAllWithChildren<Spools>(x => x.cStatus == "F");
                        }
                        Console.WriteLine("#############F################");
                        return lstObjs;
                    case inf_assignment_type.UnderWelding:
                        using (var cn = new SQLiteConnection(dbPath))
                        {
                            lstObjs = cn.GetAllWithChildren<Spools>(x => x.cStatus == "W");
                        }
                        Console.WriteLine("##############W###############");
                        return lstObjs;
                    case inf_assignment_type.Completed:
                        using (var cn = new SQLiteConnection(dbPath))
                        {
                            lstObjs = cn.GetAllWithChildren<Spools>(x => x.cStatus == "C");
                        }
                        Console.WriteLine("############C#################");
                        return lstObjs;
                    default:
                        Console.WriteLine("############N#################");
                        return null;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("############E#################");
                Console.WriteLine("############" + ex.Message + "#################");
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

        public bool UpdateSpool(Spools nSpool)
        {
            try
            {
                using (var cn = new SQLiteConnection(dbPath))
                {
                    cn.Update(nSpool);
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
