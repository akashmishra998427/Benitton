using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benitton.Models
{
    public class BlogModel
    {

        public Tbl_blog tbl_singleTbl_blog { get; set; } = new Tbl_blog();
        public List<Tbl_blog> Tbl_blogList { get; set; } = new List<Tbl_blog>();
        public List<tbl_product> ProductList { get; set; } = new List<tbl_product>();

         public List<Tbl_subCategory> subcategoryList { get; set; } = new List<Tbl_subCategory>();

    }
}