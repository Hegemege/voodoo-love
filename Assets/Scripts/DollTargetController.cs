using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public enum DollTargetType
{
    Wound,
    Pin,
    Dirt,
    Crack
}

public class DollTargetController : MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public DollTargetType Type;

    public List<Sprite> WoundSprites;
    public List<Sprite> PinSprites;
    public List<Sprite> DirtSprites;
    public List<Sprite> CrackSprites;

    public GameObject ParticleEffectPrefab;
    private ParticleSystem activateParticleEffects;

    [Space(20)]
    public bool Healed;

    private SpriteRenderer sr;
    private bool heldDown;

    private float takenDPS;
    private float heldDownTimer;

    private Vector3 originalPos;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        if (ParticleEffectPrefab == null) return;

        var effects = Instantiate(ParticleEffectPrefab);
        effects.transform.parent = transform;

        activateParticleEffects = effects.GetComponentInChildren<ParticleSystem>();

    }

    void Start() 
    {
        originalPos = transform.position;
    }
    
    void Update() 
    {
        
    }

    void FixedUpdate()
    {
        if (heldDown)
        {
            heldDownTimer += Time.fixedDeltaTime;
            Health += Time.fixedDeltaTime * takenDPS;

            if (!Healed && Type == DollTargetType.Pin)
            {
                var distance = Vector3.Distance(transform.position, originalPos);

                var speed = takenDPS / MaxHealth;

                if (distance < 1f)
                {
                    transform.Translate(Quaternion.AngleAxis(transform.localRotation.z, new Vector3(1f, 0f, 0f)) * (new Vector3(1f, 0, 0f) * Time.fixedDeltaTime) * speed);
                }
            }
        }

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
            Healed = true;

            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        // Set the correct sprite
        List<Sprite> choices;
        switch (Type)
        {
            case DollTargetType.Wound:
                choices = WoundSprites;
                break;
            case DollTargetType.Pin:
                choices = PinSprites;
                break;
            case DollTargetType.Dirt:
                choices = DirtSprites;
                break;
            case DollTargetType.Crack:
                choices = CrackSprites;
                break;
            default:
                Debug.LogWarning("Doll Type is not updated in code to reflect correct sprites");
                choices = WoundSprites;
                break;
        }

        var randomSprite = choices[Random.Range(0, choices.Count)];

        sr.sprite = randomSprite;
        transform.localRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f); // TODO: remove?

        sr.gameObject.AddComponent<BoxCollider2D>();
    }

    public float Tap(float damage)
    {
        heldDown = true;
        heldDownTimer = 0f;
        takenDPS = damage;

        if (Type == DollTargetType.Crack || Type == DollTargetType.Wound)
        {
            Health += damage;
            return damage;
        }

        if (activateParticleEffects != null)
        {
            activateParticleEffects.Play();
        }

        return 0f;
    }

    public float Drag()
    {
        return 0f;
    }

    public float Release()
    {
        heldDown = false;

        var toReturn = heldDownTimer * takenDPS;
        heldDownTimer = 0f;

        if (Type == DollTargetType.Crack || Type == DollTargetType.Wound) return 0f;

        if (activateParticleEffects != null)
        {
            activateParticleEffects.Stop();
        }

        return toReturn;
    }
}
