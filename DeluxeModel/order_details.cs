//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeluxeModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class order_details
    {
        public int order_id { get; set; }
        public Nullable<System.DateTime> order_date { get; set; }
        public string order_prd_name { get; set; }
        public string order_prd_qty { get; set; }
        public string order_prd_price { get; set; }
        public Nullable<int> prd_id { get; set; }
        public Nullable<int> ord_id { get; set; }
    
        public virtual order order { get; set; }
    }
}
