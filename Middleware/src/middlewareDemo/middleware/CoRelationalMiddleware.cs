namespace middlewareDemo.middleware;

public class CoRelationalMiddleware
{
    private readonly RequestDelegate _next;
	private const string CORRELATIONID_HEADER = "x-Correlationid-Id";

    public CoRelationalMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		string correlationdid = Guid.NewGuid().ToString();
		/*
		 Check whether correlationid is present in request header.

		if its not present we are adding it into a header. 
		 */
		if(!(context.Request.Headers.ContainsKey(CORRELATIONID_HEADER) && context.Request.Headers.TryGetValue(CORRELATIONID_HEADER, out _)))
		{
			context.Request.Headers.Add(CORRELATIONID_HEADER, correlationdid);
		}

		context.Response.OnStarting(() =>
		{
            context.Response.Headers.Add(CORRELATIONID_HEADER, correlationdid);

			return Task.CompletedTask;
        });


		/*
		 we cannkot mutate the response after returning to the client.

		because it will throw the protocol vialation.
		 */
		await _next.Invoke(context);
		
	}
}
