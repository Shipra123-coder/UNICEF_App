using Microsoft.EntityFrameworkCore;
using MO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Login_User> Login_User { get; set; }
        public DbSet<m_group> m_group { get; set; }
        public DbSet<m_UserLevel> m_UserLevel { get; set; }
        public DbSet<mst_Agency> mst_Agency { get; set; }
        public DbSet<mst_Departments> mst_Departments { get; set; }     
        public DbSet<mst_Sector> mst_Sector { get; set; }     
        public DbSet<mst_Pillar> mst_Pillar { get; set; }
        public DbSet<mst_UNSector> mst_UNSector { get; set; }
        public DbSet<mst_NatureOfSupport> mst_NatureOfSupport { get; set; }
        public DbSet<mst_SubNatureOfSupport> mst_SubNatureOfSupport { get; set; }
        public DbSet<m_Menu> m_Menu { get; set; }
        public DbSet<mst_ContactMaster> mst_ContactMaster { get; set; }
        public DbSet<mst_CMDetails> mst_CMDetails { get; set; }
        public DbSet<mst_NodalDetail> mst_NodalDetail { get; set; }
        public DbSet<m_MenuPermission> m_MenuPermission { get; set; }
    }
}
