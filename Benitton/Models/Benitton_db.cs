﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class about_certification
{
    public int ID { get; set; }
    public string image_file { get; set; }
    public bool Isactive { get; set; }
    public System.DateTime created_at { get; set; }
}

public partial class about_ourvalues
{
    public int ID { get; set; }
    public string title { get; set; }
    public string sort_description { get; set; }
    public bool Isactive { get; set; }
    public System.DateTime created_at { get; set; }
    public string image_file { get; set; }
}

public partial class about_ourvision
{
    public int ID { get; set; }
    public string title1 { get; set; }
    public string sort_description1 { get; set; }
    public string image_1 { get; set; }
    public string title2 { get; set; }
    public string sort_description2 { get; set; }
    public string image_2 { get; set; }
    public System.DateTime created_at { get; set; }
    public string image_file { get; set; }
}

public partial class about_us
{
    public int ID { get; set; }
    public string sort_description_home { get; set; }
    public string full_description { get; set; }
    public string about_image { get; set; }
    public string about_image_home { get; set; }
    public System.DateTime created_at { get; set; }
}

public partial class contact_detail
{
    public int ID { get; set; }
    public string sort_description { get; set; }
    public string contact_no { get; set; }
    public string email { get; set; }
    public string office_address { get; set; }
    public System.DateTime created_at { get; set; }
}

public partial class contact_us
{
    public int contact_id { get; set; }
    public string organisation_name { get; set; }
    public string contact_name { get; set; }
    public string mobile { get; set; }
    public string email { get; set; }
    public string message_detail { get; set; }
    public System.DateTime created_at { get; set; }
}

public partial class product_enq
{
    public int ID { get; set; }
    public string organisation_name { get; set; }
    public string contact_name { get; set; }
    public string mobile { get; set; }
    public string email { get; set; }
    public string product_title { get; set; }
    public string product_url { get; set; }
    public string message_detail { get; set; }
    public System.DateTime created_at { get; set; }
    public string state { get; set; }
}

public partial class sale_support
{
    public int complaint_id { get; set; }
    public string equipment_make { get; set; }
    public long serial_no { get; set; }
    public string customer_company { get; set; }
    public string contact_person { get; set; }
    public string email { get; set; }
    public string problem_type { get; set; }
    public string urgency_level { get; set; }
    public string equipment_model { get; set; }
    public string meter_radio { get; set; }
    public string meter_number { get; set; }
    public string address { get; set; }
    public string mobile { get; set; }
    public string nature_problem { get; set; }
    public string position { get; set; }
    public string complaint_number { get; set; }
    public string equipment_description { get; set; }
    public bool status { get; set; }
    public System.DateTime created_at { get; set; }
    public string attachment { get; set; }
    public string ticket_number { get; set; }
}

public partial class tbl_admin
{
    public int id { get; set; }
    public string name { get; set; }
    public string emailid { get; set; }
    public string password { get; set; }
    public string mobile_no { get; set; }
    public System.DateTime created_at { get; set; }
    public string user_type { get; set; }
}

public partial class Tbl_banner
{
    public int ID { get; set; }
    public string title { get; set; }
    public string image_file { get; set; }
    public Nullable<bool> Isactive { get; set; }
    public System.DateTime created_at { get; set; }
    public string Subtitle { get; set; }
}

public partial class Tbl_blog
{
    public int ID { get; set; }
    public string title { get; set; }
    public string image_file { get; set; }
    public string detail { get; set; }
    public Nullable<bool> Isactive { get; set; }
    public Nullable<bool> display_on_mainpage { get; set; }
    public System.DateTime created_at { get; set; }
}

public partial class tbl_career
{
    public int career_id { get; set; }
    public string full_name { get; set; }
    public string mobile { get; set; }
    public string email { get; set; }
    public string enquiry_for { get; set; }
    public string msg_detail { get; set; }
    public string attach_resume { get; set; }
    public System.DateTime created_at { get; set; }
}

public partial class tbl_category
{
    public int ID { get; set; }
    public int MenuID { get; set; }
    public string title { get; set; }
    public string detail { get; set; }
    public Nullable<bool> Isactive { get; set; }
    public System.DateTime created_at { get; set; }
    public string image_file { get; set; }
    public bool Isdelete { get; set; }
}

public partial class Tbl_Contact
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public Nullable<System.DateTime> CreatedDate { get; set; }
}

public partial class Tbl_faq
{
    public int ID { get; set; }
    public string title { get; set; }
    public string detail { get; set; }
    public Nullable<bool> Isactive { get; set; }
    public System.DateTime created_at { get; set; }
}

public partial class tbl_Menu
{
    public int ID { get; set; }
    public string title { get; set; }
    public Nullable<int> MenuOrder { get; set; }
    public Nullable<bool> Isactive { get; set; }
    public System.DateTime created_at { get; set; }
    public string image_file { get; set; }
}

public partial class tbl_product
{
    public int ID { get; set; }
    public int CategoryID { get; set; }
    public string Title { get; set; }
    public int Qty { get; set; }
    public int Price { get; set; }
    public string Detail { get; set; }
    public string Image_1 { get; set; }
    public string Image_2 { get; set; }
    public string Image_3 { get; set; }
    public string Image_4 { get; set; }
    public bool Isactive { get; set; }
    public System.DateTime created_at { get; set; }
    public Nullable<int> SubCategoryID { get; set; }
    public bool display_on_home { get; set; }
    public string Image_5 { get; set; }
    public string Image_6 { get; set; }
    public string Image_7 { get; set; }
    public string Image_8 { get; set; }
    public string salient_features { get; set; }
    public string specifications { get; set; }
    public string pdf_image { get; set; }
    public string sale_status { get; set; }
    public bool Isdelete { get; set; }
    public Nullable<int> serialorder { get; set; }
    public Nullable<int> otherCatID { get; set; }
    public string video_iframe { get; set; }
    public string optional_ddetail { get; set; }
    public string home_image { get; set; }
    public string Meta_Title { get; set; }
    public string Meta_Keywords { get; set; }
    public string Meta_Canical { get; set; }
    public string Meta_description { get; set; }
}

public partial class Tbl_saleSupportBannes
{
    public int ID { get; set; }
    public string image_file { get; set; }
    public Nullable<bool> Isactive { get; set; }
    public System.DateTime created_at { get; set; }
}

public partial class Tbl_subCategory
{
    public int ID { get; set; }
    public int CategoryID { get; set; }
    public string title { get; set; }
    public string image_file { get; set; }
    public string detail { get; set; }
    public Nullable<bool> Isactive { get; set; }
    public bool display_on_home { get; set; }
    public System.DateTime created_at { get; set; }
    public bool Isdelete { get; set; }
    public Nullable<int> serialorder { get; set; }
}

public partial class tbl_subscribe
{
    public int ID { get; set; }
    public string Emailid { get; set; }
    public System.DateTime CreatedAt { get; set; }
}

public partial class tbl_testimonial
{
    public int ID { get; set; }
    public string image_file { get; set; }
    public bool Isactive { get; set; }
    public System.DateTime created_at { get; set; }
}
