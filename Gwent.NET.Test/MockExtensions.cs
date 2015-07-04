using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Gwent.NET.Events;
using Gwent.NET.Interfaces;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;

namespace Gwent.NET.Test
{
    internal static class MockExtensions
    {
        public static void SetupResolve<TContext, TService>(this Mock<TContext> context, object result)
            where TContext : class, IComponentContext
        {
            var componentRegistrationMock = new Mock<IComponentRegistration>();
            IComponentRegistration componentRegistration = componentRegistrationMock.Object;
            context.Setup(c =>
                c.ComponentRegistry.TryGetRegistration(
                It.Is<Service>(s => ((TypedService)s).ServiceType == typeof(TService)),
                out componentRegistration))
                .Returns(true);
            context
                .Setup(s => s.ResolveComponent(componentRegistration, It.IsAny<IEnumerable<Parameter>>()))
                .Returns(result);
        }
        
        public static void SetupClients(this Mock<IHubCallerConnectionContext<dynamic>> mock)
        {
            dynamic client = new ExpandoObject();
            client.recieveServerEvent = new Action<Event>(e => { });
            mock.Setup(c => c.Client(It.IsAny<string>())).Returns((ExpandoObject)client);
        }

        public static void SetupMapping(this Mock<IUserConnectionMap> mock)
        {
            mock.Setup(m => m.GetConnections()).Returns(Enumerable.Empty<string>());
            mock.Setup(m => m.GetConnections(It.IsAny<string>())).Returns(Enumerable.Empty<string>());
            mock.Setup(m => m.GetUsers()).Returns(Enumerable.Empty<string>());
            mock.Setup(m => m.GetUser(It.IsAny<string>())).Returns((string) null);
        }
    }
}
