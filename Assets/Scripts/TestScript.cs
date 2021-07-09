using UnityEngine;
using UnityEngine.Profiling;

public class TestScript : MonoBehaviour
{
    Collider[] inRange;

    void Start()
    {
        inRange = new Collider[5];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Overlap();
        }
    }

    private void Overlap()
    {
        Profiler.BeginSample("OVerlap");
        for (int i = 0; i < inRange.Length; i++)
            inRange[i] = null;

        Physics.OverlapSphereNonAlloc(transform.position, 10f, inRange);
        Profiler.EndSample();
    }
}
