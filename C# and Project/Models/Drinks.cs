using System.Numerics;

namespace C__and_Project.Models
{
    public class Drinks
    {
        public int DrinkID { get; set; }
        public string DrinkName { get; set; }
        public int Stock { get; set; }
        public string TypeDrink { get; set; }
        public int VatPercentage { get; set; }
        //I feel like this should be DateTime but I'm not sure how to get it to work exactly yet

        public Drinks()
        {

        }
        //I'm just adding a comment here to test out something
        public Drinks(int drinkID,
                    string drinkName, string typedrink, int stock, int vatpercentage)
        {
            DrinkID = drinkID;
            DrinkName = drinkName;
            TypeDrink = typedrink;
            Stock = stock;
            VatPercentage = vatpercentage;
        }
    }
}
    