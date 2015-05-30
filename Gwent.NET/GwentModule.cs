using Autofac;

namespace Gwent.NET
{
    public class GwentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .SingleInstance()
                .AsImplementedInterfaces();


            base.Load(builder);
        }
    }
}