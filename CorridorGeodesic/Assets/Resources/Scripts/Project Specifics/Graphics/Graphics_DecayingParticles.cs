using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// /*
public class Graphics_DecayingParticles : MonoBehaviour
{
    public float lifetime = 2f;
    private float currentLife;
    public Vector3 startingLocalScale;

    public float LifeFactor => currentLife / lifetime;

    private void Awake()
    {
        currentLife = lifetime;
        startingLocalScale = transform.localScale;
    }

    private void Update()
    {
        currentLife -= Time.deltaTime;

        transform.localScale = startingLocalScale * Mathf.Lerp(0f, 1f, LifeFactor * 1.5f);

        if (currentLife <= 0)
            Destroy(gameObject);
    }
}
// */
 /*
public class Graphics_DecayingParticles : MonoBehaviour
{
    public float lifetime = 2f;
    private float currentLife;

    public float LifeFactor => currentLife / lifetime;

    private void Awake()
    {
        currentLife = lifetime;
    }

    private void Update()
    {
        currentLife -= Time.deltaTime;

        transform.localScale = Vector3.one * Mathf.Lerp(LifeFactor, 1f, 0f);

        if (lifetime < 0)
            Destroy(gameObject);
    }
}

// */