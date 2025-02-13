using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class HealthController : MonoBehaviour
{
    [Header("Unit Info")]
    [SerializeField] private int maxHealth;
    [SerializeField] private UnitController controller;
    [SerializeField] private List<Behaviour> toggleBehaviours;
    [Header("Visuals")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform geometry;
    [SerializeField] private ParticleSystem healParticle;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private int currentMaxHealth;
    private int health;

    public Vector3 Position => geometry.position;
    public float HealthPercentage => (float)health / currentMaxHealth;
    public bool IsDead => health <= 0;

    #region UNITY MESSAGES

    private void Awake()
    {
        initialScale = geometry.localScale;
        targetScale = Vector3.zero;
        currentMaxHealth = maxHealth;
    }

    private void OnEnable()
    {
        ToggleBehaviourState(true);

        //Set health to max health
        UpdateHealth(currentMaxHealth);
        healthBar.gameObject.SetActive(false);
    }

    #endregion

    #region HEALTH FUNCTIONS

    public void UpdateMaxHealth(float multiplier) => currentMaxHealth = Mathf.RoundToInt(maxHealth * multiplier);

    private void UpdateHealth(int newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, currentMaxHealth);

        //Enable health bar when it's not full
        healthBar.gameObject.SetActive(health != currentMaxHealth);
        healthBar.value = Mathf.Clamp01((float)newHealth / currentMaxHealth);
    }

    public void TakeHit(int damage, bool instantKill = false)
    {
        //Prevents from starting the kill player coroutine twice in the same frame
        if (!gameObject.activeSelf || IsDead)
            return;

        UpdateHealth(instantKill ? 0 : health - damage);

        if (IsDead)
            StartCoroutine(KillPlayer());
    }

    public void Heal(int healAmount)
    {
        if (!gameObject.activeSelf || IsDead)
            return;

        UpdateHealth(health + healAmount);
        healParticle.Play();
    }

    #endregion

    #region UTITLITY

    IEnumerator KillPlayer()
    {
        healthBar.gameObject.SetActive(false);
        ToggleBehaviourState(false);
        yield return StartCoroutine(PlayDeadAnimation());
        controller.PoolUnit();
    }

    IEnumerator PlayDeadAnimation()
    {
        float time = 0;
        float duration = 0.5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            geometry.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            yield return null;
        }
    }

    //Toggle navmesh and unit controller
    private void ToggleBehaviourState(bool state)
    {
        geometry.localScale = initialScale;

        foreach (Behaviour behaviour in toggleBehaviours)
            behaviour.enabled = state;
    }

    #endregion
}
