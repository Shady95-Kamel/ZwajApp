using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZwajApp.API.Helpers;
using ZwajApp.API.Models;

namespace ZwajApp.API.Data
{
    public class ZwajRepository : IZwajRepository
    {
        private readonly DataContext _context;
        public ZwajRepository(DataContext context)
        {
            _context = context;

        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u=>u.UserId==userId).FirstOrDefaultAsync(c=>c.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
           var photo = await _context.Photos.FirstOrDefaultAsync(c=>c.Id==id);
           return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(c=>c.Photos).FirstOrDefaultAsync(c=>c.Id==id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users =  _context.Users.Include(c=>c.Photos).OrderByDescending(u=>u.LastActive).AsQueryable();
            users= users.Where(c=>c.Id != userParams.UserId);
            users=users.Where(c=>c.Gender==userParams.Gender);
            if(userParams.MinAge!=18 || userParams.MaxAge!=99){
                var minDob=DateTime.Today.AddYears(-userParams.MaxAge-1);
                var maxDob=DateTime.Today.AddYears(-userParams.MinAge);
                users= users.Where(c=>c.DateOfBirth>=minDob && c.DateOfBirth<=maxDob);
            }
            if(!string.IsNullOrEmpty(userParams.OrderBy)){
                switch (userParams.OrderBy)
                {
                    case "created":
                    users = users.OrderByDescending(c=>c.Created);
                    break;
                    default:
                    users = users.OrderByDescending(c=>c.LastActive);
                    break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber,userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
           return await _context.SaveChangesAsync()>0;
        }
    }
}