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
    
    public partial class VisitPlaceInfo
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int VisitPlaceID { get; set; }
        public Nullable<int> PlaceCapacity { get; set; }
        public Nullable<int> PeopleInterviewed { get; set; }
        public Nullable<int> PeopleToTasted { get; set; }
        public Nullable<int> PeopleSurvey { get; set; }
        public string PlaceNegativeComment { get; set; }
        public string PlacePositiveComment { get; set; }
        public string CustomerNegativeComment { get; set; }
        public string CustomerPositiveComment { get; set; }
        public string PlaceSaleProduct { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual User User { get; set; }
        public virtual VisitPlace VisitPlace { get; set; }
    }
}