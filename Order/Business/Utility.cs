using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Order.Business
{
    [DebuggerDisplay("Index = {Index}, Name = {Name}, Ignore = {Ignore}, ReferenceKey = {ReferenceKey}")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CsvFieldAttribute : Attribute
    {
        public virtual object Default { get; set; }
        public virtual string Format { get; set; }
        public virtual bool Ignore { get; set; }
        public virtual int Index { get; set; }
        public virtual string Name { get; set; }
        public virtual string[] Names { get; set; }
        public virtual string ReferenceKey { get; set; }
    }
    public class Utility
    {
        public static string GetCsv<T>(List<T> csvDataObjects)
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            var sb = new StringBuilder();
            sb.AppendLine(GetCsvHeaderSorted(propertyInfos));
            csvDataObjects.ForEach(d => sb.AppendLine(GetCsvDataRowSorted(d, propertyInfos)));
            return sb.ToString();
        }

        private static string GetCsvDataRowSorted<T>(T csvDataObject, PropertyInfo[] propertyInfos)
        {
            IEnumerable<string> valuesSorted = propertyInfos
                .Select(x => new
                {
                    Value = x.GetValue(csvDataObject, null),
                    Attribute = (CsvFieldAttribute)Attribute.GetCustomAttribute(x, typeof(CsvFieldAttribute), false)
                })
                .Where(x => x.Attribute != null)
                .OrderBy(x => x.Attribute.Index)
                .Select(x => GetPropertyValueAsString(x.Value));
            return String.Join(",", valuesSorted);
        }

        private static string GetCsvHeaderSorted(PropertyInfo[] propertyInfos)
        {
            IEnumerable<string> headersSorted = propertyInfos
                .Select(x => (CsvFieldAttribute)Attribute.GetCustomAttribute(x, typeof(CsvFieldAttribute), false))
                .Where(x => x != null)
                .OrderBy(x => x.Index)
                .Select(x => x.Name);
            return String.Join(",", headersSorted);
        }

        private static string GetPropertyValueAsString(object propertyValue)
        {
            string propertyValueString;

            if (propertyValue == null)
                propertyValueString = "";
            else if (propertyValue is DateTime)
                propertyValueString = ((DateTime)propertyValue).ToString("dd MMM yyyy");
            else if (propertyValue is int)
                propertyValueString = propertyValue.ToString();
            else if (propertyValue is float)
                propertyValueString = ((float)propertyValue).ToString("#.####"); // format as you need it
            else if (propertyValue is double)
                propertyValueString = ((double)propertyValue).ToString("#.####"); // format as you need it
            else // treat as a string
                propertyValueString = @"""" + propertyValue.ToString().Replace(@"""", @"""""") + @""""; // quotes with 2 quotes

            return propertyValueString;
        }
    }
}