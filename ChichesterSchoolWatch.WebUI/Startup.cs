using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolWatch.Business;
using SchoolWatch.Business.BudgetExpenditures;
using SchoolWatch.Business.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers;
using SchoolWatch.Business.Something;
using SchoolWatch.Data;
using SchoolWatch.Data.Repositories;
using SchoolWatch.Data.Repositories.Interfaces;

namespace ChichesterSchoolWatch.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBudgetExpendituresService, BudgetExpendituresService>();
            services.AddTransient<IBudgetRevenuesService, BudgetRevenuesService>();
            services.AddScoped<IBudgetsService, BudgetsService>();
            services.AddTransient<IDistrictComparisonService, DistrictComparisonService>();

            services.AddTransient<IDistrictComparer, TotalRevenueComparer>();
            services.AddTransient<IDistrictComparer, CostPerStudentComparer>();
            services.AddTransient<IDistrictComparer, DeficitComparer>();
            services.AddTransient<IDistrictComparer, TaxRateIncreaseComparer>();
            services.AddTransient<IDistrictComparer, SpecialEducation1200Comparer>();
            services.AddTransient<IDistrictComparer, PropertyAssessmentComparer>();
            services.AddTransient<IDistrictComparer, ChichesterCostComparer>();
            services.AddTransient<IDistrictComparer, PsersComparer>();

            services.AddTransient<IDistrictsService, DistrictsService>(); 
            services.AddTransient<IEmployeesRepository, EmployeesRepository>();
            services.AddTransient<IEmployeesService, EmployeesService>();
            services.AddTransient<IEnrollmentsService, EnrollmentsService>();
            services.AddTransient<IExpenditureCodesService, ExpenditureCodesService>();
            services.AddScoped<IFiscalYearsService, FiscalYearsService>(); 
            services.AddTransient<IRevenueCodesService, RevenueCodesService>();
            services.AddTransient<ISomeNewService, SomeNewService>();

            
            services.AddScoped<IBudgetExpendituresRepository, BudgetExpendituresRepository>();
            services.AddScoped<IBudgetRevenuesRepository, BudgetRevenuesRepository>();
            services.AddScoped<IBudgetsRepository, BudgetsRepository>();
            services.AddTransient<IDataEngine, DataEngine>();
            services.AddScoped<IDistrictsRepository, DistrictsRepository>();
            services.AddTransient<IExpenditureCodesRepository, ExpenditureCodesRepository>();
            services.AddScoped<IFiscalYearRepository, FiscalYearRepository>();
            services.AddTransient<IRevenuesRepository, RevenuesRepository>();
            services.AddScoped<IStatePensionRatesRepository, StatePensionRatesRepository>();
            services.AddScoped<ITotalEnrollmentsRepository, TotalEnrollmentsRepository>();


            var connectionString = Configuration.GetConnectionString("readonly");
            services.AddDbContext<SchoolDbContext>(
                options => options.UseMySQL(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("StaticData12Hr",
                    new CacheProfile
                    {
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        Duration = (int) TimeSpan.FromHours(12).TotalSeconds
                    });
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpsRedirection(options => { options.HttpsPort = 443; });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
 
   
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-Frame-Options", "deny");
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff"); 
                    context.Response.Headers.Add("Content-Security-Policy", "default-src https: data: 'unsafe-inline' 'unsafe-eval'");
                    await next();
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
 
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "debug");
                }
            });
        }
    }
}
