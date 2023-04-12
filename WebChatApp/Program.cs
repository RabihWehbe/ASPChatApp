using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebChatApp.Middlewares;
using WebChatApp.Model;
using WebChatApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//configure service to use database and the adding identity together
//first generic is the user and the second is the role
//bind it then to the entity framework store that we have
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

//configure the direction path of failed access to an authorized page:
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Welcome";
});


//Add my services:
builder.Services.AddScoped<UserInfo>();
builder.Services.AddScoped<RegisterUser>();
builder.Services.AddScoped<ContactsService>();

/*
 the references of services in DI container is stored and used in server side and not in client side,
so when we register a singleton service this means its data will be shared among all users so, make sure when to use it.
 */
//builder.Services.AddSingleton<ListsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//we set up the service to use the Authentication middleware
app.UseAuthentication();

app.UseAuthorization();


/*
 if we register a middleware here, every time we send a http request to this application this middleware is going to run,
to make a middle run only for specific request, apply it here, but using StartsWithSegements method
 */
app.UseMiddleware<GetUserInfoMiddleware>();


//app.MapWhen(context => context.Request.Path.StartsWithSegments("/my-path"),
//        branch =>
//        {
//            branch.UseMiddleware<GetUserInfoMiddleware>();
//        });

app.MapRazorPages();


app.Run();
