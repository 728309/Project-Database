namespace C__and_Project.Models
{
    public class Order
    {
        public int StudentId { get; set; }
        public int DrinkId { get; set; }
        public int Amount { get; set; }

        public Order()
        {

        }

        public Order(int studentId, int drinkId, int amount)
        {
            StudentId = studentId;
            DrinkId = drinkId;
            Amount = amount;
        }
    }
}
