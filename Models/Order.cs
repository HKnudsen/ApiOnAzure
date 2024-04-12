namespace ApiOnAzure.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int StaffId { get; set; }
        public string Treatment { get; set; } = "";
        public double Price { get; set; }
    }
}