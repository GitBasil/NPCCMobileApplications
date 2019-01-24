using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Graphics;
using Java.Lang;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace NPCCMobileApplications.Library
{
    public abstract class IdentifiableModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }

    [Table("Spools")]
    public class Spools: IdentifiableModel
    {
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<SpoolItem> SpoolItem { get; set; }

        public int iAssignmentId { get; set; }
        public string cProjType { get; set; }
        public int iProjYear { get; set; }
        public int iProjNo { get; set; }
        public string cProjSuffix { get; set; }
        public int iProjectId { get; set; }
        public string cProjDesc { get; set; }
        public string cTransmittal { get; set; }
        public DateTime dTransmittal { get; set; }
        public int iDrwgSrl { get; set; }
        public string cISO { get; set; }
        public int iStationId { get; set; }
        public string cStationName { get; set; }
        public int iItemSno { get; set; }
        public string cSpoolNo { get; set; }
        public string cRevNo { get; set; }
        public string cForemanUser { get; set; }
        public string icon { get; set; }

        public static explicit operator Spools(Java.Lang.Object v)
        {
            throw new NotImplementedException();
        }

        public static explicit operator Spools(PropertyInfo v)
        {
            throw new NotImplementedException();
        }
    }

    [Table("SpoolItem")]
    public class SpoolItem: IdentifiableModel
    {
        [ForeignKey(typeof(Spools))]
        public int SpoolId { get; set; }

        [ManyToOne]
        public Spools Spool { get; set; }

        public string cMatType { get; set; }
        public string cVocab { get; set; }
        public string cClassCode { get; set; }
        public string cColorCode { get; set; }
    }

    [Table("UserInfo")]
    public class UserInfo: IdentifiableModel
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public byte[] img { get; set; }

        public static explicit operator UserInfo(Java.Lang.Object v)
        {
            throw new NotImplementedException();
        }

        public static explicit operator UserInfo(PropertyInfo v)
        {
            throw new NotImplementedException();
        }
    }


}
