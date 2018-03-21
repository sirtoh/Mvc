// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures.Internal
{
    public class LifecyclePropertyTest
    {
        [Fact]
        public void GetLifecycleProperties_GetsPropertiesWithAttributes()
        {
            // Arrange & Act
            var properties = LifecycleProperty.GetLifecycleProperties(typeof(TestSubject), typeof(TestAttribute));

            // Assert
            Assert.Collection(
                properties,
                property =>
                {
                    Assert.Equal(nameof(TestSubject.Property2), property.PropertyInfo.Name);
                    Assert.Equal("Property2", property.Key);
                },
                property =>
                {
                    Assert.Equal(nameof(TestSubject.Property3), property.PropertyInfo.Name);
                    Assert.Equal("Custom", property.Key);
                });
        }

        [Fact]
        public void GetLifecycleProperties_UsesPrefixToCalculateKey()
        {
            // Arrange & Act
            var properties = LifecycleProperty.GetLifecycleProperties(typeof(TestSubject), typeof(TestAttribute), "Prefix-");

            // Assert
            Assert.Collection(
                properties,
                property =>
                {
                    Assert.Equal(nameof(TestSubject.Property2), property.PropertyInfo.Name);
                    Assert.Equal("Prefix-Property2", property.Key);
                },
                property =>
                {
                    Assert.Equal(nameof(TestSubject.Property3), property.PropertyInfo.Name);
                    Assert.Equal("Custom", property.Key);
                });
        }

        public class TestSubject
        {
            public string Property1 { get; set; }

            [Test]
            public object Property2 { get; }

            [Test(Key = "Custom")]
            public DateTime Property3 { get; set; }
        }

        public class TestAttribute : Attribute, IKeyedAttribute
        {
            public string Key { get; set; }
        }
    }
}
