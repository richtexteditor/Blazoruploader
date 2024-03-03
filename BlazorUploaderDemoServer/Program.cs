using BlazorUploaderDemoServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorUploaderDemoServer
{
	public class Program
	{
		static public string UploadDemoTempDir = string.Empty;
		static public string GetTempFileName(string uploaderid, int fileid)
		{
			//uploaderid is generated at serverside and it's safe
			return Path.Combine(UploadDemoTempDir, uploaderid + "_" + fileid + ".dat");
		}

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			UploadDemoTempDir = builder.Configuration["UploadDemoTempDir"];
			if (string.IsNullOrEmpty(UploadDemoTempDir))
			{
				UploadDemoTempDir = Path.Combine(Path.GetTempPath(), "UploadDemoTempDir");
			}
			if (!Directory.Exists(UploadDemoTempDir))
			{
				Directory.CreateDirectory(UploadDemoTempDir);
			}
			builder.Services.AddSignalR(opt =>
			{
				opt.MaximumReceiveMessageSize = 666 * 1024;
			});
			////upload max chunk size
			BlazorUploader.CoreUploader.MaxBufferSize = 512 * 1024;

			// BlazorUploader.demo.lic is compiled into EmbededResource in BlazorUploaderDemoServer.dll
			using var licstream = typeof(Program).Assembly.GetManifestResourceStream("BlazorUploaderDemoServer.BlazorUploader.demo.lic");
			BlazorUploader.CoreUploader.InstallLicense(licstream);





			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddServerSideBlazor();
			builder.Services.AddSingleton<WeatherForecastService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			//app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.MapBlazorHub();
			app.MapFallbackToPage("/_Host");

			app.Run();
		}
	}
}