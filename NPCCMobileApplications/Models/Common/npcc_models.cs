using System;
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
}
