using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    NativeArray<RaycastHit> hits;
    NativeArray<SpherecastCommand> commands;

    void Start()
    {
        hits = new NativeArray<RaycastHit>(10, Allocator.Persistent);
        commands = new NativeArray<SpherecastCommand>(10, Allocator.Persistent);
        commands[0] = new SpherecastCommand(transform.position - transform.forward * 10, 5f, transform.forward);
        commands[1] = new SpherecastCommand(transform.position - transform.forward * 10, 5f, transform.forward);
        commands[2] = new SpherecastCommand(transform.position - transform.forward * 10, 5f, transform.forward);
        commands[3] = new SpherecastCommand(transform.position - transform.forward * 10, 5f, transform.forward);
        commands[4] = new SpherecastCommand(transform.position - transform.forward * 10, 5f, transform.forward);
        commands[5] = new SpherecastCommand(transform.position - transform.forward * 10, 5f, transform.forward);
    }

    void Update()
    {
        Physics.Simulate(Time.deltaTime);
        SpherecastCommand.ScheduleBatch(commands, hits, 1).Complete();

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log($"{i}:{hits[i].transform}");
        }
    }

    private void OnDestroy()
    {
        hits.Dispose();
        commands.Dispose();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - transform.forward * 10, 5);
    }
}

[BurstCompile]
public struct FindNearest : IJob
{
    public NativeArray<RaycastHit> hits;
    public int target;

    public void Execute()
    {
        throw new System.NotImplementedException();
    }
}