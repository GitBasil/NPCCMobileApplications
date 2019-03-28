using System;
using System.Collections.Generic;
using System.Reflection;
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

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<SpoolJoints> SpoolJoints { get; set; }

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
        public string cSpoolSize { get; set; }
        public string cSpoolMaterial { get; set; }
        public string cRevNo { get; set; }
        public string cForemanUser { get; set; }
        public string cStatus { get; set; }
        public string icon { get; set; }
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

    [Table("SpoolJoints")]
    public class SpoolJoints : IdentifiableModel
    {
        [ForeignKey(typeof(Spools))]
        public int SpoolId { get; set; }

        [ManyToOne]
        public Spools Spool { get; set; }

        public string cProjType { get; set; }
        public int iProjYear { get; set; }
        public int iProjNo { get; set; }
        public string cProjSuffix { get; set; }
        public int iDrwgSrl { get; set; }
        public int iSubDrwgSrl { get; set; }
        public short iJointNo { get; set; }
        public short iJointSerial { get; set; }
        public string cJointSuffix { get; set; }
        public string cCreatedFor { get; set; }
        public string cJointType { get; set; }
        public string cClass { get; set; }
        public decimal rDia { get; set; }
        public decimal rLength { get; set; }
        public decimal rJointThk { get; set; }
        public Nullable<decimal> iWeldLogNo { get; set; }
        public Nullable<DateTime> dWeld { get; set; }
        public string cWPSCode { get; set; }
        public string cJointAreaCode { get; set; }
        public string cMatType { get; set; }

        [TextBlob("cRHWelders")]
        public List<string> cRHWelders { get; set; }

        [TextBlob("cFCWelders")]
        public List<string> cFCWelders { get; set; }
    }

    [Table("UserInfo")]
    public class UserInfo: IdentifiableModel
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public byte[] img { get; set; }
        public string group { get; set; }
    }
}
