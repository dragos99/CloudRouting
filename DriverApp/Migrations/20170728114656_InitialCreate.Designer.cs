using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DriverApp.Models;

namespace DriverApp.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20170728114656_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DriverApp.Models.Driver", b =>
                {
                    b.Property<string>("DriverId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ManagerCustomerKey");

                    b.HasKey("DriverId");

                    b.HasIndex("ManagerCustomerKey");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("DriverApp.Models.Manager", b =>
                {
                    b.Property<string>("CustomerKey")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(4);

                    b.HasKey("CustomerKey");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("DriverApp.Models.Driver", b =>
                {
                    b.HasOne("DriverApp.Models.Manager", "Manager")
                        .WithMany("Drivers")
                        .HasForeignKey("ManagerCustomerKey");
                });
        }
    }
}
