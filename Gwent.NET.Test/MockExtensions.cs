using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using Autofac.Core;
using Moq;

namespace Gwent.NET.Test
{
    internal static class MockExtensions
    {
        public static void SetupResolve<TContext, TService>(this Mock<TContext> context, object result)
            where TContext : class, IComponentContext
        {
            var componentRegistration = It.IsAny<IComponentRegistration>();
            context.Setup(c =>
                c.ComponentRegistry.TryGetRegistration(It.Is<Service>(s => ((TypedService)s).ServiceType == typeof(TService)),
                out componentRegistration))
                .Returns(true);
            context.Setup(s => s.ResolveComponent(componentRegistration, It.IsAny<IEnumerable<Parameter>>()))
                   .Returns(result);
        }
    }
}
