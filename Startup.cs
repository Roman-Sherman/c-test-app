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
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
                .AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddMvcOptions(options =>
                {
                    // バリデーション例外処理
                    options.Filters.Add(new RestErrorFilter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    // 自動的な 400 応答を無効にする
                    // ref : https://docs.microsoft.com/ja-jp/aspnet/core/web-api/?view=aspnetcore-3.0#disable-automatic-400-response
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddNewtonsoftJson(options =>
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
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
            });

            // CORS設定
            services
                .AddCors(options =>
                    options.AddPolicy("AllowAll", p => p.WithOrigins("http://localhost:3000", "http://localhost").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.NET Core Web App Sample API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Log4Netによるロギングを有効にする
            loggerFactory.AddLog4Net();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET Core Web App Sample API V1");
            });

            // ヘルスチェックを有効にする
            app.UseHealthChecks("/api/health", port: 8080);
            app.UseRouting();

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
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
