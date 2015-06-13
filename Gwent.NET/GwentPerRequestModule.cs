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
                //.InstancePerRequest()
                .AsImplementedInterfaces();
            base.Load(builder);
        }
    }
}