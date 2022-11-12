using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsDb
{
    [Table("announcements")]
    public class AnnouncementDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("number")]
        public int Number { get; set; }

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [Column("text")]
        public string Text { get; set; }

        [Required]
        [Column("image")]
        public string Image { get; set; }

        [Required]
        [Column("rating")]
        public int Rating { get; set; }

        [Required]
        [Column("created_by", TypeName = "date")]
        public DateTime Created { get; set; }
        
        [Required]
        [Column("expiration_date", TypeName = "date")]
        public DateTime ExpirationDate { get; set; }

        public UserDb User { get; set; }
    }
}
