using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CodeSmith.Engine;
using SchemaExplorer;

namespace Common
{
    public static class Helper
    {
        public static string GetModelClassDescription(TableSchema table)
        {
            if(string.IsNullOrWhiteSpace(table.Description))
            {
                return table.Name;
            }
            
            return $"{table.Name}\r\n {table.Description}";
        }
        
        public static string GetModelClassName(TableSchema table)
        {
            return StringUtil.ToPascalCase(table.Name.ToLower());
        }
        
        public static string GetModelClassTrimName(TableSchema table,string trimStart,string trimEnd)
        {
            var className = GetModelClassName(table);
            
            if(!string.IsNullOrWhiteSpace(trimStart) && className.StartsWith(trimStart))
            {
                className = className.Substring(trimStart.Length);    
            }
            if(!string.IsNullOrWhiteSpace(trimEnd) && className.EndsWith(trimEnd))
            {
                className = className.Remove(className.Length-trimEnd.Length,trimEnd.Length);    
            }
            
            return className;
        }
    
        public static string GetModelPropertyType(ColumnSchema column)
        {
            if(column.AllowDBNull && column.SystemType.IsValueType)
            {
               return $"{GetTypeName(column.SystemType)}?";
            }
            
            return GetTypeName(column.SystemType);
        }

        public static string GetModelPropertyName(ColumnSchema column)
        {
            return StringUtil.ToPascalCase(column.Name);
        }
       
        public static string GetModelPropertyDescription(ColumnSchema column)
        {
            if(string.IsNullOrWhiteSpace(column.Description))
            {
                return column.Name;
            }
            
            return $"{column.Name}\r\n {column.Description}";
        }
        
        
        public static bool IsNullableType(ColumnSchema column)
        {
            if(column.AllowDBNull || !column.SystemType.IsValueType)
            {
                return true;
            }
            
            return false;
        }
        
        static string GetTypeName(Type type)
        {
            if(type == typeof(string)){
                return "string";
            }
            if(type == typeof(bool)){
                return "bool";
            }
            
            if(type== typeof(Byte))
            {
                return "byte";                
            }
            
            if(type== typeof(Int16))
            {
                return "short";                
            }
            if(type== typeof(Int32))
            {
                return "int";                
            }
            if(type== typeof(Int64))
            {
                return "long";                
            }
            
             if(type== typeof(UInt16))
            {
                return "ushort";                
            }
            if(type== typeof(UInt32))
            {
                return "uint";                
            }
            if(type== typeof(UInt64))
            {
                return "ulong";                
            }
            
            return type.Name;
        }
    }
}