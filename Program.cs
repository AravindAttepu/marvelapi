

var builder = WebApplication.CreateBuilder(args);
// Add gitignore... learn more about git

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-9.0#generated-clients
// use the above documentation or refer Nick chapsas on youtube to understand how to use http client.
// you should use a scoped client instead for service.
// Understanding this will help you work in microservices architecture a bit more informed
builder.Services.AddHttpClient<MarvelApiService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// I dont see OAuth implemented in this project. just thought this was something you are working on.
app.UseAuthorization();
app.MapControllerRoute(
    name: "root",
    pattern: "",
    defaults: new { controller = "Home", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
