// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures.Internal
{
    [DebuggerDisplay("{PropertyInfo, nq}")]
    public readonly struct LifecycleProperty
    {
        private readonly PropertyHelper _propertyHelper;
        private readonly bool _isReferenceTypeOrNullable;

        private LifecycleProperty(string key, PropertyHelper propertyHelper)
        {
            Key = key;
            _propertyHelper = propertyHelper;
            var propertyType = propertyHelper.Property.PropertyType;
            _isReferenceTypeOrNullable = !propertyType.IsValueType || Nullable.GetUnderlyingType(propertyType) != null;
        }

        public string Key { get; }

        public PropertyInfo PropertyInfo => _propertyHelper.Property;

        public object GetValue(object instance) => _propertyHelper.GetValue(instance);

        public void SetValue(object instance, object value)
        {
            if (value != null || _isReferenceTypeOrNullable)
            {
                _propertyHelper.SetValue(instance, value);
            }
        }

        public static IReadOnlyList<LifecycleProperty> GetLifecycleProperties(
            Type containerType,
            Type attributeType,
            string keyPrefix = null)
        {
            var propertyHelpers = PropertyHelper.GetVisibleProperties(type: containerType);

            List<LifecycleProperty> results = null;

            for (var i = 0; i < propertyHelpers.Length; i++)
            {
                var propertyHelper = propertyHelpers[i];
                var attribute = propertyHelper.Property.GetCustomAttribute(attributeType);
                if (attribute == null)
                {
                    continue;
                }

                var key = ((IKeyedAttribute)attribute).Key;
                if (key == null)
                {
                    key = keyPrefix + propertyHelper.Name;
                }

               
                if (results == null)
                {
                    results = new List<LifecycleProperty>();
                }

                results.Add(new LifecycleProperty(key, propertyHelper));
            }

            return results;
        }
    }
}
