using ImagX_API.Contracts;
using ImagX_API.Data;
using ImagX_API.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AppUser> Add(AppUser entity)
        {
            await _context.Users.AddAsync(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0 ? entity : null;
        }


        public async Task<bool> Exists(string id)
        {
            return await _context.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<ICollection<AppUser>> GetAll()
        {
            if(_context.Users is not null)
                return await _context.Users.ToListAsync();

            return null;
        }

        public async Task<ICollection<AppUser>> GetBuddies(string id)
        {
            var friendships = _context.Friendships.Where(x => x.User1ID == id || x.User2ID == id);
            var firstSet = friendships.Select(x => x.User1).Where(x => x.Id != id).ToList();
            var secondSet = friendships.Select(x => x.User2).Where(x => x.Id != id).ToList();

           var buddies =  Enumerable.Concat(firstSet, secondSet);
           return await  Task.Run(() => buddies.ToList());
        }

        public async Task<AppUser> GetById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    

        public async Task<ICollection<AppUser>> GetPaginated(int page = 1, int pageSize = 10)
        {
            return await _context.Users.Skip((page - 1) * pageSize).Take(pageSize).OrderBy(x => x.Created).ToListAsync();
        }

        public async Task<bool> Remove(string id)
        {
            var toremove = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (toremove is null)
                return false;

            _context.Users.Remove(toremove);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(JsonPatchDocument entity, string id)
        {
            var toUpdate = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(toUpdate is not null)
                entity.ApplyTo(toUpdate);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
