namespace Insql.Tests.Domain.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public UserGender? UserGender { get; set; }
    }

    public enum UserGender
    {
        M,
        W
    }
}
