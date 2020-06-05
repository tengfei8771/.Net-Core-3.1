using Microsoft.EntityFrameworkCore;
using System;

namespace Models
{
    public class AppContext:DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {

        }
    }
}
