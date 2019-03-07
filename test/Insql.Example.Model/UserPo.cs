using System;

namespace Insql.Example.Model
{
    public class UserPo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public UserGender UserGender { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? LoginTime { get; set; }

        public string ExcludeData { get; set; }
    }

    public enum UserGender
    {
        W, M
    }
}
