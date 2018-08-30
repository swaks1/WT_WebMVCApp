using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WT_WebMVCApp.Helpers
{
    public static class WorkotTrackerHelper
    {        
        public static string ApiUrl = "https://localhost:44333/";
        public static string IdentityServerUrl = "https://localhost:44352/";

        public static int GetUserId(ClaimsPrincipal User)
        {
            //return 2;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            return int.Parse(userId);
        }
    }
}
