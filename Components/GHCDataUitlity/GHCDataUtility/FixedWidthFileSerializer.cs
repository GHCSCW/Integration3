using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Data;
using System.ComponentModel;

namespace Ghc.Utility
{
    public static class FixedWidthFileSerializer
    {
        #region FUNCTION: CreateItem<T>(DataRow row)

        /// <summary>
        /// Convert From Data Source to List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);

            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);

                    try
                    {
                        object value = row[column.ColumnName];

                        switch (prop.PropertyType.Name.ToString())
                        {
                            case "DateTime":
                                prop.SetValue(obj, Convert.ToDateTime(value == null ? null : value), null);
                                break;

                            case "Int32":
                                prop.SetValue(obj, Convert.ToInt32(value), null);
                                break;

                            case "Decimal":
                                prop.SetValue(obj, Convert.ToDecimal(value), null);
                                break;

                            default:
                                prop.SetValue(obj, value == null ? null : value.ToString(), null);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return obj;
        }
        #endregion
        
        #region PROCEDURE:  GetProperties(Type type)

        private static IEnumerable<PropertyInfo> GetProperties(Type Type)
        {
            var vntAttributeType = typeof(FlatFileAttribute);

            return Type.GetProperties()
                    .Where(prop => prop.GetCustomAttributes(vntAttributeType, false).Any())
                    .OrderBy(
                        prop =>
                        ((FlatFileAttribute)prop.GetCustomAttributes(vntAttributeType, false).First()).Position);
        }

        #endregion
        
        #region PROCEDURE: SerializeOutputFile(object Data, StreamWriter Writer)

        public static void SerializeOutputFile(object Data, StreamWriter Writer)
        {
            try
            {
                var properties = GetProperties(Data.GetType());
                var attributeType = typeof(FlatFileAttribute);
                int intAttrLength;
                DateTime dtObject;
                string numericString;

                foreach (var propertyInfo in properties)
                {
                    //Get object property info
                    var Value = propertyInfo.GetValue(Data, null).ToString();
                    intAttrLength = propertyInfo.GetCustomAttributes(attributeType, false).Length;

                    //Not all Properties have Attributes, checking length for that reason
                    if (intAttrLength > 0)
                    {
                        var Attr = (FlatFileAttribute)propertyInfo.GetCustomAttributes(attributeType, false).First();

                        //Format property if specified
                        if (Attr.Format.Length != 0)
                        {
                            switch (propertyInfo.PropertyType.Name.ToString())
                            {
                                case "DateTime":
                                    switch (Attr.Format)
                                    {
                                        case "yyyyMMdd":

                                            if (propertyInfo.Name == "CoverageEndDate" && Value.Substring(0, 10) == "12/31/2999")
                                            {
                                                Value = "00000000";
                                            }
                                            else
                                            {
                                                //Convert Var to Datetime object so we can use the ToString function to format date
                                                dtObject = (DateTime)Convert.ChangeType(Value, typeof(DateTime));
                                                Value = dtObject.Date.ToString(Attr.Format);
                                            }
                                            break;

                                        case "HHmmss":

                                            //Convert Var to Datetime object so we can use the ToString function to format date
                                            dtObject = (DateTime)Convert.ChangeType(Value, typeof(DateTime));
                                            Value = dtObject.ToString(Attr.Format);
                                            break;

                                        case "MM/dd/yyyy":
                                            //Convert Var to Datetime object so we can use the ToString function to format date
                                            dtObject = (DateTime)Convert.ChangeType(Value, typeof(DateTime));
                                            Value = dtObject.Date.ToString(Attr.Format);
                                            break;
                                    }
                                    break;

                                case "Decimal":

                                    switch (Attr.Format.ToUpper())
                                    {
                                        case "REMOVEDECIMAL":
                                            numericString = Convert.ToString(Value);

                                            Value = (numericString.Contains("-")) ? "-" + numericString.Replace("-", "").Replace(".", "").PadLeft(Attr.Length - 1, '0') : numericString.Replace(".", "").PadLeft(Attr.Length, '0');
                                            break;
                                    }

                                    break;

                                //Holder for int types
                                case "Int32":

                                    switch (Attr.Format.ToUpper())
                                    {
                                        case "PADZERO":
                                            numericString = Convert.ToString(Value);

                                            Value = Attr.Padding == Padding.Left ? numericString.PadLeft(Attr.Length, '0') : numericString.PadRight(Attr.Length, '0');
                                            break;
                                    }

                                    break;
                            }
                        }

                        Value = Attr.Padding == Padding.Left ? Value.PadLeft(Attr.Length) : Value.PadRight(Attr.Length);
                        Writer.Write(Value);
                    }
                }

                Writer.WriteLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region FUNCTION: static List<T> DeserializeToList<T>(Stream source) where T : class, new()

        public static List<T> DeserializeToList<T>(Stream source) where T : class, new()
        {
            List<T> lstObject;
            StreamReader srFile = null;
            int intLineCount = 0;
            DateTime dtObject;
            string strPropertyName = string.Empty;
            string strValue = string.Empty;

            try
            {
                var properties = GetProperties(typeof(T));

                lstObject = new List<T>();
                var obj = new T();
                srFile = new StreamReader(source);

                while (!srFile.EndOfStream)
                {
                    string strLine = srFile.ReadLine();
                    int attrIndex = 0;
                    var attributeType = typeof(FlatFileAttribute);
                    obj = new T();
                    intLineCount++;                    

                    foreach (var propertyInfo in properties)
                    {                       
                        //Use value for Debugging purposes
                        strPropertyName = propertyInfo.Name;

                        //new
                        var attr = (FlatFileAttribute)propertyInfo.GetCustomAttributes(attributeType, false).First();
                        var value = strLine.Substring(attrIndex, attr.Length).Trim();
                        attrIndex = attrIndex + attr.Length;

                        //Use value for debugging purposes
                        strValue = value.ToString();

                        if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            //switch (attr.StringFormat.ToUpper())
                            switch (attr.Format.ToUpper())
                            {
                                case "YYYYMMDD":
                                    //Convert Var to Datetime object so we can use the ToString function to format date
                                    dtObject = (DateTime)Convert.ChangeType(value, typeof(DateTime));
                                    value = dtObject.Date.ToString(attr.Format);
                                    break;

                                case "MM/DD/YYYY":
                                    DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtObject);
                                    value = dtObject.Date.ToString(attr.Format);
                                    break;
                            }
                        }

                        //*********************************************************************************************
                        //Assign null value to object - when value is all empty or an invalid date
                        if (value.Length == 0 | (propertyInfo.PropertyType == typeof(DateTime) & value == "01/01/0001"))
                        {
                            propertyInfo.SetValue(obj, null, null);
                        }
                        else if (propertyInfo.PropertyType != typeof(string))
                        {
                            propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(obj, value.Trim(), null);
                        }
                    }

                    lstObject.Add(obj);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error on Line: [" + intLineCount + "]" + ", Property Name: [" + strPropertyName + "], Value: [" + strValue + "]. Function: " + MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                if (srFile != null)
                {
                    srFile.Close();
                }
            }

            return lstObject;
        }

        #endregion

        #region FUNCTION: DataTable ConvertTo<T>(List<T> list)

        public static DataTable ConvertTo<T>(List<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }
        #endregion

        #region FUNCTION: DataTable CreateTable<T>()

        private static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
        #endregion

    }
}
