namespace OfflineTicketingSystem.Data.Configurations;

﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities;
using Enums;
 using Helper;
public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        // Rule 1: The 'Email' property must be unique across all users.
        // An index is created to enforce uniqueness and speed up lookups.
        builder.HasIndex(u => u.Email).IsUnique();
        
        // Populate the 'Users' table with initial data.
        // var adminId = 000000-0000-0000-0000-000000000000;
        // var employeeId = 00000000-0000-0000-0000-000000000000;
        //
        // builder.HasData(
        //     new UserEntity()
        //     {
        //         Id = adminId,
        //         FullName = "Admin User",
        //         Email = "admin@bargheto.com",
        //         PasswordHash = PasswordHasher.Hash("Admin@bargheto"),
        //         Role = Role.Admin
        //     },
        //     new UserEntity()
        //     {
        //         Id = employeeId,
        //         FullName = "Peyman Sharifi",
        //         Email = "p.sharifi@bargheto.com",
        //         PasswordHash = PasswordHasher.Hash("pSH@bargheto"),
        //         Role = Role.Employee
        //     }
      //  );
    }
}