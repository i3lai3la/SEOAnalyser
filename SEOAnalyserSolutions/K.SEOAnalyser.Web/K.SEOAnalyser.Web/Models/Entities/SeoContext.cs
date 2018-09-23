using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K.SEOAnalyser.Web.Models.Entities
{
    public class SeoContext : DbContext
    {
        public SeoContext(DbContextOptions<SeoContext> options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<Content> Contents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
