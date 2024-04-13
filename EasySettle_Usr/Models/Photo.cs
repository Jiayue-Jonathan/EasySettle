using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EasySettle.Models
{
    public class Photo
    {
        
        public int PhotoID { get; set; }
        public int PropertyID { get; set; }
        public string? Url { get; set; }
        
        public virtual Property? Property { get; set; }

    }
}
