using System.Diagnostics; // To use Stopwatch.
using static System.Console;


OutputThreadInfo();
Stopwatch timer = Stopwatch.StartNew();

SectionTitle("Running methods synchronously on one thread.");
MethodA();
MethodB();
MethodC();

WriteLine($"{timer.ElapsedMilliseconds:#,##0}ms elapsed.");
timer.Restart();

SectionTitle("Running methods asynchronously on multiple threads");

Task taskA = new(MethodA);
taskA.Start();
Task taskB = Task.Factory.StartNew(MethodB);
Task taskC = Task.Run(MethodC);

Task[] tasks = { taskA, taskB, taskC };
Task.WaitAll(tasks);

timer.Restart();

SectionTitle("Passing results of the one task as an input into another.");

Task<string> taskServiceThenSProc = Task.Factory
    .StartNew(CallWebService) // returns Task<decimal>
    .ContinueWith(previousTask => // return Task<string>
    CallStoreProcedure(previousTask.Result));


WriteLine($"Result: {taskServiceThenSProc.Result}");

WriteLine($"{timer.ElapsedMilliseconds:#,##0}ms elapsed.");