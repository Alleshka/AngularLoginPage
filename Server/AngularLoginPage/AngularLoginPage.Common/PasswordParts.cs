namespace AngularLoginPage.Common
{
    public class PasswordParts
    {
        public string Password { get; protected set; }
        public string Salt { get; protected set; }

        public PasswordParts(string password, string salt)
        {
            Password = password;
            Salt = salt;
        }

        public PasswordParts(byte[] password, byte[] salt) :
            this(Convert.ToBase64String(password), Convert.ToBase64String(salt))
        {

        }
    }
}
