using System.Reflection;

namespace Helpful.TextParser.Interface
{
    public interface IValueSetter
    {
        bool Set(PropertyInfo propertyInfo, object value, object instance);
    }
}
