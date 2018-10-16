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

}
