using Insql.Mappers;

namespace Insql.Tests.Models
{
    public class FluentModelInfoBuilder : InsqlEntityBuilder<FluentModelInfo>
    {
        public FluentModelInfoBuilder()
        {
            this.Table("fluent_model_info");

            this.Property(o => o.Id).Column("id").Key().Identity();
            this.Property(o => o.Name).Column("name");
            this.Property(o => o.Size).Column("Size");
            this.Property(o => o.Extra).Ignore();
        }
    }
}
