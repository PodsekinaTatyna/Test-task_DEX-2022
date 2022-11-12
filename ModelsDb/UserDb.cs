using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsDb
{
    [Table("users")]
    public class UserDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        public ICollection<AnnouncementDb> Ads { get; set; }
    }
}
