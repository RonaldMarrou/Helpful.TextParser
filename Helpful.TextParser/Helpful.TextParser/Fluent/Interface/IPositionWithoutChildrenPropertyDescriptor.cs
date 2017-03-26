using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionWithoutChildrenPropertyDescriptor<TClass>
    {
        IPositionWithoutChildrenPropertyStartPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property);
    }
}
