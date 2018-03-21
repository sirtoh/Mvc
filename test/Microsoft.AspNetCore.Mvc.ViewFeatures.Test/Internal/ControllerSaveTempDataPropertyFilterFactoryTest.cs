﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures.Internal
{
    public class ControllerSaveTempDataPropertyFilterFactoryTest
    {
        [Fact]
        public void CreateInstance_CreatesFilter()
        {
            // Arrange
            var properties = LifecycleProperty.GetLifecycleProperties(typeof(StringController), typeof(TempDataAttribute));
            var factory = new ControllerSaveTempDataPropertyFilterFactory(properties);

            // Act
            var filter = factory.CreateInstance(CreateServiceProvider());

            // Assert
            var tempDataFilter = Assert.IsType<ControllerSaveTempDataPropertyFilter>(filter);
            Assert.Same(properties, tempDataFilter.Properties);
        }

        private ServiceProvider CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(Mock.Of<ITempDataProvider>());
            serviceCollection.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
            serviceCollection.AddTransient<ControllerSaveTempDataPropertyFilter>();

            return serviceCollection.BuildServiceProvider();
        }

        private class StringController
        {
            [TempData]
            public string StringProp { get; set; }
        }
    }
}
