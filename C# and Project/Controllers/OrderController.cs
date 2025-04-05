using C__and_Project.Repositories;
using Microsoft.AspNetCore.Mvc;
using C__and_Project.Models;

namespace C__and_Project.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDrinkRepository _drinkRepository;
        private readonly IStudentRepository _studentRepository;

        public OrderController(IOrderRepository orderRepository, IDrinkRepository drinkRepository, IStudentRepository studentRepository)
        {
            _orderRepository = orderRepository;
            _drinkRepository = drinkRepository;
            _studentRepository = studentRepository;
        }

        public IActionResult Index()
        {
            var orders = _orderRepository.GetAllOrders();
            return View(orders);
        }

        public IActionResult PlaceOrder()
        {
            OrderViewModel viewModel = new OrderViewModel
            {
                Orders = _orderRepository.GetAllOrders(),
                Students = _studentRepository.GetAllStudents(),
                Drinks = _drinkRepository.GetAllDrinks()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            try
            {
                _orderRepository.AddOrder(order);
                TempData["SuccessMessage"] = "Order placed successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
