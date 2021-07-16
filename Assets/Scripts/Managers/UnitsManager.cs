using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public class UnitsManager : Singleton<UnitsManager>
{
    [SerializeField] private int initialCapacity;
    [SerializeField] private float maxCastDistance;
    private NativeList<SpherecastCommand> commands;
    private NativeList<RaycastHit> hits;

    protected override void Awake()
    {
        base.Awake();
        commands = new NativeList<SpherecastCommand>(initialCapacity, Allocator.Persistent);
        hits = new NativeList<RaycastHit>(initialCapacity, Allocator.Persistent);
    }

    public int AddCommandToList(Vector3 origin, float raduis, Vector3 direction, LayerMask unitsLayer)
    {
        int unitIndex = commands.Length;
        commands.Add(new SpherecastCommand(origin - direction * maxCastDistance, raduis, direction, maxCastDistance, unitsLayer));
        hits.Add(new RaycastHit());
        return unitIndex;
    }

    private void Update()
    {
        SpherecastCommand.ScheduleBatch(commands, hits, 25).Complete();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateCommand(int unitIndex, Vector3 unitPosition, Vector3 direction)
    {
        SpherecastCommand c = commands[unitIndex];
        c.origin = unitPosition - direction * maxCastDistance;
        c.direction = direction;
        commands[unitIndex] = c;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HealthController GetTarget(int unitIndex)
    {
        if (hits[unitIndex].transform != null)
            return hits[unitIndex].transform.GetComponent<HealthController>();
        return null;
    }

    private void OnDestroy()
    {
        commands.Dispose();
        hits.Dispose();
    }
}