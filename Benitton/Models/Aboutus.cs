using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benitton.Models
{
    public class Aboutus
    {
        public about_us about_usSingleRec { get; set; } = new about_us();
        public about_ourvision about_ourvisionSinglrRec { get; set; } = new about_ourvision();
        public List<about_ourvalues> ourvaluesList { get; set; } = new List<about_ourvalues>();
        public List<about_certification> certificationList { get; set; } = new List<about_certification>();
    }
}