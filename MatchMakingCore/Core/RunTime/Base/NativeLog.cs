using System;
namespace MatchMakingCore
{
    public class NativeLog : LxLogInstance
    {
        public override void Error(string error)
        {
            Console.WriteLine($"[ERROR] {error}");
        }

        public override void Log(string log)
        {
            Console.WriteLine($"[LOG] {log}");
        }

        public override void Warning(string warning)
        {
            Console.WriteLine($"[WARNING] {warning}");
        }
    }
}
