using System;
using System.Web;
using CodeCampServerLite.Infrastructure.DataAccess;
using StructureMap;

namespace CodeCampServerLite.UI.Helpers
{
    public class TransactionBoundaryModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
        }

        public void Dispose() { }

        private static void context_BeginRequest(object sender, EventArgs e)
        {
            var instance = ObjectFactory.GetInstance<ITransactionBoundary>();
            instance.Begin();
        }

        private static void context_EndRequest(object sender, EventArgs e)
        {
            var instance = ObjectFactory.GetInstance<ITransactionBoundary>();
            try
            {
                instance.Commit();
            }
            catch
            {
                instance.RollBack();
                throw;
            }
            finally
            {
                instance.Dispose();
            }
        }
    }
}