partial class Program
{
    private static void MethodA()
    {
        TaskTitle("Starting Method A...");
        OutputThreadInfo();
        Thread.Sleep(3000); // Simulate three seconds of work.
        TaskTitle("Finished Method A.");
    }
    private static void MethodB()
    {
        TaskTitle("Starting Method B...");
        OutputThreadInfo();
        Thread.Sleep(2000); // Simulate two seconds of work.
        TaskTitle("Finished Method B.");
    }
    private static void MethodC()
    {
        TaskTitle("Starting Method C...");
        OutputThreadInfo();
        Thread.Sleep(1000); // Simulate one second of work.
        TaskTitle("Finished Method C.");
    }

    private static decimal CallWebService()
    {
        TaskTitle("Strating call to web serivice");
        OutputThreadInfo();
        Thread.Sleep(Random.Shared.Next(2000, 4000));
        TaskTitle("Finished call to web service");
        return 89.99M;
    }

    private static string CallStoreProcedure(decimal amount)
    {
        TaskTitle("Starting call to store procedure...");
        OutputThreadInfo();
        Thread.Sleep((Random.Shared.Next(2000, 4000)));
        TaskTitle("Finished call to store procedure...");
        return $"12 products cost more than {amount:C}.";
    }
}