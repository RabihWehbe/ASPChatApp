using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebChatApp.Model;

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
    config.LoginPath = "/Login";
});




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

app.MapRazorPages();

app.Run();
