namespace R4R_API.ApiModel
{
    public class updatePay
    {

        public string IdRoom { get; set; } = null!;
        public string CartId { get; set; } = null!;
        public string id { get; set; }
        public int NoWater { get; set; }
        public int NoElectic { get; set; }
        public string? note { get; set; }
        public int? status { get; set; }
    }
}
