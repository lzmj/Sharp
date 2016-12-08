using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


public static class ObjectExtension
{

    #region 简单类型 转换方法
    /// <summary>
    /// 对象类型转换
    /// </summary>
    /// <typeparam name="TResult">返回类型</typeparam>
    /// <param name="obj">对象</param>
    /// <returns></returns>
    public static TResult ConvertTo<TResult>(this object obj)
    {
        Type type = typeof(TResult);
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type != null)
        {
            object result = null;
            if (type.IsEnum)
            {
                result = Enum.Parse(type, obj.ToString(), true);
            }
            else if (type.IsAssignableFrom(typeof(Guid)))
            {
                result = Guid.Parse(obj.ToString());
            }

            if (result != null)
            {
                return (TResult)result;
            }
        }
        return (TResult)Convert.ChangeType(obj, type);
    }

    /// <summary>
    /// 对象类型转换，转换失败，返回默认值
    /// </summary>
    /// <typeparam name="TResult">返回类型</typeparam>
    /// <param name="obj">对象</param>
    /// <param name="def">默认值</param>
    /// <returns></returns>
    public static TResult ConvertTo<TResult>(this object obj, TResult def)
    {
        try
        {
            if (obj == null)
            {
                return def;
            }
            return ConvertTo<TResult>(obj);
        }
        catch
        {
            return def;
        }
    }

    public static object ConvertTo(this object obj, Type conversionType)
    {
        Type type = conversionType;
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type != null)
        {
            object result = null;
            if (type.IsEnum)
            {
                result = Enum.Parse(type, obj.ToString());
            }
            else if (type.IsAssignableFrom(typeof(Guid)))
            {
                result = Guid.Parse(obj.ToString());
            }
            if (result != null)
            {
                return result;
            }
        }
        return Convert.ChangeType(obj, type);
    }

    public static object ConvertTo(this object obj, Type conversionType, object def)
    {
        try
        {
            if (obj == null)
            {
                return def;
            }
            return ConvertTo(obj, conversionType);
        }
        catch
        {
            return def;
        }
    }
    #endregion


    /// <summary>
    /// 判定 对象的类型 是否是 可空类型
    /// </summary>
    /// <param name="theType">对象类型</param>
    /// <returns></returns>
    public static bool IsNullableType(this Type theType)
    {
        return (theType.IsGenericType && theType.
          GetGenericTypeDefinition().Equals
          (typeof(Nullable<>)));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="columnName"></param>
    /// <returns></returns>
    public static List<T> GetFirstCol<T>(this DataTable data, string columnName = "")
    {
        List<T> lst = new List<T>();
        if (data == null || data.Rows.Count <= 0)
        {
            return lst;
        }
        else
        {
            foreach (DataRow dr in data.Rows)
            {
                if (!string.IsNullOrWhiteSpace(columnName))
                {
                    if (dr[columnName] != null)
                    {
                        T t = (T)Convert.ChangeType(dr[columnName], typeof(T));
                        lst.Add(t);
                    }
                }
                else
                {
                    if (dr[0] != null)
                    {
                        T t = (T)Convert.ChangeType(dr[0], typeof(T));
                        lst.Add(t);
                    }
                }
            }
        }
        return lst;
    }


    public static Dictionary<TKey, TVal> GetDict<TKey, TVal>(this DataTable data, string ColumnNameKey, string ColumnVal)
    {
        Dictionary<TKey, TVal> dict = new Dictionary<TKey, TVal>();
        if (data == null || data.Rows.Count <= 0)
        {
            return new Dictionary<TKey, TVal>();
        }
        else
        {
            foreach (DataRow dr in data.Rows)
            {
                TKey k = (TKey)Convert.ChangeType(dr[ColumnNameKey], typeof(TKey));
                TVal v = (TVal)Convert.ChangeType(dr[ColumnVal], typeof(TVal));
                dict.Add(k, v);
            }
        }
        return dict;
    }
    public static Dictionary<string, string> GetDictEnums(this Enum em, bool underlyingInt = false)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        Type tyEnum = em.GetType();
        foreach (string str in tyEnum.GetEnumNames())
        {
            dict.Add(str, str);
            if (underlyingInt)
            {
                dict[str] = Convert.ToInt32(Enum.Parse(tyEnum, str)).ToString();
            }
        }
        return dict;
    }

    public static List<string> GetListEnums(this Enum em, bool underlyingInt = false)
    {
        List<string> lstEnums = new List<string>();
        Type tyEnum = em.GetType();
        foreach (string str in tyEnum.GetEnumNames())
        {
            string item = str;
            if (underlyingInt)
            {
                item = Convert.ToInt32(Enum.Parse(tyEnum, str)).ToString();
            }
            lstEnums.Add(item);
        }
        return lstEnums;
    }

    public static T ConvertToObjectFromDR<T>(this DataRow row)
    {
        T obj = (T)Activator.CreateInstance(typeof(T));
        obj = ConvertToObjectFromDR<T>(obj, row);
        return obj;
    }
    /// <summary>
    /// 将数据库中的值转换为对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static T ConvertToObjectFromDR<T>(T obj, DataRow row)
    {
        Type type = obj.GetType();
        System.Reflection.PropertyInfo[] propInfo = type.GetProperties();
        for (int i = 0; i < propInfo.Length; i++)
        {
            if (row.Table.Columns[propInfo[i].Name] != null && row[propInfo[i].Name] != System.DBNull.Value)
            {
                object objVal = row[propInfo[i].Name];
                Type typeVal = Nullable.GetUnderlyingType(propInfo[i].PropertyType) ?? propInfo[i].PropertyType;
                int mark = 0;
                try
                {
                    if (typeVal.Name == "Guid")
                    {
                        mark = 1;
                        propInfo[i].SetValue(obj, Guid.Parse(objVal.ToString()), null);
                    }
                    else
                    {
                        if (typeVal.IsEnum && objVal != null)
                        {
                            Type tyEnum = Enum.GetUnderlyingType(typeVal);
                            if (tyEnum.IsAssignableFrom(typeof(int)))
                            {
                                mark = 2;
                                propInfo[i].SetValue(obj, Enum.Parse(typeVal, objVal.ToString()), null);
                            }
                            else
                            {
                                mark = 3;
                                propInfo[i].SetValue(obj, Convert.ChangeType(objVal, typeVal), null);
                            }
                        }
                        else
                        {
                            if (objVal == null || string.IsNullOrWhiteSpace(objVal.ToString()))
                            {
                                mark = 4;
                                if (propInfo[i].PropertyType.IsNullableType())
                                {
                                    objVal = null;
                                }
                                propInfo[i].SetValue(obj, objVal, null);
                            }
                            else
                            {
                                mark = 5;
                                propInfo[i].SetValue(obj, Convert.ChangeType(objVal, typeVal), null);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("SetValue出错！(" + mark + ")", propInfo[i].Name + ":" + objVal, ex);
                }
            }
        }
        return obj;
    }

    /// <summary>
    /// 将datatable转换为List对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static List<T> ConvertToListObject<T>(this DataTable data)
    {

        List<T> objs = new List<T>();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj = ConvertToObjectFromDR(obj, data.Rows[i]);
            objs.Add(obj);
        }
        return objs;
    }





    public static string ToUpperFirstword(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        string result = string.Empty;
        string[] words = value.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
        if (value.Length <= 1)
        {
            return value.ToUpper();
        }
        else if (words.Length <= 1)
        {
            //result = value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1).ToLower();
            result = value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1);
        }
        else
        {
            List<string> lst = new List<string>();
            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    string temp = word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1).ToLower();
                    lst.Add(temp);
                }
            }
            result = string.Join("_", lst);
        }
        return result;
    }

}

