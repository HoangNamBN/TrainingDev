using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// đăng ký eTrainingDbContext sử dụng kết nối đến SQL Server
builder.Services.AddDbContext<eTrainingDbContext>(options =>
{
    string strConnect = builder.Configuration.GetConnectionString("eTrainingConnection");
    options.UseSqlServer(strConnect, b => b.MigrationsAssembly("eTrainingSolution.DbMigrator"));
});
/*
    Đăng ký các dịch vụ Identity với cấu hình mặc định cho User và Role
    Thêm Token Provider để phát sinh mã token khi mà reset mật khẩu, confirm email, ...
 */
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<eTrainingDbContext>().AddDefaultTokenProviders().AddDefaultUI();

// truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
    // thiết lập về việc khóa User sau 5 lần đăng nhập thất bại
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // thiết lập cấu hình cho Password
    options.Password.RequiredLength = 8; // số ký tự tối thiểu của password
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

    // thiết lập tên email là duy nhất
    options.User.RequireUniqueEmail = true;

    // cấu hình xác thực địa chỉ email tức là email phải tồn tại. Việc này áp dụng khi mà ngườ dùng đăng ký thì gửi mail kích hoạt, reset password thì gửi mail kích hoạt, ...
    options.SignIn.RequireConfirmedEmail = false;
    // khi đăng ký xong thì xác thực tài khoản
    options.SignIn.RequireConfirmedAccount = false;
});

// thiết lập đường dẫn đến các trang
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/"; // thiết lập đường dẫn đến trang login
    options.LogoutPath = "/logout/"; // thiết lập đường dẫn để logout
    options.AccessDeniedPath = "/Account/AccessDenied"; // thiết lập đường dẫn khi mà không có quyền truy cập
});

// đăng ký dịch vụ xác thực bằng Email

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // phục hồi thông tin đăng nhập (Xác thực)
app.UseAuthorization(); // phục hồi thông tin về quyền của user

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
