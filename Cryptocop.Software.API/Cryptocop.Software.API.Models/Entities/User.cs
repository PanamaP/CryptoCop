using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Entities
{
    public class User
    {
        public int Id {get; set;}
        public string FullName {get;set;}
        public string Email {get; set;}
        public string HashedPassword {get;set;}
    }
}