using BillBookManagementSystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BillBookManagementSystemApp.Context
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<OtpModel> otpModel { get; set; }

    }
}
