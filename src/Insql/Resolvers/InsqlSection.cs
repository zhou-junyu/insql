using System;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Resolvers
{
    public class InsqlSection
    {
        /// <summary>
        /// {IdName},
        /// {IdName}.{ServerName},
        /// {IdName}.{ServerName}.{ServerVersion}
        /// </summary>
        public string Id { get; }

        public string IdName { get; }

        public string ServerName { get; }

        public int? ServerVersion { get; }

        public List<IInsqlSectionElement> Elements { get; }

        public InsqlSection(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var splitId = id.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            this.Id = id;

            if (splitId.Length > 0)
            {
                this.IdName = splitId[0];
            }
            if (splitId.Length > 1)
            {
                this.ServerName = splitId[1];
            }
            if (splitId.Length > 2)
            {
                this.ServerVersion = Convert.ToInt32(splitId[2]);
            }

            this.Elements = new List<IInsqlSectionElement>();
        }

        public virtual string Resolve(ResolveContext context)
        {
            var elementsResult = this.Elements.Select(element =>
            {
                return element.Resolve(context);
            });

            return string.Join(" ", elementsResult).Trim();
        }
    }
}
