namespace R4R_API.Models
{
    public partial class PayRoom
    {

        public string? Id { get; set; }

        public string? IdRoom { get; set; } 
        public string? CartId { get; set; } 
        public int? Month { get; set; } 
        public int? NoWater { get; set; } 

        public int? NoElectic { get; set; } 
        public int? RoomPrice { get; set; }
        public int? otherPrice { get; set; } 
        public string? note { get; set; }
        public int? status { get; set; }
        public DateTime? Created { get; set; }
    }
}
