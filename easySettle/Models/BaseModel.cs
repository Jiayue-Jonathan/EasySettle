using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace easySettle.Models
{
    public class BaseModel : FullAudit
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        [Display(Name = "Deleted?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Enable?")]
        public bool Enable { get; set; }

    }
}
