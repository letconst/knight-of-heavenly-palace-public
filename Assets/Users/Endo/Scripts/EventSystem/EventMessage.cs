public abstract class EventMessage
{
}

public abstract class EventMessage<E> : EventMessage
    where E : EventMessage<E>, new()
{
    protected static readonly E cache = new();

    public static E GetEvent() => cache;
}

public abstract class EventMessage<E, P1> : EventMessage
    where E : EventMessage<E, P1>, new()
{
    protected static readonly E cache = new();

    protected P1 param1;

    public static E GetEvent(P1 param1)
    {
        cache.param1 = param1;

        return cache;
    }
}

public abstract class EventMessage<E, P1, P2> : EventMessage
    where E : EventMessage<E, P1, P2>, new()
{
    protected static readonly E cache = new();

    protected P1 param1;
    protected P2 param2;

    public static E GetEvent(P1 param1, P2 param2)
    {
        cache.param1 = param1;
        cache.param2 = param2;

        return cache;
    }
}

public abstract class EventMessage<E, P1, P2, P3> : EventMessage
    where E : EventMessage<E, P1, P2, P3>, new()
{
    protected static readonly E cache = new();

    protected P1 param1;
    protected P2 param2;
    protected P3 param3;

    public static E GetEvent(P1 param1, P2 param2, P3 param3)
    {
        cache.param1 = param1;
        cache.param2 = param2;
        cache.param3 = param3;

        return cache;
    }
}

public abstract class EventMessage<E, P1, P2, P3, P4> : EventMessage
    where E : EventMessage<E, P1, P2, P3, P4>, new()
{
    protected static readonly E cache = new();

    protected P1 param1;
    protected P2 param2;
    protected P3 param3;
    protected P4 param4;

    public static E GetEvent(P1 param1, P2 param2, P3 param3, P4 param4)
    {
        cache.param1 = param1;
        cache.param2 = param2;
        cache.param3 = param3;
        cache.param4 = param4;

        return cache;
    }
}
