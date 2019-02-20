using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    [Obsolete("will be removed in the new version")]
    public class ResolveEnviron
    {
        private readonly Dictionary<string, string> items;

        public static ResolveEnviron Create()
        {
            return new ResolveEnviron();
        }

        public ResolveEnviron()
        {
            this.items = new Dictionary<string, string>();
        }

        public ResolveEnviron(IDictionary<string, string> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            this.items = new Dictionary<string, string>(items);
        }

        public IEnumerable<string> Keys => this.items.Keys;

        public ResolveEnviron Clone()
        {
            return new ResolveEnviron(this.items);
        }

        public bool Contain(string key)
        {
            return this.items.ContainsKey(key);
        }

        public ResolveEnviron Set(string key, string value)
        {
            this.items[key] = value;

            return this;
        }

        public string Get(string key)
        {
            if (this.items.TryGetValue(key, out string value))
            {
                return value;
            }

            return null;
        }
    }
}
