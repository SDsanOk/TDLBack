using System;
using System.Text;
using CloudCall.Todo.DAL;
using CloudCall.Todo.DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using CloudCall.Todo.Services;
using CloudCall.Todo.Services.Stores;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;


namespace simpleApp
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
            services.AddCors(options => options.AddPolicy("all", builder => builder.WithHeaders("todo-task-header").WithOrigins("localhost:9080").AllowAnyMethod()));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IStore<List>, ListStore>();
            services.AddScoped<IStore<Event>, EventStore>();
            services.AddScoped<IStore<Board>, BoardStore>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0.2", new Info { Title = "TODOListBackend", Version = "v0.2" });
            });

            services.AddAuthentication();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/error/unauthorized");
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
                app.UseHsts();
                app.UseExceptionHandler("/Error/Index");
            }

            app.UseAuthentication();

            //app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0.2/swagger.json", "TODO");
                
            });

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
