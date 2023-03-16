using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Entities
{
    public class PaymentCard
    {
        public int Id {get;set;}
        public int UserId {get;set;}
        public string CardHolderName {get;set;}
        public string CardNumber {get;set;}
        public int Month {get;set;}
        public int Year {get;set;}

        //Navigation properties
        public User User {get;set;}
    }
}