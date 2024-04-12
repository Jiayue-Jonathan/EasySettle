//User.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasySettle.Models
{
    public class User
    {
        [Key]
        public string? Email { get; set; }

        // Navigation property
        public virtual ICollection<UserProperty> UserProperties { get; set; } = new List<UserProperty>();
    }
}
