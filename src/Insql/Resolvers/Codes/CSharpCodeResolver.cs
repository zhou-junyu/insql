//using Microsoft.CodeAnalysis.CSharp.Scripting;
//using Microsoft.CodeAnalysis.Scripting;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace Insql.Resolvers.Codes
//{
//    public class CSharpCodeResolver : IInsqlCodeResolver
//    {
//        private readonly ScriptOptions scriptOptions;
//        private readonly ConcurrentDictionary<string, Script> scriptCaches;

//        public CSharpCodeResolver()
//        {
//            this.scriptOptions = this.CreateOptions();

//            this.scriptCaches = new ConcurrentDictionary<string, Script>();
//        }

//        public void Dispose()
//        {
//        }

//        private string ConvertCode(string code)
//        {
//            return code
//                .Replace(" and ", " && ")
//                .Replace(" or ", " || ")
//                .Replace(" gt ", " > ")
//                .Replace(" gte ", " >= ")
//                .Replace(" lt ", " < ")
//                .Replace(" lte ", " <= ")
//                .Replace(" eq ", " == ")
//                .Replace(" neq ", " != ");
//        }

//        private string CreateCode(string code, IDictionary<string, object> param)
//        {
//            var paramString = string.Join(Environment.NewLine, param.Select(item =>
//            {
//                if (item.Value == null)
//                {
//                    return $"dynamic {item.Key} = {nameof(CSharpCodeGlobals.Globals)}[\"{item.Key}\"];";
//                }

//                return $"var {item.Key} = ({item.Value.GetType().FullName}){nameof(CSharpCodeGlobals.Globals)}[\"{item.Key}\"];";
//            }));

//            return $"{paramString}{Environment.NewLine}{Environment.NewLine}{this.ConvertCode(code)}";
//        }

//        private ScriptOptions CreateOptions()
//        {
//            return ScriptOptions.Default.AddReferences(new List<Assembly>
//            {
//                Assembly.GetEntryAssembly(),
//                Assembly.GetCallingAssembly(),
//                Assembly.GetExecutingAssembly()
//            }.Distinct()).AddImports(new List<string>
//            {
//                "System",
//                "System.Text",
//                "System.Linq",
//                "System.Collections.Generic",
//            });
//        }

//        public T Resolve<T>(string code, IDictionary<string, object> param)
//        {
//            if (string.IsNullOrWhiteSpace(code))
//            {
//                throw new ArgumentNullException(code);
//            }
//            if (param == null)
//            {
//                throw new ArgumentNullException(nameof(param));
//            }

//            var codeString = this.CreateCode(code, param);
//            var hashString = $"{codeString.GetHashCode()}-{typeof(T).GetHashCode()}";

//            var script = (Script<T>)this.scriptCaches.GetOrAdd(hashString, (key) =>
//            {
//                return CSharpScript.Create<T>(codeString, this.scriptOptions, typeof(CSharpCodeGlobals));
//            });

//            var state = script.RunAsync(new CSharpCodeGlobals { Globals = param }).Result;

//            return state.ReturnValue;
//        }
//    }

//    public class CSharpCodeGlobals
//    {
//        public IDictionary<string, object> Globals { get; set; }
//    }
//}
