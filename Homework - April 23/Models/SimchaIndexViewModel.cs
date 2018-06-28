using Homework___April_23.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework___April_23.Models
{
    public class SimchaIndexViewModel
    {
        public IEnumerable<Simcha> Simchos { get; set; }
        public int ContributorCount { get; set; }
    }
}