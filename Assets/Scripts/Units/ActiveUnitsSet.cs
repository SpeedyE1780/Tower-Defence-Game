using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/Set/Active")]
public class ActiveUnitsSet : ScriptableObject
{
    private Dictionary<int, HealthController> activeUnits;

    public HealthController this[int index] => activeUnits[index];
    public void Initialize(int capacity) => activeUnits = new Dictionary<int, HealthController>(capacity);
    public void Add(int ID, HealthController controller) => activeUnits.Add(ID, controller);
    public void Remove(int ID) => activeUnits.Remove(ID);

    public HealthController TryGetValue(int ID)
    {
        activeUnits.TryGetValue(ID, out HealthController controller);
        return controller;
    }
}