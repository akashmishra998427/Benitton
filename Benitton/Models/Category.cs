using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Benitton.Models
{
    public class Category
    {
        public int CatId { get; set; }
        public string CatName { get; set; }

        public string image_file { get; set; }

    }
}