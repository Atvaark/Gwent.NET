using Autofac;
using Gwent.NET.Repositories;

namespace Gwent.NET
{
    public class GwentPerRequestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .InstancePerRequest()
                .AsImplementedInterfaces();

            builder.RegisterType<GwintContext>()
                .InstancePerRequest()
                .AsImplementedInterfaces();
            base.Load(builder);
        }
    }
}