using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework___April_23.Data
{
    public class ContributorContribution
    {
        public Contributor Contributor { get; set; }
        public bool Contribute { get; set; }
        public decimal? ContributionAmount { get; set; }
    }
}
