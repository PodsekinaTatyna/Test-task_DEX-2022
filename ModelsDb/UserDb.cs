using System;
using System.Collections.Generic;
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

        [Column("name")]
        public string Name { get; set; }

        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        public ICollection<AdDb> Ads { get; set; }
    }
}
