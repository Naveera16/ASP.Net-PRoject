//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace online_sms_site.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class credit_card
    {
        public int id { get; set; }
        public Nullable<int> user_Id { get; set; }
        public Nullable<long> card_no { get; set; }
        public string holder_name { get; set; }
        public string expiry_date { get; set; }
        public Nullable<int> CVV { get; set; }
        public string email { get; set; }
        public Nullable<int> amount { get; set; }
        public Nullable<int> activated_service { get; set; }
    
        public virtual user_required user_required { get; set; }
        public virtual user_required user_required1 { get; set; }
    }
}
