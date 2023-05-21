namespace R4R_API.Models
{
    public partial class Tenant
    {
        public string? Id { get; set; }

        public string idRoom { get; set; }

        public string Name { get; set; }

        public string adress { get; set; }

        public string phone { get; set; }

        public string cartId { get; set; }

        public int status { get; set; }

        public DateTime dateJoin { get; set; }

        public DateTime dateOut { get; set; }
    }
}
