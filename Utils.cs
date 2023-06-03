using System.Reflection;

namespace Moments;

public class Utils
{
    public static Dictionary<string, object> ObjectToDictionary(object obj)
    {
        var dictionary = new Dictionary<string, object>();

        PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            string propertyName = property.Name;
            object? propertyValue = property.GetValue(obj);

            if (propertyValue != null) dictionary.Add(propertyName, propertyValue);
        }

        return dictionary;
    }
}