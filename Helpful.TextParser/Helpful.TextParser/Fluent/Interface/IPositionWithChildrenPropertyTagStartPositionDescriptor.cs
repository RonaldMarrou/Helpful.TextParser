﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionWithChildrenPropertyTagStartPositionDescriptor
    {
        IPositionWithChildrenPropertyTagEndPositionDescriptor StartPosition(int startPosition);
    }
}