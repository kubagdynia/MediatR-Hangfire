namespace MediatRTest.Invoices.Tests.Fakes;

// This is a simple class that uses Interlocked methods to perform atomic operations on an integer field.
public class Counter
{
    private int _count = 0;

    public int Decrement() => Interlocked.Decrement(ref _count);

    public int Add(int value) => Interlocked.Add(ref _count, value);

    public int Get() => Interlocked.CompareExchange(ref _count, 0, 0);

    public int Increment() => Interlocked.Increment(ref _count);

    public int Subtract(int value) => Interlocked.Add(ref _count, -value);
}