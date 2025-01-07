namespace DecoratorDemonstration;

internal static class Program {
    private static void Main() => Test();

    [Decorator("DecoratorDemonstration.Program.Decorated")]
    public static void Test() => Console.WriteLine("Test");

    public static Action Decorated(Action action) {
        void wrapped() {
            Console.WriteLine("Before");
            action();
            Console.WriteLine("After");
        }

        return wrapped;
    }
}
