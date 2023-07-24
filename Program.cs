using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Configuration;
using tinderr.Data;
using tinderr.Models;
using tinderr.Services;
using static tinderr.Data.SeedData;
using Swashbuckle.AspNetCore.SwaggerUI;
using tinderr.Hubs;

var builder = WebApplication.CreateBuilder(args);


//cấu hình lại config hệ thống với file applicatin.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//api controller
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Documentation", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

//Add connetdatabase
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("MSSQL")));

//add identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<IIdentityDataInitializer, IdentityDataInitializer>();

//add sinalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 102400000;// max size là 1gb data
});


//add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();

    });
    
    options.AddPolicy("production1", builder =>
    {
        builder.WithOrigins("http://tinderr.id.vn").AllowAnyMethod().AllowAnyHeader().AllowCredentials();

    });

    options.AddPolicy("production2", builder =>
    {
        builder.WithOrigins("https://tinderr.id.vn").AllowAnyMethod().AllowAnyHeader().AllowCredentials();

    });

});

//add service system
builder.Services.AddScoped<ICommon, Common>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

//thêm session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "State1";
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Thời gian không hoạt động trước khi phiên hết hạn
    options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập thông qua HTTP
    options.Cookie.IsEssential = true; // Đảm bảo phiên vẫn hoạt động ngay cả khi người dùng không đồng ý cookie
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "Tinderr"; // Tên cookie
    options.Cookie.Domain = "scammer.click"; // Tên miền cookie áp dụng (nếu có)
    options.Cookie.Path = "/"; // Đường dẫn cookie áp dụng
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Chính sách bảo mật (SameAsRequest, Always, None)
    options.Cookie.HttpOnly = true; // Cookie chỉ được truy cập bằng HTTP (không bằng JavaScript)
    options.Cookie.SameSite = SameSiteMode.Strict; // Giới hạn cookie chỉ được gửi trong cùng nguồn (Strict, Lax, None)
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Thời gian hết hạn của cookie
    options.SlidingExpiration = true; // Cho phép cập nhật thời gian hết hạn khi có hoạt động từ người dùng
    options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn chuyển hướng khi truy cập bị từ chối
});

// Thêm giới hạn kích thước file gửi lên
var maxRequestBodySize = builder.Configuration.GetValue<long>("MaxRequestBodySize");
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = maxRequestBodySize; // Kích thước tối đa của nội dung yêu cầu multipart
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// lấy ip client truy cập web
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});


app.UseRouting();
app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseCors("production1");
app.UseCors("production2");



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation v1");
//    c.RoutePrefix = string.Empty; // Hiển thị Swagger UI ngay từ root URL
//    c.DocumentTitle = "API Documentation";
//    c.DocExpansion(DocExpansion.List);
//});


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<ChatHub>("/chatHub");
//    endpoints.MapHub<GameHub>("/gameHub");
//});


app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/Home/Index");
        return Task.CompletedTask;
    });
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapHub<ChatHub>("/chatHub").AllowAnonymous();
    endpoints.MapHub<GameHub>("/gameHub").AllowAnonymous();
});

// Đặt cấu hình Swagger UI ngay từ root URL
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation v1");
    c.RoutePrefix = "api";
    c.DocumentTitle = "API Documentation";
    c.DocExpansion(DocExpansion.List);
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var initializer = services.GetRequiredService<IIdentityDataInitializer>();
    await initializer.SeedData(
        services.GetRequiredService<UserManager<ApplicationUser>>(),
        services.GetRequiredService<RoleManager<IdentityRole>>()
    );
}

app.Run();
