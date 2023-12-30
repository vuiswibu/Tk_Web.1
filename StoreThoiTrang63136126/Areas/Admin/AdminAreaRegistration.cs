using System.Web.Mvc;

namespace StoreThoiTrang63136126.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home63136126", action = "Index", id = UrlParameter.Optional },
                new[] { "StoreThoiTrang63136126.Areas.Admin.Controllers" }
            );
        }
    }
}