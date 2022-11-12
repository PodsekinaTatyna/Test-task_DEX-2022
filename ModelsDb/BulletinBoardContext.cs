using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsDb
{
    public class BulletinBoardContext : DbContext
    {
        public DbSet<AnnouncementDb> Ads { get; set; }

        public DbSet<UserDb> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                                    "Host=localhost;" +
                                    "Port = 5432;" +
                                    "Database = bulletin_board;" +
                                    "Username = postgres;" +
                                    "Password = tany0206"
                                    );

        }
    }
}
