using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace API.Service
{
    public class ModuleService : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PaymentService>().As<IPaymentService>().InstancePerLifetimeScope();
        }
    }
}