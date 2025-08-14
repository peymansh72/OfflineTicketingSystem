﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace OfflineTicketingSystem.Data;
using Configurations;
using Entities;

public class AppDbContext(DbContextOptions options) : DbContext(options) {
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TicketEntity> Tickets { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
    }
    }