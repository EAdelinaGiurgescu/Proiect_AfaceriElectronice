namespace Imbracaminte.Models.Entities
{
    public class Users
    {
        public Users()
        {
            UserName = string.Empty;
            Password= string.Empty;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }

        public static List<Users> GetAll()
        {
            var users = new List<Users>();

            users.Add(new Users() { Id = 1, UserName = "Ionut", Password = "100"});
            users.Add(new Users() { Id = 2, UserName = "Mihai", Password = "200" });
            users.Add(new Users() { Id = 3, UserName = "Ioana", Password = "300" });

            return users;
        }
    }
}
