using System;
using System.Collections.Generic;

namespace OrderSystem.Domain.Entities
{
    public class Client
    {
        public List<Order> Orders {get; set;} = new List<Order>();

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}