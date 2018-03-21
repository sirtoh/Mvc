// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.ApplicationParts
{
    public class DefaultApplicationPartFactory : ApplicationPartFactory
    {
        public static DefaultApplicationPartFactory Instance { get; } = new DefaultApplicationPartFactory();

        public static IEnumerable<ApplicationPart> GetDefaultApplicationParts(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            yield return new AssemblyPart(assembly);
        }

        public override IEnumerable<ApplicationPart> GetApplicationParts(Assembly assembly)
        {
            return GetDefaultApplicationParts(assembly);
        }
    }
}
