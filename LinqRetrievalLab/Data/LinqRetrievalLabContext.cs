using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LinqRetrievalLab.Models.Domain;

namespace LinqRetrievalLab.Data
{
    public class LinqRetrievalLabContext : DbContext
    {
        public LinqRetrievalLabContext (DbContextOptions<LinqRetrievalLabContext> options)
            : base(options)
        {
        }

        public DbSet<LinqRetrievalLab.Models.Domain.Category> Category { get; set; } = default!;
    }
}
