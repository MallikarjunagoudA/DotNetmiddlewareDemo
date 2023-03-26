using middlewareDemo.interfaces;
using middlewareDemo.middleware;
using middlewareDemo.service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<FactoryMiddleware>();
builder.Services.AddSingleton<IMiddleware,FactoryMiddleware>();
builder.Services.AddSingleton<DummyInterface, Dummy>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//to get the request and response. the http context is the global variable which holds the instance of request and response objects
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//adding service to create the policy for the cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev-env-policy", builder =>
    {
        builder
                .AllowAnyOrigin() //Access-control-allow-origin: *
                .AllowAnyHeader() //Access-control-allow-Header: *
                .AllowAnyMethod(); // for allowing unsafe requests Access-control-allow-origin: *
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Prod-env-policy", builder =>
    {
        builder
                .WithOrigins("https://www.google.com/")
                .WithMethods("GET", "POST", "PUT", "DELETE");
    });
});

// the above the builder.build for register the services
var app = builder.Build();
// The below builder.build for register middleware


#region inline middleware , comment this region to check other region and vs is true
//** Inline Middleware

//app.Use(async (HttpContext context, Func<Task> next) =>
//{
//    app.Logger.LogInformation("before the 1st middleware");

//    //to pass the control to next middleware we can use app.next() method or next()

//    await next(); // await next.Invoke();

//    app.Logger.LogInformation("exiting first middleware");
//});

//app.Use(async (HttpContext context, Func<Task> next) =>
//{
//    app.Logger.LogInformation("before the second middleware");

//    //to pass the control to next middleware we can use app.next() method or next()

//    await next(); // await next.Invoke();

//    app.Logger.LogInformation("exiting second middleware");
//});



////app.map() will help to map a path or a end point. route: https://localhost:7026/map
//app.Map("/map", (app) =>
//{
//    app.UseRouting();
//    app.Run(async (context) =>
//    {
//        await context.Response.WriteAsJsonAsync(new
//        {
//            message = "app map"
//        });
//    });
//});
//// app.Run terminates the flow.
//app.Run(async (context) =>
//{

//app.Logger.LogInformation("in the terminate middleware");

//   await context.Response.WriteAsJsonAsync(new
//    { 
//        message = "hello world"
//    });
//});

#endregion

#region convention middleware
//app.UseMiddleware<ConventionalMiddleware>();
#endregion

#region Factory middleware
//app.UseMiddleware<FactoryMiddleware>();
#endregion

#region Unhandled middleware
app.UseMiddleware<UnHandledMiddleware>();
#endregion

#region correlation middleware
//app.UseMiddleware<CoRelationalMiddleware>();
#endregion

#region cors middleware
//cors middleware
/*
 * optionally attach the middleware based on the evn
 */
if (app.Environment.IsDevelopment())
{
    app.UseCors("Dev-env-policy");
}

if (app.Environment.IsProduction())
{
    app.UseCors("Prod-env-policy");
}


#endregion
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// used to access the wwwroot folder contents publicly.
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
