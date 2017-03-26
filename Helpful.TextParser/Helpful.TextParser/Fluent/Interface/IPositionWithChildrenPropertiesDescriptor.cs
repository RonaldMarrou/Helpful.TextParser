using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionWithChildrenPropertiesDescriptor<TClass>
    {
        void Properties(Action<IPositionWithChildrenPropertyDescriptor<TClass>> properties);
    }
}
