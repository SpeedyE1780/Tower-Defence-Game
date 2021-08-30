using UnityEngine;

public class ShowTroopHologram : MonoBehaviour
{
    [SerializeField] private Mesh troopMesh;
    [SerializeField] private Color highlightColor;

    public void ShowHologram(Vector3 position, Quaternion rotation)
    {
        Gizmos.color = highlightColor;
        Gizmos.DrawMesh(troopMesh, position, rotation, Vector3.one);
    }
}