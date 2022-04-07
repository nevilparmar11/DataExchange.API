using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using DataExchange.API.Serialization;
using DataExchange.API.EventProducer.Interface;
using DataExchange.API.EventProducer;
using Microsoft.Extensions.DependencyInjection;
using DataExchange.API.Config;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace DataExchange.API
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
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new DataExchangeEventConverter()));

            services.AddSingleton<IProducerFactory, ProducerFactory>();
            services.AddSingleton<IDataExchangeProducer, DataExchangeProducer>();

            services.Configure<KafkaSettings>(Configuration.GetSection("Kafka"));
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<KafkaSettings>>().Value);

            services.Configure<Dictionary<string, string>>(Configuration.GetSection("EventNamesToTopicsMapping"));
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<Dictionary<string, string>>>().Value);

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Title = "Data Exchange";
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();

            app.UseSwaggerUi3();
        }
    }
}
