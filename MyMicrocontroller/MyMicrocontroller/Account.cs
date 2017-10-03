namespace MyMicrocontroller
{
    public class Account
    {
        private string name;
        private string password;
        private bool isAdmin;

        public string Name { get => name; }
        public string Password { get => password; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }

        public Account(string name, string password, bool isAdmin = false)
        {
            this.name = name;
            this.password = password;
            this.isAdmin = isAdmin;
        }
    }
}
