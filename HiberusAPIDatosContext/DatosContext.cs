using HiberusAPIEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiberusAPIDatosContext
{
    public class DatosContext:DbContext
    {

        public DatosContext(){}

        public DatosContext(DbContextOptions<DatosContext> options):base(options) { }

        public DbSet<RateEnt> Rates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("conexionDB"));
            }
        }


        /// <summary>
        /// Mapeo del modelo de EF con tablas rates y transactions
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RateEnt>(entity =>
            {
                entity.ToTable("rates");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int");

                entity.Property(e => e.From).HasColumnName("from").HasColumnType("varchar(3)");

                entity.Property(e => e.To).HasColumnName("to").HasColumnType("varchar(3)");

                entity.Property(e => e.Rate).HasColumnName("rate").HasColumnType("decimal(10,3)");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int");

                entity.Property(e => e.Sku).HasColumnName("sku").HasColumnType("varchar(255)");

                entity.Property(e => e.Currency).HasColumnName("currency").HasColumnType("varchar(3)");

                entity.Property(e => e.Amount).HasColumnName("amount").HasColumnType("decimal(10,3)");
            });
        }

    }
}
