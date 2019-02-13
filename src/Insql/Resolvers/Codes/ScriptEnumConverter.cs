using Jint.Native;
using Jint.Runtime.Interop;

namespace Insql.Resolvers.Codes
{
    internal class ScriptEnumConverter : IObjectConverter
    {
        public static ScriptEnumConverter Instance = new ScriptEnumConverter();

        public bool TryConvert(object value, out JsValue result)
        {
            if (value == null)
            {
                result = JsValue.Null; return false;
            }

            if (value.GetType().IsEnum)
            {
                result = new JsValue(value.ToString()); return true;
            }

            result = JsValue.Null; return false;
        }
    }
}
