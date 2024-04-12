//UserProperty.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySettle.Models
{
    public class UserProperty
    {
        [Key, Column(Order = 0)]
        public string? Email { get; set; }
        
        [Key, Column(Order = 1)]
        public int PropertyID { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Property? Property { get; set; }

        // Additional fields can be added here
    }
}
