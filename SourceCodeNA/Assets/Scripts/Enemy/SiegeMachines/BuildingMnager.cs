using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMnager : MonoBehaviour
{
    public float buildingHealth = 1000f;
    public float activationDuration = 0.3f;

    public bool isDestroyed;

    [SerializeField] private GameObject buildingTroop;
    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private ParticleSystem[] smokeParticles;

    private float activateTimer;
    private int activatedParticles;

    private EnemyHealthBar healthBar;

    private void Start()
    {
        healthBar = healthCanvas.GetComponent<EnemyHealthBar>();
        healthBar.SetMaxHealth(buildingHealth);
    }
    void Update()
    {
        if (buildingHealth <= 0)
        {
            buildingHealth = 0;
            isDestroyed = true;
            Destroyed();
            Destroy(gameObject, 1f);
            Destroy(buildingTroop, 1f);
        }

        healthBar.SetHealth(buildingHealth);
    }

    void Destroyed()
    {
        if (activatedParticles < smokeParticles.Length)
        {
            activateTimer += Time.deltaTime;

            if (activateTimer >= activationDuration)
            {
                smokeParticles[activatedParticles].Play();
                activatedParticles++;
            }
        }
    }
}
