﻿using System;
using System.Reflection;
using Helpful.TextParser.Interface;

namespace Helpful.TextParser.Impl
{
    public class ValueSetter : IValueSetter
    {
        public bool Set(PropertyInfo propertyInfo, object value, object instance)
        {
            try
            {
                var isGeneric = propertyInfo.PropertyType.IsGenericType;

                if (isGeneric && propertyInfo.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                {
                    return false;
                }

                var conversionType = isGeneric ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

                if (value == null && isGeneric)
                {
                    return true;
                }

                propertyInfo.SetValue(instance, Convert.ChangeType(value, conversionType));

                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }
    }
}
