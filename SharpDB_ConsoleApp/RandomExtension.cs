using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class RandomExtension
{
    /// <summary>
    /// 随机返回true或false
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public static bool NextBool(this Random random)
    {
        return random.Next() % 2 == 1;
    }

    /// <summary>
    /// 随机返回一个时间区间内的 一个时间
    /// </summary>
    public static DateTime NextDateTime(this Random random, DateTime minValue, DateTime maxValue)
    {
        var ticks = minValue.Ticks + (long)((maxValue.Ticks - minValue.Ticks) * random.NextDouble());
        return new DateTime(ticks);
    }

    /// <summary>
    /// 随机返回一个时间
    /// </summary>
    public static DateTime NextDateTime(this Random random)
    {
        return NextDateTime(random, DateTime.MinValue, DateTime.MaxValue);
    }

    /// <summary>
    /// 随机返回一个枚举对象
    /// </summary>
    public static T NextEnum<T>(this Random random)
    where T : struct
    {
        Type type = typeof(T);
        if (type.IsEnum == false) throw new InvalidOperationException();

        var array = Enum.GetValues(type);
        var index = random.Next(array.GetLowerBound(0), array.GetUpperBound(0) + 1);
        return (T)array.GetValue(index);
    }


    /// <summary>
    /// 随机返回 数字字符串
    /// </summary>
    /// <param name="len">字符长度</param>
    public static string NextRdomNumberString(this Random random, int len)
    {
        string val = random.Next(1, 10).ToString();
        for (int i = 1; i < len; i++)
        {
            val = val + random.Next(0, 10).ToString();
        }
        return val;
    }

    /// <summary>
    ///  随机返回 字符串
    /// </summary>
    /// <param name="len">字符长度</param>
    public static string NextRdomString(this Random random, int len)
    {
        string noceStr = string.Empty;
        string strs = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        for (int i = 0; i < len; i++)
        {
            string r = strs[random.Next(0, strs.Length)].ToString();
            noceStr += r;
        }
        return noceStr;
    }

    /// <summary>
    /// 随机返回 数组中的一项
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    /// <param name="arr">数组</param>
    /// <returns></returns>
    public static T NextRdomItem<T>(this Random random,T[] arr)
    {
        if (arr.Length > 0)
        {
            return arr[random.Next(0, arr.Length)];
        }
        else
           return default(T);
    }
}

