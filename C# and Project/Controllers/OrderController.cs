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
            return PlaceOrder();
        }

        public IActionResult PlaceOrder()
        {
            OrderViewModel viewModel = new OrderViewModel
            {
                Orders = _orderRepository.GetAllOrders(),
                Students = _studentRepository.GetAllStudents(),
                Drinks = _drinkRepository.GetAllDrinks()
            };

            foreach (var order in viewModel.Orders)
            {
                var student = viewModel.Students.FirstOrDefault(s => s.StudentID== order.StudentId);
                var drink = viewModel.Drinks.FirstOrDefault(d => d.DrinkID == order.DrinkId);

                if (student != null && drink != null)
                {
                    viewModel.DisplayOrders.Add(new OrderDisplay
                    {
                        StudentFullName = student.FirstName + " " + student.LastName,
                        DrinkName = drink.DrinkName,
                        Amount = order.Amount,
                        StudentId = order.StudentId,
                        DrinkId = order.DrinkId

                    });
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult PlaceOrder(int StudentId, int DrinkId, int Amount)
        {
            try
            {
                var student = _studentRepository.GetStudentByID(StudentId);
                var drink = _drinkRepository.GetDrinksByID(DrinkId);

                if (drink == null || student == null)
                {
                    TempData["ErrorMessage"] = "Invalid student or drink.";
                    return RedirectToAction("Index");
                }

                Order order = new Order
                {
                    StudentId = StudentId,
                    DrinkId = DrinkId,
                    Amount = Amount
                };
                _orderRepository.AddOrder(order);
                TempData["SuccessMessage"] = "Order placed successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int studentId, int drinkId)
        {
            var order = _orderRepository.GetOrderByID(studentId, drinkId);
            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.UpdateOrder(order);
                return RedirectToAction("Index"); // or the appropriate view
            }
            return View(order);
        }

        public IActionResult Delete(int studentId, int drinkId)
        {
            var order = _orderRepository.GetOrderByID(studentId, drinkId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult Delete(Order order)
        {
            try
            {
                _orderRepository.DeleteOrder(order);
                TempData["SuccessMessage"] = "Order deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}