using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using aspdotnet_managesys.Common;
using aspdotnet_managesys.Controllers;
using aspdotnet_managesys.Logger;
using aspdotnet_managesys.Models;
using aspdotnet_managesys.Repositories;
using aspdotnet_managesys.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace aspdotnet_managesys
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // データベースのヘルスチェックを有効化する
            services.AddHealthChecks()
                    .AddDbContextCheck<BookRepository>();

            // サービスの登録(DI)
            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<MasterService, MasterService>();
            services.AddScoped<BookService, BookService>();

            if (string.IsNullOrEmpty(Configuration.GetConnectionString("NpgSqlConnection")))
            {
                // EntityFrameworkCoreにリポジトリを登録
                services.AddDbContext<BookRepository>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                // EntityFrameworkCoreにリポジトリを登録
                services.AddDbContext<BookRepository>(options => options.UseNpgsql(Configuration.GetConnectionString("NpgSqlConnection")));
            }

            // ASP.NET Core MVCの設定
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddMvcOptions(options =>
                {
                    // バリデーション例外処理
                    options.Filters.Add(new RestErrorFilter());
                })
                .AddJsonOptions(options =>
                {
                    // JSONデータのカスタマイズ
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            // ASP.NET Core Identityの認証設定
            services
                .AddIdentity<Account, AccountRole>()
                .AddEntityFrameworkStores<BookRepository>()
                .AddDefaultTokenProviders();

            // Cookie認証を利用する、認証失敗時は401を返す
            services.ConfigureApplicationCookie(options =>
			{
				options.Events.OnRedirectToLogin = context =>
				{
					context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
					return Task.CompletedTask;
				};
			});
			
            // CORS設定
            services
                .AddCors(options =>
                    options.AddPolicy("AllowAll", p => p.WithOrigins("http://localhost:3000", "http://localhost").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
            
            // Swaggerをサービスに登録する
            // http://localhost:8080/swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ManagementSystem API",
                    Description = "A simple example ASP.NET Core Web API",
                    License = new License { Name = "Use under LICX", Url = "https://example.com/license" }
                 });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Log4Netによるロギングを有効にする
            loggerFactory.AddLog4Net();

            // Swaggerを有効にする
            app.UseSwagger();

            // Swagger UIを有効にする
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // ヘルスチェックを有効にする
            app.UseHealthChecks("/api/health", port: 8080);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("AllowAll");
                MasterDataGenerator.InitializeAsync(app.ApplicationServices);
            }

            if (env.IsEnvironment("Docker"))
            {
                app.UseCors("AllowAll");
                MasterDataGenerator.InitializeAsync(app.ApplicationServices);
            }

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
