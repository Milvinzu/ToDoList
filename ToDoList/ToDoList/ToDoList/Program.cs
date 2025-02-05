using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Repositories.Interfaces;
using ToDoList.Repositories;
using ToDoList.Services.Interfaces;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

if (useInMemory)
{
    builder.Services.AddDbContext<TododbContext>(options =>
        options.UseInMemoryDatabase("TestDatabase"));
}
else
{
    builder.Services.AddDbContext<TododbContext>(options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
}

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskReminderRepository, TaskReminderRepository>();
builder.Services.AddScoped<ITaskTagRepository, TaskTagRepository>();
builder.Services.AddScoped<ISubtaskRepository, SubtaskRepository>();
builder.Services.AddScoped<ITaskAttachmentRepository, TaskAttachmentRepository>();
builder.Services.AddScoped<ITaskCollaboratorRepository, TaskCollaboratorRepository>();
builder.Services.AddScoped<IRecurringTaskRepository, RecurringTaskRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();


// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
