using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine.TestTools;

namespace MatchMaking.Tests
{
    public class MatchmakingTest
    {
        public static Stopwatch Measure(Action action, int iteration = 1)
        {
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < iteration; ++i)
            {
                action.Invoke();
            }
            watch.Stop();

            return watch;
        }

        [UnityTest]
        public IEnumerator MatchmakingTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}