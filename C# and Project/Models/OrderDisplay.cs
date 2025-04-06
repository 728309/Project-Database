using C__and_Project.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Models
{
    public class OrderDisplay
    {
        public string StudentFullName { get; set; }
        public string DrinkName { get; set; }
        public int Amount { get; set; }
        public int StudentId { get; set; }
        public int DrinkId { get; set; }
    }

}


