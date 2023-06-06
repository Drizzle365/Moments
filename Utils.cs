using System.Reflection;

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
}