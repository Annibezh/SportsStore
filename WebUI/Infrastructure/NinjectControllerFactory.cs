using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;

namespace WebUI.Infrastructure
{
    //реалізація пользовательськой фабрики контроллерів
    //наслідується від фабрики, яка використовується за замовчуванням
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            //створюємо контейнер
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance (RequestContext rc, Type controllerType)
        {
            //отримання обєкта контроллера з контейнера, використовуючи його тип
            return controllerType == null ?
                null : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };
            ninjectKernel.Bind<IOrderProcessor>()
                .To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);
        }
    }
}