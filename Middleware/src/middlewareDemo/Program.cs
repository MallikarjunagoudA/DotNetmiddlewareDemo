var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// the above the builder.build for register the services
var app = builder.Build();
// The below builder.build for register middleware


//** Inline Middleware

app.Use(async (HttpContext context, Func<Task> next) =>
{
    app.Logger.LogInformation("before the 1st middleware");

    //to pass the control to next middleware we can use app.next() method or next()

    await next(); // await next.Invoke();

    app.Logger.LogInformation("exiting first middleware");
});

app.Use(async (HttpContext context, Func<Task> next) =>
{
    app.Logger.LogInformation("before the second middleware");

    //to pass the control to next middleware we can use app.next() method or next()

    await next(); // await next.Invoke();

    app.Logger.LogInformation("exiting second middleware");
});



//app.map() will help to map a path or a end point. route: https://localhost:7026/map
app.Map("/map", (app) =>
{
    app.UseRouting();
    app.Run(async (context) =>
    {
        await context.Response.WriteAsJsonAsync(new
        {
            message = "app map"
        });
    });
});
// app.Run terminates the flow.
app.Run(async (context) =>
{

app.Logger.LogInformation("in the terminate middleware");

   await context.Response.WriteAsJsonAsync(new
    { 
        message = "hello world"
    });
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
