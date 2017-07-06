using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class CommonMethod
    {
        public static List<T> DataTableToDto<T>(this DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }
        }

        private static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }

        public static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            // Create the result table, and gather all properties of a T
            DataTable table = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add the properties as columns to the datatable
            foreach (var prop in props)
            {
                Type propType = prop.PropertyType;

                // Is it a nullable type? Get the underlying type
                if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    propType = new NullableConverter(propType).UnderlyingType;

                table.Columns.Add(prop.Name, propType);
            }

            // Add the property values per T as rows to the datatable
            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                    values[i] = props[i].GetValue(item, null);

                table.Rows.Add(values);
            }

            return table;
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            if (str != null)
            {
                return str.Length > 0;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNumber(this string valStr)
        {
            string pattern = @"[\D]";
            Regex myRegex = new Regex(pattern);
            if (myRegex.IsMatch(valStr))
            {
                return false;
            }
            return true;
        }

        public static int ParseInt32(this string num)
        {
            if (num.IsNumber() && num.IsNotNullOrEmpty())
            {
                return int.Parse(num);
            }
            else
            {
                return 0;
            }
        }

        public static DateTime? ParseDateTime(this object date)
        {
            if (date is DateTime)
            {
                return (DateTime)date;
            }
            else
            {
                return null;
            }
        }

        public static bool Parsebool(this string bole)
        {
            if (bole.IsNotNullOrEmpty() && bole.Length > 0)
            {
                return bool.Parse(bole);
            }
            else
            {
                return false;
            }
        }
    }
}
