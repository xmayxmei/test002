﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Weixin.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bgxtEntities : DbContext
    {
        public bgxtEntities()
            : base("name=bgEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    
        public DbSet<Bg_Mains> Bg_Mains { get; set; }
        //public DbSet<codeclass> codeclass { get; set; }
        public DbSet<Codes> Codes { get; set; }
        public DbSet<Stcodes> Stcodes { get; set; }
        //public DbSet<Sys_Users> Sys_Users { get; set; }
        public DbSet<InterData> InterData { get; set; }
        public DbSet<Stitems> Stitems { get; set; }
        public DbSet<Companies> Companies { get; set; }
    }
}