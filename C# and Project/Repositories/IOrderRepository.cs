using C__and_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace C__and_Project.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        Order? GetOrderByID(int studentId, int drinkId);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
    }
}