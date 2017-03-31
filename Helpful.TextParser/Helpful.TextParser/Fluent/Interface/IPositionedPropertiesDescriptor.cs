﻿using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertiesDescriptor<TClass> where TClass : class
    {
        void Properties(Action<IPositionedPropertyDescritor<TClass>> properties);
    }
}