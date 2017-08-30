using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnergyDetails.Models
{
    public class ServiceResponse
    {
        public bool success { get; set; }
        public string result { get; set; }
        public string msg { get; set; }
    }
}