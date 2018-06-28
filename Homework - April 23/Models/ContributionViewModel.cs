using Homework___April_23.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework___April_23.Models
{
    public class ContributionViewModel
    {
        public Simcha Simcha { get; set; }
        public IEnumerable<ContributorContribution> Contributors { get; set; }
    }
}