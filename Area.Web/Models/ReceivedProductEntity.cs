using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area.Web.Models
{
    public class ReceivedProductEntity
    {
        public int ID { get; set; }
        public string Product { get; set; }
        public string VisitPlaceID { get; set; }
        public string Quantity { get; set; }
        public string Note { get; set; }
    }
}