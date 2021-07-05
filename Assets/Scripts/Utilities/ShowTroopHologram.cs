using UnityEngine;

public class ShowTroopHologram : MonoBehaviour
{
    [SerializeField] private Mesh troopMesh;
    [SerializeField] private Color highlightColor;
    private void OnDrawGizmos()
    {
        foreach (Transform child in transform)
        {
            Gizmos.color = highlightColor;
            Gizmos.DrawMesh(troopMesh, child.position, child.rotation, Vector3.one);
        }
    }
}