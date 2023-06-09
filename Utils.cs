using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Moments;

public static class Utils
{
    public static Dictionary<string, string?> ObjectToDictionary(object obj)
    {
        var dictionary = new Dictionary<string, string?>();

        PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            string propertyName = property.Name;
            object? propertyValue = property.GetValue(obj);

            if (propertyValue != null) dictionary.Add(propertyName, propertyValue.ToString());
        }

        return dictionary;
    }
    
    public static string Md5(string str)
    {
        var inputBytes = Encoding.UTF8.GetBytes(str);
        using MD5 md5 = MD5.Create();
        var outputBytes = md5.ComputeHash(inputBytes);
        var output = BitConverter.ToString(outputBytes).Replace("-", "").ToLower();
        return output;
    }
}