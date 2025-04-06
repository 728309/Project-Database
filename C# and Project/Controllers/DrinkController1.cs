using C__and_Project.Models;
using C__and_Project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Controllers
{
    public class DrinkController : Controller
    {
        private readonly IDrinkRepository _drinkRepository;

        public DrinkController(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

        public IActionResult Index()
        {
            var drinks = _drinkRepository.GetAllDrinks();
            return View(drinks);
        }
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Drinks drink)
        {
            if (ModelState.IsValid)
            {
                _drinkRepository.AddDrink(drink);
                return RedirectToAction("Index");
            }
            return View(drink);
        }
    }
}