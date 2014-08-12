using System.Web.Mvc;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(CodeCampServerLite.UI.App_Start.StructuremapMvc), "Start")]

namespace CodeCampServerLite.UI.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
            var container = IoC.Container;
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}