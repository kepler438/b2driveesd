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
    
    public partial class VisitPlace
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VisitPlace()
        {
            this.AvailableUserVisits = new HashSet<AvailableUserVisit>();
            this.PlacePhotoes = new HashSet<PlacePhoto>();
            this.ProductRecivedDelivereds = new HashSet<ProductRecivedDelivered>();
            this.ProductSales = new HashSet<ProductSale>();
            this.VisitPlaceInfoes = new HashSet<VisitPlaceInfo>();
            this.SupervisorVisitPlaces = new HashSet<SupervisorVisitPlace>();
            this.VisitPlaceCarInfoes = new HashSet<VisitPlaceCarInfo>();
            this.VisitPlaceWareHouses = new HashSet<VisitPlaceWareHouse>();
            this.SupervisorVisitPlaces1 = new HashSet<SupervisorVisitPlace>();
        }
    
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<int> PlaceID { get; set; }
        public Nullable<int> RegionID { get; set; }
        public Nullable<int> CheckInfoID { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvailableUserVisit> AvailableUserVisits { get; set; }
        public virtual Place Place { get; set; }
        public virtual PlaceCheckInfo PlaceCheckInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlacePhoto> PlacePhotoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductRecivedDelivered> ProductRecivedDelivereds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSale> ProductSales { get; set; }
        public virtual Region Region { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VisitPlaceInfo> VisitPlaceInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupervisorVisitPlace> SupervisorVisitPlaces { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VisitPlaceCarInfo> VisitPlaceCarInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VisitPlaceWareHouse> VisitPlaceWareHouses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupervisorVisitPlace> SupervisorVisitPlaces1 { get; set; }
    }
}