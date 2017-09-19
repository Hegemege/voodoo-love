using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleMover : MonoBehaviour
{
    [Header("Target reference or tag name")]
    public Transform Target;
    public string TargetTagName;

    [Header("Speed")]
    public float Speed = 5f;
    public AnimationCurve SpeedOverTime;
    
    ParticleSystem ps;
    ParticleSystem.Particle[] particles;
    int numParticlesAlive;

    float startTime;
    float duration;

    void Start()
    {
        Target = Target != null ? Target : GameObject.FindGameObjectWithTag(TargetTagName).transform;
        
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];

        startTime = Time.time;
        duration = ps.main.duration;
    }

    void Update()
    {
        var normalizedTime = (Time.time - startTime) / duration;
        var step = Speed * SpeedOverTime.Evaluate(normalizedTime) * Time.deltaTime;
        
        numParticlesAlive = ps.GetParticles(particles);
        for (var i = 0; i < numParticlesAlive; i++)
            particles[i].position = Vector3.MoveTowards(particles[i].position, Target.position, step);
        
        ps.SetParticles(particles, numParticlesAlive);
    }
}