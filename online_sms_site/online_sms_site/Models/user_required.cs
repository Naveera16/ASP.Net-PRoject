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
    
    public partial class user_required
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user_required()
        {
            this.friend_list = new HashSet<friend_list>();
            this.friend_list1 = new HashSet<friend_list>();
            this.Friend_Request = new HashSet<Friend_Request>();
            this.Friend_Request1 = new HashSet<Friend_Request>();
            this.Messages = new HashSet<Messages>();
            this.user_personal = new HashSet<user_personal>();
            this.user_professional = new HashSet<user_professional>();
            this.Contact = new HashSet<Contact>();
            this.texting = new HashSet<texting>();
            this.credit_card = new HashSet<credit_card>();
            this.credit_card1 = new HashSet<credit_card>();
        }
    
        public int Id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public Nullable<long> phone_no { get; set; }
        public string password { get; set; }
        public Nullable<bool> active_status { get; set; }
        public string time { get; set; }
        public Nullable<bool> account_status { get; set; }
        public string service_activate { get; set; }
        public string lastseen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<friend_list> friend_list { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<friend_list> friend_list1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Friend_Request> Friend_Request { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Friend_Request> Friend_Request1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messages> Messages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_personal> user_personal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_professional> user_professional { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contact { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<texting> texting { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<credit_card> credit_card { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<credit_card> credit_card1 { get; set; }
    }
}
