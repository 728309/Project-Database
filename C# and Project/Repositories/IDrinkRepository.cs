using C__and_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace C__and_Project.Repositories
{
    public interface IDrinkRepository
    {
        List<Drinks> GetAllDrinks();
        Drinks? GetDrinksByID(int drinkID);
        void AddDrink(Drinks drink);
        void UpdateDrink(Drinks drink);
        void DeleteDrink(Drinks student);
    }
}
