using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entityes;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Data.DbSettings
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Article> Articles { get; set; } = null!;

        public DbSet<Tag> Tags { get; set; } = null!;

        public DbSet<Comment> Comments { get; set; } = null!;


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.Migrate();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Остановился на создании таблиц БД, нужно проверить связи таблицы
        }
    }
}
