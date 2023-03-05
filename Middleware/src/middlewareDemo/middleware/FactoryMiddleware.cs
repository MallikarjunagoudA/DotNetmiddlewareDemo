using middlewareDemo.interfaces;

namespace middlewareDemo.middleware;

public class FactoryMiddleware : IMiddleware
{
    private DummyInterface _dummy;

    /*
     *  Need to use service by DI through the ctor
     */
    public FactoryMiddleware(DummyInterface dummy)
    {
        _dummy = dummy;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
       await context.Response.WriteAsJsonAsync($"this is from the Factory middleware {_dummy.MyName()}");
    }
}

