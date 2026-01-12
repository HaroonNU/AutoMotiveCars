using AutoMotiveProject.cs.Components;
using AutoMotiveProject.cs.Services;

namespace AutoMotiveProject.cs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<GoogleSheetsDbService>();
            builder.Services.AddSingleton<InvoiceService>();
            builder.Services.AddSingleton<AuthStateService>();
            builder.Services.AddSingleton<AuthService>();
            
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            
            app.MapControllers();

            app.Run();
        }
    }
}
