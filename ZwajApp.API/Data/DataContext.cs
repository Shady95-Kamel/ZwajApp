using Microsoft.EntityFrameworkCore;
using ZwajApp.API.Models;

namespace ZwajApp.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
         public DbSet<Value> Values { get; set; }
         public DbSet<User> Users { get; set; }
         public DbSet<Photo> Photos { get; set; }
         public DbSet<Like> Likes { get; set; }

         protected override void OnModelCreating(ModelBuilder builder){
             builder.Entity<Like>()
             .HasKey(k=> new {k.LikeeId,k.LikerId});
             builder.Entity<Like>()
             .HasOne(l=>l.Likee)
             .WithMany(u=>u.Likers)
             .HasForeignKey(l=>l.LikeeId)
             .OnDelete(DeleteBehavior.Restrict);

             builder.Entity<Like>()
             .HasKey(k=> new {k.LikeeId,k.LikerId});
             builder.Entity<Like>()
             .HasOne(l=>l.Liker)
             .WithMany(u=>u.Likees)
             .HasForeignKey(l=>l.LikerId)
             .OnDelete(DeleteBehavior.Restrict);
         }
    }
}