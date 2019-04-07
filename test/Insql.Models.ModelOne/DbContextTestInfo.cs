using System;

namespace Insql.Models.ModelOne
{
    public class DbContextTestInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public InfoType? Type { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public enum InfoType
    {
        Red, Black
    }
}
