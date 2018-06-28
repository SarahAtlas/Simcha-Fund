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
    public class ContributorController : Controller
    {
        // GET: Contributor
        public ActionResult Index()
        {
            var cManager = new ContributorManager(Settings.Default.Constr);
            var vm = new ContributorIndexViewModel
            {
                Contributors = cManager.GetContributors()
            };
            vm.TotalBalance = vm.Contributors.Sum(c => c.Balance);
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddContributor(Contributor contributor)
        {
            var cManager = new ContributorManager(Settings.Default.Constr);
            cManager.AddContributor(contributor);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddDeposit(Deposit deposit)
        {
            var cManager = new ContributorManager(Settings.Default.Constr);
            cManager.AddDeposit(deposit);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateContributor(Contributor contributor)
        {
            var cManager = new ContributorManager(Settings.Default.Constr);
            cManager.UpdateContributor(contributor);
            return RedirectToAction("Index");
        }

        public ActionResult History(int contributorId)
        {
            var cManager = new ContributorManager(Settings.Default.Constr);
            var contributors = cManager.GetContributionsByContributorID(contributorId);
            var deposits = cManager.GetDepositByContributorId(contributorId);

            IEnumerable<History> historys = contributors.Select(c => new History
            {
                Action = $"Contribution to {c.SimchaName}",
                Date = c.SimchaDate,
                Amount = c.Amount
            }).Concat(deposits.Select(d => new History
            {
                Action = "Deposit",
                Date = d.Date,
                Amount = d.DepositAmount
            }));

            var vm = new ShowHistoryViewModel
            {
                Historys = historys,
                Contributor = cManager.GetContributorById(contributorId)
            };

            return View(vm);
        }
    }
}