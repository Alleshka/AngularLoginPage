using System.ComponentModel.DataAnnotations;

namespace AngularLoginPage.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public Guid ProvinceId { get; set; }

        public Province Province { get; set; }
    }
}
