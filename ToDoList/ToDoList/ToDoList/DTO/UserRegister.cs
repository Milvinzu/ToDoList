namespace ToDoList.DTO
{
    public class UserRegister
    {
        private string email;
        private string password;

        public UserRegister(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
