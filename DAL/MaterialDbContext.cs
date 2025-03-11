using Microsoft.EntityFrameworkCore;
using MaterialGatePassTacker.Models;

namespace MaterialGatePassTacker
{
    public class MaterialDbContext : DbContext
    {
        
        public MaterialDbContext(DbContextOptions<MaterialDbContext> options) : base(options)
        {
        }
        public DbSet<D_User> Users { get; set; }

        public DbSet<D_User_Attribute> UsersAttributes { get; set; }

        public DbSet<M_Department> Departments { get; set; }

        public DbSet<M_Gate> Gates { get; set; }

        public DbSet<M_Project> Projects { get; set; }

        public DbSet<M_Role> Roles { get; set; }

        public DbSet<M_Status> Statuss{ get; set; }

        public DbSet<T_Classification_Type> Classifications { get; set; }

        public DbSet<T_Gate_Pass> GatesPasses { get; set; }

        public DbSet<T_Gate_Pass_Document> GatesPassDocuments { get; set; }

        public DbSet<T_Gate_Pass_History> GatesPassHistory { get; set; }

        public DbSet<M_SOU> SOUs { get; set; }
    }
}
