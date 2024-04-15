namespace ApiOnAzure.Dtos
{
    public partial class OrderForUserDto
    {
        public int OrderId { get; set; }
        public int StaffId { get; set; }
        public string FirstName { get; set; } = "";
        public string Treatment { get; set; } = "";
        public double Price { get; set; }
    }
}

