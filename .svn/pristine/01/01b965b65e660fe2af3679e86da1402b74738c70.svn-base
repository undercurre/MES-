using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JZ.IMS.WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
            //加载job程序集
            List<System.Reflection.Assembly> assemblies = new List<System.Reflection.Assembly>();
            String path = System.AppDomain.CurrentDomain.BaseDirectory + "JZ.IMS.Job.dll";
            System.Reflection.Assembly assembly = Core.Utilities.ReflectHelper.LoadAssembly(path);
            assemblies.Add(assembly);
            Core.Utilities.ReflectHelper.Initialized(assemblies);
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("host.json", true).Build();
			var host = CreateWebHostBuilder(args).UseConfiguration(config).Build();
			host.Run();
            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
