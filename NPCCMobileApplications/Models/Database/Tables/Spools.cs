using System;
using Android.Graphics;
using SQLite;

namespace NPCCMobileApplications.Library
{
    [Table("Spools")]
    public class Spools
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(50)]
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
        public string cNpccDrwgCode { get; set; }
        public string cEngrDrwgCode { get; set; }
        public int iStationId { get; set; }
        public string cStationName { get; set; }
        public int iItemSno { get; set; }
        public string cSpoolNo { get; set; }
        public string cRevNo { get; set; }
        public string cForemanUser { get; set; }
        public string icon { get; set; }
    }

    public class pSpools
    {
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
        public string cNpccDrwgCode { get; set; }
        public string cEngrDrwgCode { get; set; }
        public int iStationId { get; set; }
        public string cStationName { get; set; }
        public int iItemSno { get; set; }
        public string cSpoolNo { get; set; }
        public string cRevNo { get; set; }
        public string cForemanUser { get; set; }
        public string icon { get; set; }
    }
}
