using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Text.Json;

namespace middlewareDemo.middleware;

public class UnHandledMiddleware
{
    private RequestDelegate _next;

    public UnHandledMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	//IwebHostEnvioronment will give the current hosting env, and by this we can make a check.
	public async Task Invoke(HttpContext context, IWebHostEnvironment webHostEnvironment)
	{
		try
		{
			await _next(context);
		}
		catch(Exception ex) 
		{
			// in prod layout we not supposed to give exact details. its easy for a hacker to get to know the layout of project.

			/*
			 * Inspect ASPNETCORE_ENVIRONMENT	environment variable
			 * looking at its value it will decide which environment it is.
			 */
			context.Response.StatusCode = 500;
			ProblemDetails details;
			if(webHostEnvironment.IsDevelopment())
			{
				details = new ProblemDetails
				{
					Type = ex.GetType().ToString(),
					Detail = ex.Message,
					Status = 500
				};
			}
			else
			{
                details = new ProblemDetails
                {
                    Type = ex.GetType().ToString(),
                    Detail = "an error occured",
					Status = 500
                };
            }
		await context.Response.WriteAsync(JsonSerializer.Serialize(details));
		}
	}

}
