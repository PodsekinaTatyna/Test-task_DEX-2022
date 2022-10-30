using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsDb
{
    [Table("ads")]
    public class AdDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("number")]
        public int Number { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("image")]
        public string Image { get; set; }

        [Column("rating")]
        public int Rating { get; set; }

        [Column("created_by")]
        public DateTime CreatedBy { get; set; }

        [Column("expiration_date")]
        public DateTime ExpirationDate { get; set; }

        public UserDb User { get; set; }
    }
}
