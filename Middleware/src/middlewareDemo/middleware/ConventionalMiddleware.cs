using middlewareDemo.interfaces;

namespace middlewareDemo.middleware;

public class ConventionalMiddleware
{
	private readonly RequestDelegate _next;

	/*
	 *  DI the RequestDelegate in the ctor.
	 */
	public ConventionalMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	/*
	 *  Implement the invoke method.
	 */
	public async Task Invoke(HttpContext context, DummyInterface dummy)
	{
		await context.Response.WriteAsJsonAsync($"this is from convention middleware {dummy.MyName()}");
	}

}
