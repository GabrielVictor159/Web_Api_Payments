using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Repository;
using API.Service;
using Autofac;
using TEST.Infraestructure;
using Xunit.Abstractions;
using Xunit.Frameworks.Autofac;

[assembly: TestFramework("TEST.ConfigureTestFramework", "TEST")]
namespace TEST
{
    public class ConfigureTestFramework : AutofacTestFramework
    {
        public ConfigureTestFramework(IMessageSink diagnosticMessageSink)
          : base(diagnosticMessageSink)
        {
        }
        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ModuleInfra());
            builder.RegisterModule(new ModuleRepository());
        }
    }
}