using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopMovementController : MonoBehaviour
{
    private const string speedParameter = "Speed";
    private const string motionParameter = "MotionSpeed";

    [SerializeField] private float speed;
    [SerializeField] private int maxHits;

    private int hits;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetEnemyController()
    {
        anim.SetFloat(speedParameter, 2);
        anim.SetFloat(motionParameter, 1);
    }
}