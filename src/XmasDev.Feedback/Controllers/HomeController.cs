using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using System.Web.Mvc;
using XmasDev.Feedback.Models;

namespace XmasDev.Feedback.Controllers
{
    public class HomeController : Controller
    {
        private readonly CrmServiceClient m_client;

        public HomeController()
        {
            m_client = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString);
        }

        public ActionResult Index(string user, string product)
        {
            var model = new SaveFeedbackModel
            {
                UserCode = user ?? "Not found",
                ProductCode = product ?? "Not found",
                None = true
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveFeedback(SaveFeedbackModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Entity("xms_feedback");
                entity["xms_productcode"] = model.ProductCode;
                entity["xms_usercode"] = model.UserCode;
                entity["xms_rating"] = model.Rating;
                m_client.Create(entity);

                model.None = false;
                model.Success = true;
            }

            return View("Index", model);
        }
    }
}