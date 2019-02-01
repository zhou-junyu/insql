namespace Example.Domain
{
    public class UserInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public Gender? UserGender { get; set; }
    }

    public enum Gender
    {
        M,
        W
    }
}
