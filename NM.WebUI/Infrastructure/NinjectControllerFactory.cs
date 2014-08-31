using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using NM.Domain.Abstract;
using NM.Domain.Concrete;

namespace NM.WebUI.Infrastructure {
    public class NinjectControllerFactory : DefaultControllerFactory {
        private readonly IKernel _ninjectKernel;

        public NinjectControllerFactory() {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType) {
            return controllerType == null
                       ? null
                       : (IController) _ninjectKernel.Get(controllerType);
        }

        private void AddBindings() {
            _ninjectKernel.Bind<INodeRepository>()
                          .To<EFNodeRepository>();
            _ninjectKernel.Bind<INodeLinkRepository>()
                          .To<EFNodeLinkRepository>();
        }
    }
}