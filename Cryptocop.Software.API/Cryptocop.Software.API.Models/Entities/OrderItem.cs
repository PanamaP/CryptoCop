using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Entities
{
    public class OrderItem
    {
        public int Id {get;set;}
        public int OrderId {get;set;}
        public string ProductIdentifier {get;set;}
        public float Quantity {get;set;}
        public float UnitPrice {get;set;}
        public float TotalPrice {get;set;}

        //nav
        public Order Order {get;set;}
    }
}