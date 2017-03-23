using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbdulLCTest.Data
{
    public class Posts
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string Subject { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(128)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(128)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        
    }
}
