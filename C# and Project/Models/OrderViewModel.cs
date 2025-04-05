﻿using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Models
{
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
        public List<Student> Students { get; set; }
        public List<Drinks> Drinks { get; set; }

        public OrderViewModel() 
        {
            Orders = new List<Order>();
            Students = new List<Student>();
            Drinks = new List<Drinks>();
        }
    }
}
