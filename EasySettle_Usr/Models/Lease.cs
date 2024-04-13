using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySettle.Models
{
    public class Lease
    {
        
        public int LeaseID { get; set; }
        public decimal DepositPaid { get; set; }
        public DateTime RentStart { get; set; }
        public DateTime RentFinish { get; set; }
        //[ForeignKey("Property")]
        public int PropertyID { get; set; }
        //[ForeignKey("ClientNo")]
        public int ClientID { get; set; }

        // Navigation property for one-to-one relationship with Client
        public virtual Property? Property { get; set; }
        public virtual Client? Client { get; set; }
    }
}
