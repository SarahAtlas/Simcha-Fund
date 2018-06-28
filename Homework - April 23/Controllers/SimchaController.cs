using Homework___April_23.Data;
using Homework___April_23.Models;
using Homework___April_23.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework___April_23.Controllers
{
    public class SimchaController : Controller
    {
        // GET: Simcha
        public ActionResult Index()
        {
            var cManager = new ContributorManager(Settings.Default.Constr);
            var sManager = new SimchaManager(Settings.Default.Constr);
            var vm = new SimchaIndexViewModel
            {
                Simchos = sManager.GetSimchos(),
                ContributorCount = cManager.GetContributorCount()
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddSimcha(Simcha simcha)
        {

            var sManager = new SimchaManager(Settings.Default.Constr);
            sManager.AddSimcha(simcha);
            return RedirectToAction("Index");
        }

        public ActionResult Contributions(int simchaId)
        {
            var cManager = new ContributorManager(Settings.Default.Constr);
            var sManager = new SimchaManager(Settings.Default.Constr);
            var vm = new ContributionViewModel
            {
                Simcha = sManager.GetSimchaById(simchaId),
                Contributors = cManager.GetContributorContributions(simchaId)
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult UpdateContributions(int simchaId, IEnumerable<ContributorContribution> contributors)
        {
            var sManager = new SimchaManager(Settings.Default.Constr);
            var id = sManager.UpdateContributions(simchaId, contributors);
            return RedirectToAction("/Index");
        }

        public ActionResult EmailOrganizer(int simchaId)
        {
            var cManager = new ContributorManager(Settings.Default.Constr);
            var sManager = new SimchaManager(Settings.Default.Constr);
            var vm = new EmailOrganizerViewModel()
            {
                Contributors = cManager.GetContributorsBySimchaId(simchaId),
                Simcha = sManager.GetSimchaById(simchaId)
            };
            return View(vm);
        }
    }
}