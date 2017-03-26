using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionWithChildrenPropertyWithChildrenDescriptor<TClass>
    {
        IPositionWithChildrenPropertyWithChildrenTagDescriptor Child();

        IPositionWithChildrenPropertyStartPositionDescriptor StartPosition(int position);
    }
}
