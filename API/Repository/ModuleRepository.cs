using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace API.Repository
{
    public class ModuleRepository : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PaymentRepository>().As<IPaymentRepository>().InstancePerLifetimeScope();
        }

    }
}