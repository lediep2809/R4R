namespace R4R_API.ApiModel
{
    public class NewPayMonth
    {

        public string IdRoom { get; set; } = null!;
        public string CartId { get; set; } = null!;
        public int Month { get; set; }
        public int Year { get; set; }
        public int NoWater { get; set; } 
        public int NoElectic { get; set; } 
        public string? note { get; set; }
        public int? status { get; set; }
    }
}
