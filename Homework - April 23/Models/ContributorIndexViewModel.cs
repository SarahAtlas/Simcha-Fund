using Homework___April_23.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework___April_23.Models
{
    public class ContributorIndexViewModel
    {
        public IEnumerable<Contributor> Contributors { get; set; }
        public decimal TotalBalance { get; set; }
    }
}