#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPIWithCosmosDB.Models;

namespace WebAPIWithCosmosDB.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext (DbContextOptions<OrderContext> options)
            : base(options)
        {
        }

        public DbSet<WebAPIWithCosmosDB.Models.Order> Order { get; set; }
    }
}
