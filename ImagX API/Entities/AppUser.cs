using ImagX_API.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Created = DateTime.Now;
        }   
        public string FirstName { get; set; }
        public int Status { get; set; }
        public string LastName { get; set; }
        public override string UserName { get; set; }
        public DateTime DOB { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<AppUser> Buddies { get; set; } = new List<AppUser>();
        public ICollection<Like> MyLikes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public DateTime Created { get ; set; }
        public string Identifier { get => Id; }


    }
}
