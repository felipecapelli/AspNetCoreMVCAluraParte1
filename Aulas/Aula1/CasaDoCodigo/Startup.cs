using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CasaDoCodigo
{
    public class Startup
    {
        public Startup(IConfiguration configuration){ Configuration = configuration;}
        public IConfiguration Configuration { get; }



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            //---------------------------------------------
            string connectionString = Configuration.GetConnectionString("Default"); /*Esse Default foi configurado no arquivo appsettings*/
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            //---------------------------------------------
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}/{id?}");
            });

            //---------------- Criar o banco se não tiver criado
            serviceProvider.GetService<ApplicationContext>().Database.Migrate(); //no final pode usar .EnSureCreated() no lugar de Migrate(), mas daí não vai poder mais fazer migrações
        }
    }
}
