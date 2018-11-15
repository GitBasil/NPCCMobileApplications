using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

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
                var cn = new SQLiteConnection(dbPath);
                cn.CreateTable<Spools>();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RefreshSpool()
        {
            Console.WriteLine("#################################!!!!!!!!!!!11111111!!!!!!!!!!!!###################3");
            try
            {
                List<Spools> lstObjs = new List<Spools>
                {
                    new Spools{Ename="Nepal", Job="Nepal",icon= "http://edms.npcc.ae/ndms/PublicDocuPreview.aspx?ORIG=TEST-A-PR-001-VER-A0B00.jpg&DocuVersID=2141632&SID=O" },
                    new Spools{Ename="Solomon Sea", Job="May 7, 2015",icon="http://edms.npcc.ae/ndms/PublicDocuPreview.aspx?ORIG=TEST-A-PR-001-VER-A0B00.jpg&DocuVersID=2141632&SID=O"  },
                    new Spools{Ename="Papua New Guinea", Job="May 5, 2015",icon="https://cdn3.volusion.com/vyfsn.knvgw/v/vspfiles/photos/am-3524-2.jpg" },
                    new Spools{Ename="Nepal", Job="April 25, 2015",icon="http://edms.npcc.ae/ndms/PublicDocuPreview.aspx?ORIG=TEST-A-PR-001-VER-A0B00.jpg&DocuVersID=2141632&SID=O" },
                    new Spools{Ename="Taiwan", Job="April 20, 2015",icon="http://edms.npcc.ae/ndms/PublicDocuPreview.aspx?ORIG=TEST-A-PR-001-VER-A0B00.jpg&DocuVersID=2141632&SID=O"  },
                    new Spools{Ename="Papua New Guinea", Job="March 29, 2015",icon="https://cdn3.volusion.com/vyfsn.knvgw/v/vspfiles/photos/am-3524-2.jpg"  },
                    new Spools{Ename="Flores Sea", Job="Febdruary 27, 2015",icon="http://edms.npcc.ae/ndms/PublicDocuPreview.aspx?ORIG=TEST-A-PR-001-VER-A0B00.jpg&DocuVersID=2141632&SID=O" },
                    new Spools{Ename="Mid-Atlantic range", Job="Febdruary 13, 2015",icon="http://edms.npcc.ae/ndms/PublicDocuPreview.aspx?ORIG=TEST-A-PR-001-VER-A0B00.jpg&DocuVersID=2141632&SID=O"  }
                };
                var cn = new SQLiteConnection(dbPath);
                cn.DeleteAll<Spools>();
                cn.InsertAll(lstObjs);
                Console.WriteLine("#################################!!!!!!!!!!!222222!!!!!!!!!!!!###################3");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("#################################!!!!!!!!!!!3333333!!!!!!!!!!!!###################3");
                return false;
            }
        }

        public List<Spools> GetSpools()
        {
            try
            {
                var cn = new SQLiteConnection(dbPath);
                var Spools = cn.Table<Spools>().ToList();
                return Spools;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Spools GetSpool(int id)
        {
            //var emp = cn.Get<Spools>(id);
            var cn = new SQLiteConnection(dbPath);
            var spl = cn.Table<Spools>().Where(x => x.Id == id).FirstOrDefault();
            return spl;
        }

        public bool UpdateSpool(int id, Spools nSpool)
        {
            try
            {
                var cn = new SQLiteConnection(dbPath);
                var spl = cn.Get<Spools>(id);
                spl.Ename = nSpool.Ename;
                spl.Salary = nSpool.Salary;
                spl.Job = nSpool.Job;
                spl.icon = nSpool.icon;
                cn.Update(spl);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool DeleteSpool(int id)
        {
            try
            {
                var cn = new SQLiteConnection(dbPath);
                //cn.Delete<Spools>(id);
                var spl = cn.Get<Spools>(id);
                cn.Delete(spl);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
