#if UNITY
using UnityEngine;
using MatchMakingCore;

public sealed class UnityLog : LxLogInstance
{
    public override void Error(string error)
    {
        Debug.LogError(error);
    }

    public override void Log(string log)
    {
        Debug.Log(log);
    }

    public override void Warning(string warning)
    {
        Debug.LogWarning(warning);
    }
}
#endif
