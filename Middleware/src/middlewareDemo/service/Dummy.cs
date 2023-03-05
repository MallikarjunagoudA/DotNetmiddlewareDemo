using middlewareDemo.interfaces;

namespace middlewareDemo.service;

public class Dummy : DummyInterface
{
    public string MyName()
    {
        return "Hello";
            
    }
    
}
