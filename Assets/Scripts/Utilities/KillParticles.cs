using UnityEngine;

public class KillParticles : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!_particleSystem.IsAlive(true) && !_particleSystem.isEmitting)
            Destroy(gameObject);
    }
}
