using Jint.Native;
using Jint.Runtime.Interop;
using System;

namespace Insql.Resolvers.Scripts
{
    internal class ScriptDateTimeConverter : IObjectConverter
    {
        public static ScriptDateTimeConverter Instance = new ScriptDateTimeConverter();

        public bool TryConvert(object value, out JsValue result)
        {
            if (value == null)
            {
                result = JsValue.Null; return false;
            }

            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue == DateTime.MinValue)
                {
                    result = JsValue.Null; return true;
                }
            }
            if (value is DateTimeOffset dateTimeOffsetValue)
            {
                if (dateTimeOffsetValue == DateTimeOffset.MinValue)
                {
                    result = JsValue.Null; return true;
                }
            }

            result = JsValue.Null; return false;
        }
    }

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
