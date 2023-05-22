using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Repository;
using Autofac;

namespace TEST.Repository.mock
{
    public class ModuleRepository : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PaymentRepository>().As<IPaymentRepository>().InstancePerLifetimeScope();
        }

    }
}