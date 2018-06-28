using Homework___April_23.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework___April_23.Models
{
    public class EmailOrganizerViewModel
    {
        public IEnumerable<Contributor> Contributors { get; set; }
        public Simcha Simcha { get; set; }
    }
}