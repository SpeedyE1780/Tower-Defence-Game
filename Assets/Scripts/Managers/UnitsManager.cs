using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public class UnitsManager : Singleton<UnitsManager>
{
    [SerializeField] private int initialCapacity;
    [SerializeField] private int batchSize;
    [SerializeField] private float maxCastDistance;
    [SerializeField] private float refreshRate;
    private NativeList<SpherecastCommand> commands;
    private NativeList<RaycastHit> hits;
    private float currentReferesh;

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

    public void RemoveCommandFromList(int unitIndex)
    {
        commands.RemoveAt(unitIndex);
        hits.RemoveAt(unitIndex);
        EventManager.RaiseUnitIndex(unitIndex);
    }

    private void Update()
    {
        if (currentReferesh < 0)
        {
            SpherecastCommand.ScheduleBatch(commands, hits, batchSize).Complete();
            currentReferesh = refreshRate;
        }

        currentReferesh -= Time.deltaTime;
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