using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Frmwk.Controllers;
using WebUtils.Frmwk.Logging;
using WebUtils.Logging;

namespace Web.Frmwk
{
    public class Global : HttpApplication
    {
        private readonly ILogContract<Global> logger;

        public Global()
        {
            var logMgr = new NLogManagerAdapter();
            this.logger = logMgr.GetLogger<Global>();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                logger.Debug("Application_Start called");
                UnityConfig.RegisterComponents();
                logger.Debug("Registered Unity Components");
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                logger.Debug("Registered Global Filters");
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                logger.Debug("Registered Routes");
                //UnityConfig.RegisterComponents();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                if (exception is HttpException httpException)
                {
                    switch (httpException.GetHttpCode())
                    {
                        case 404:
                            // Page not found.
                            logger.Warn(string.Format("A request for missing resource {0} was made", Request.Url));
                            return;
                        default:
                            logger.Warn(httpException.Message, httpException);
                            break;
                    }
                }

                // Log the exception.
                logger.Error(exception.Message, exception);

                Response.Clear();

                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Index");

                // Pass exception details to the target error View.
                //routeData.Values.Add("error", exception);

                // Clear the error on server.
                Server.ClearError();

                // Avoid IIS7 getting in the middle
                Response.TrySkipIisCustomErrors = true;

                // Call target Controller and pass the routeData.
                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(
                     new HttpContextWrapper(Context), routeData));
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}