using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Hangfire;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Jobs;
using inventoryAppWebUi.Infrastructures;

namespace inventoryAppWebUi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            yield return new BackgroundJobServer();
        }

        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(configuration => configuration.AddProfile<MappingProfile>());
            HangfireAspNet.Use(GetHangfireServers);
            
            RecurringJob.AddOrUpdate(() => NotificationReminderJob.RunReminder(TimeFrame.WEEKLY), Cron.Daily);
            RecurringJob.AddOrUpdate(() => NotificationReminderJob.RunReminder(TimeFrame.MONTHLY), Cron.Daily);
            RecurringJob.AddOrUpdate(() => NotificationReminderJob.ExpireDrugs(), Cron.Daily);
        }
    }
}
