using BookApplication.Domain.Identity;
using BookApplication.Repository;
using BookApplication.Repository.Implementation;
using BookApplication.Repository.Interface;
using BookApplication.Service;
using BookApplication.Service.Implementation;
using BookApplication.Service.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<BookAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUserRepository),typeof(UserRepository));
builder.Services.AddScoped(typeof(IOrderRepository),typeof(OrderRepository));



builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<IPublisherService, PublisherService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IShoppingCartsService,ShoppingCartService>();
builder.Services.AddTransient<IBookInShoppingCart,BookInShoppingCartService>();
builder.Services.AddTransient<IOrderService,OrderService>();
builder.Services.AddTransient<IBookInOrderService,BookInOrderService>();

builder.Services.AddTransient<MainService>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
