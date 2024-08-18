using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Benitton.Models
{   
    public class ViewProducts
    {
        [AllowHtml]
        public string Resume { get; set; }
        public tbl_product tbl_singleProduct { get; set; } = new tbl_product();
        public about_us tbl_singleAboutus { get; set; } = new about_us();
        public List<Tbl_banner> tbl_bannerList { get; set; } = new List<Tbl_banner>();
        public tbl_category tbl_singleCategory { get; set; } = new tbl_category();
        public Tbl_subCategory tbl_singleSubCategory { get; set; } = new Tbl_subCategory();
        public List<tbl_category> categoryMd { get; set; } = new List<tbl_category>();
        public List<Tbl_subCategory> subcategoryMd { get; set; } = new List<Tbl_subCategory>();
        public List<tbl_product> ProductMd { get; set; } = new List<tbl_product>();
        public List<tbl_Menu> MenuMd { get; set; } = new List<tbl_Menu>();
        public tbl_Menu tbl_singleMenu { get; set; } = new tbl_Menu();
        public List<about_certification> certificationList { get; set; } = new List<about_certification>();


    }
}