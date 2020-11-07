namespace MatchMakingCore
{
    public static class LxLog
    {
        private static LxLogInstance _instance;

        public static void Init(LxLogInstance instance)
        {
            _instance = instance;
        }

        public static void Log(string log)
        {
            _instance?.Log(log);
        }

        public static void Warning(string warning)
        {
            _instance?.Warning(warning);
        }

        public static void Error(string error)
        {
            _instance?.Error(error);
        }
    }

    public abstract class LxLogInstance
    {
        public abstract void Log(string log);
        public abstract void Warning(string warning);
        public abstract void Error(string error);
    }
}
