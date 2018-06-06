//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Area.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductRecivedDelivered
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> VisitPlaceWareHouseID { get; set; }
        public Nullable<int> SupervisorVisitPlaceID { get; set; }
        public Nullable<byte> ProductType { get; set; }
        public int VisitPlaceID { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> IsDeliveryWareHouse { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual VisitPlace VisitPlace { get; set; }
        public virtual SupervisorVisitPlace SupervisorVisitPlace { get; set; }
        public virtual VisitPlaceWareHouse VisitPlaceWareHouse { get; set; }
    }
}
