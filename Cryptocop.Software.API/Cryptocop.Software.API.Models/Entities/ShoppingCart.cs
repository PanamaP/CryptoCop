using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Entities
{
    public class ShoppingCart
    {
        public int Id {get;set;}
        public int UserId {get;set;}

        //Nav
        public User User {get;set;}
    }
}