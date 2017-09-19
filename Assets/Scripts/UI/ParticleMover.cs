using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleMover : MonoBehaviour
{
    [Header("Target reference or tag name")]
    public Transform Target;
    public string TargetTagName;
    
    public float Speed = 5f;
    
    ParticleSystem ps;
    ParticleSystem.Particle[] particles;
    int numParticlesAlive;

    void Start()
    {
        Target = Target != null ? Target : GameObject.FindGameObjectWithTag(TargetTagName).transform;
        
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void Update()
    {
        numParticlesAlive = ps.GetParticles(particles);
        
        var step = Speed * Time.deltaTime;
        for (var i = 0; i < numParticlesAlive; i++)
            particles[i].position = Vector3.MoveTowards(particles[i].position, Target.position, step);
        
        ps.SetParticles(particles, numParticlesAlive);
    }
}