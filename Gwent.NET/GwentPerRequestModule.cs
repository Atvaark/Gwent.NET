using System.Data.Entity;
using Autofac;
using Gwent.NET.Repositories;

namespace Gwent.NET
{
    public class GwentPerRequestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GwintContext>()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            builder.RegisterType<GwintContextInitializer>().As<IDatabaseInitializer<GwintContext>>();
            base.Load(builder);
        }
    }
}