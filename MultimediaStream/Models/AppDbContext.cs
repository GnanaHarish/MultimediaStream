using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MultimediaStream.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        //public AppDbContext()
        //{

        //}

        public AppDbContext(DbContextOptions options) : base(options){ 
            
        }
    }
}
