namespace KTPMUD.Models
{
    public class User
    {
        public int id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string sdt { get; set; }
        public string email { get; set; }
        public int id_xa { get; set; }
        public bool status { get; set; }
        public int id_quy_mo { get; set; }
        public int id_huyen { get; set; }
        public int id_co_so { get; set; }
    }
}
