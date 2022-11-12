using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Filters
{
    public class AnnouncementFilter
    {
        public int? Number { get; set; }

        public Guid? UserId { get; set; }

        public string? Text { get; set; }

        public string? Image { get; set; }

        public int? maxRating { get; set; }

        public int? minRating { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }
    }
}
