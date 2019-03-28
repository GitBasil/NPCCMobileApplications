using System;
using System.Collections.Generic;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Library
{
    public class inf_project_info
    {
        string cProjType { get; set; }
        int cProjYear { get; set; }
        int cProjNo { get; set; }
        string cProjSuffix { get; set; }
        string cProjDesc { get; set; }
    }

    public class inf_login_info
    {
        public inf_login_result Authenticated { get; set; }
        public string Token { get; set; }
    }

    public class inf_credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class inf_userinfo
    {
        public string cUsername { get; set; }
        public string cFullName { get; set; }
    }

    public class inf_assignment
    {
        public string cFabricatorUser { get; set; }
        public int iAssignmentId { get; set; }
    }

    public class inf_spool_list_type
    {
        public inf_assignment_type assignment_type { get; set; }
        public string group { get; set; }
    }

    public class inf_JointWPS
    {
        public string cWPSCode { get; set; }
        public string cWeldType { get; set; }
        public string cQualRefKey { get; set; }
        public string cQualRefDesc { get; set; }
        public string cJointType { get; set; }
        public string cClasses { get; set; }
        public decimal rDiaMin { get; set; }
        public decimal rDiaMax { get; set; }
        public decimal rThkMin { get; set; }
        public decimal rThkMax { get; set; }
    }

    public class inf_JointWelder
    {
        public string cBadgeNo { get; set; }
        public string cName { get; set; }
    }

    public class inf_ReturnStatus
    {
        public bool status { get; set; }
        public string msg { get; set; }
    }

    public class inf_SpoolJoints
    {
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
        public List<string> cRHWelders { get; set; }
        public List<string> cFCWelders { get; set; }
    }
}
