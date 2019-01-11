using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZnjTest.BLL;
using ZnjTest.DAL;
using ZnjTest.IBLL;
using ZnjTest.IDAL;
using ZnjTest.Model;

namespace ZnjTest.MisApi
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connectString = Configuration.GetSection("MysqlConnection").Value;
            services.AddDbContext<ZnjTestContext>(options => options.UseMySql(connectString));

            #region autofac配置
            var builder = new ContainerBuilder();
            builder.Populate(services);//管道寄居
             //builder.RegisterAssemblyTypes(typeof(Startup).Assembly).AsImplementedInterfaces();
            AutofacConfig(builder);
            ApplicationContainer = builder.Build();//IUserService UserService 构造 
            #endregion
            return new AutofacServiceProvider(ApplicationContainer);//将autofac反馈到管道中
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
        /// <summary>
        /// autofac 注册
        /// </summary>
        /// <param name="builder"></param>
        private void AutofacConfig(ContainerBuilder builder)
        {
            var iServices = Assembly.Load("ZnjTest.IBLL");
            var services = Assembly.Load("ZnjTest.BLL");
            //根据名称约定（服务层的接口和实现均以Services结尾），实现服务接口和服务实现的依赖
            builder.RegisterAssemblyTypes(iServices, services)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().PropertiesAutowired();


            var iDal = Assembly.Load("ZnjTest.IDAL");
            var dal = Assembly.Load("ZnjTest.DAL");

            builder.RegisterAssemblyTypes(iDal, dal)
                .Where(t => t.Name.EndsWith("Dal"))
                .AsImplementedInterfaces().PropertiesAutowired();

            //泛型注入
            builder.RegisterGeneric(typeof(BaseDal<>)).As(typeof(IBaseDal<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>)).InstancePerDependency();
        }
    }
}
