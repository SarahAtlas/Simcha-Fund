using Homework___April_23.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework___April_23.Models
{
    public class ShowHistoryViewModel
    {
        public IEnumerable<History> Historys { get; set; }
        public Contributor Contributor { get; set; }
    }
}