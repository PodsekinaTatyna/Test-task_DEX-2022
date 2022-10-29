using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Ad
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public Guid User { get; set; }  

        public string Text { get; set; }

        public string Image { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedBy { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
