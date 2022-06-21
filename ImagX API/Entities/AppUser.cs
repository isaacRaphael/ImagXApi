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
        public override string PhoneNumber { get; set; }
        public string ChatConnectionId { get; set; }
        public string NotificationConnectionId { get; set; }
        public SharingKey  Key { get; set; }
        public DateTime DOB { get; set; }
        public ICollection<Package> Packages { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Like> MyLikes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public DateTime Created { get ; set; }


    }
}
