using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class JobsTest : MonoBehaviour
{
    public bool jobs;
    public bool burst;

    private void Update()
    {
        float time = Time.realtimeSinceStartup;
        if (!jobs)
        {
            float value = 0;

            for (int j = 0; j < 10; j++)
                for (int i = 0; i < math.pow(10, 6); i++)
                    value = math.exp10(math.sqrt(value));
        }
        else
        {
            NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(10, Allocator.Temp);
            for (int i = 0; i < 10; i++)
            {
                if (burst)
                    jobs[i] = new BurstTask().Schedule();
                else
                    jobs[i] = new NormalTask().Schedule();
            }

            JobHandle.CompleteAll(jobs);
        }

        Debug.Log(Time.realtimeSinceStartup - time);
    }
}

[BurstCompile]
public struct BurstTask : IJob
{
    public void Execute()
    {
        float value = 0;
        for (int i = 0; i < math.pow(10, 6); i++)
            value = math.exp10(math.sqrt(value));
    }
}

public struct NormalTask : IJob
{
    public void Execute()
    {
        float value = 0;
        for (int i = 0; i < math.pow(10, 6); i++)
            value = math.exp10(math.sqrt(value));
    }
}