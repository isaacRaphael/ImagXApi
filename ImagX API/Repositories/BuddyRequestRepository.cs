using ImagX_API.Contracts;
using ImagX_API.Data;
using ImagX_API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Repositories
{
    public class BuddyRequestRepository : GenericRepository<BuddyRequest>, IBuddyRequestRepository
    {
        private readonly AppDbContext _context;

        public BuddyRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        
        public async Task<bool> AddBuddy(BuddyRequest buddyRequest)
        {
            _context.BuddyRequests.Add(buddyRequest);
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
                return false;

            var newNotification = new RequestNotification { 
                RecipientID = buddyRequest.RecipientId, 
                SenderID = buddyRequest.SenderId,
                Action = buddyRequest, Created = DateTime.UtcNow 
            };
            _context.RequestNotifications.Add(newNotification);
            return await _context.SaveChangesAsync() > 0;
        }

        public async  Task<bool> ConfirmBuddyRequest(int id)
        {
            var br = await _context.BuddyRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (br is null)
                return false;

            var u1 = await _context.Users.FirstOrDefaultAsync(x => x.Id == br.SenderId);
            var u2 = await _context.Users.FirstOrDefaultAsync(x => x.Id == br.RecipientId);
            

            var FriendShip = new Friendship { User1 = u1, User1ID = u1.Id, User2 = u2, User2ID = u2.Id };
            await _context.Friendships.AddAsync(FriendShip);


            return await _context.SaveChangesAsync() > 0;
        }

        


    }
}
