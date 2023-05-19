using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infraestructure;
using Autofac;

namespace TEST.Infraestructure
{
    public class ModuleInfra : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContextMock>().As<ApplicationDbContext>().InstancePerLifetimeScope();
        }
    }
}