namespace ApiOnAzure.Dtos
{
    public partial class OrderForStaffDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Treatment { get; set; } = "";
        public int Price {get; set;}
    }
}