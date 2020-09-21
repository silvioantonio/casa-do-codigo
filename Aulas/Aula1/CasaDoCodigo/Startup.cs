using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CasaDoCodigo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            string connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<Applicationcontext>(op => op.UseSqlServer(connectionString));

            services.AddTransient<IDataService, DataService>();

            services.AddTransient<IProdutoRepository, ProdutoRepository>();
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

            // Dessa forma garantimos que o banco de dados seja criado caso nao exista.
            // Injeçao de dependencia utilizando Interface
            //serviceProvider.GetService<Applicationcontext>().Database.EnsureCreated();

            //Utilizando Migrate() eu posso utilizar migraçoes futuramente..utilizando Ensure Created eu nao possu utilizar migraçoes apos o banco ser criado
            serviceProvider.GetService<IDataService>().InicializaDB();
        }
    }

}
