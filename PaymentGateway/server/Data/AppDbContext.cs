﻿using Microsoft.EntityFrameworkCore;
using PaymentGateway.Server.Models;

namespace PaymentGateway.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PaymentGateway.Server.Models.PaymentRequest> PaymentRequests { get; set; }
    public DbSet<PaymentResult> PaymentResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentResult>()
            .HasOne(p => p.PaymentRequest)
            .WithOne()
            .HasForeignKey<PaymentResult>(p => p.PaymentRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}