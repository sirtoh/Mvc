// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures
{
    internal class TempDataApplicationModelProvider : IApplicationModelProvider
    {
        private readonly MvcViewOptions _options;

        public TempDataApplicationModelProvider(IOptions<MvcViewOptions> options)
        {
            _options = options.Value;
        }

        /// <inheritdoc />
        /// <remarks>This order ensures that <see cref="TempDataApplicationModelProvider"/> runs after the <see cref="DefaultApplicationModelProvider"/>.</remarks>
        public int Order => -1000 + 10;

        /// <inheritdoc />
        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        /// <inheritdoc />
        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var keyPrefix = _options.SuppressTempDataAttributePrefix ? string.Empty : "TempDataProperty-";

            foreach (var controllerModel in context.Result.Controllers)
            {
                var modelType = controllerModel.ControllerType.AsType();

                var tempDataProperties = LifecycleProperty.GetLifecycleProperties(modelType, typeof(TempDataAttribute), keyPrefix);
                if (tempDataProperties == null)
                {
                    continue;
                }

                foreach (var property in tempDataProperties)
                {
                    SaveTempDataPropertyFilterBase.ValidateTempDataProperty(property.PropertyInfo);
                }

                var filter = new ControllerSaveTempDataPropertyFilterFactory(tempDataProperties);
                controllerModel.Filters.Add(filter);
            }
        }
    }
}
