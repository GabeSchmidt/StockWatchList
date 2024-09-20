using System.ComponentModel;

namespace CodingDemo
{
    public static class Extensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            if (value == null) { return ""; }

            var type = value.GetType();
            var field = type.GetField(value.ToString());
            var custAttr = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            DescriptionAttribute attribute = custAttr?.SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
