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

    public AudioClip WoundDamageClip;
    public AudioClip WoundHealClip;
    public AudioClip CrackDamageClip;
    public AudioClip CrackHealClip;
    public AudioClip PinPullingClip;
    public AudioClip PinOutClip;
    public AudioClip DirtClip;

    public GameObject AudioPlayerPrefab;

    public GameObject DirtParticleEffectPrefab;
    public GameObject PinParticleEffectPrefab;
    public GameObject WoundParticleEffectPrefab;
    public GameObject CrackParticleEffectPrefab;

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
            if (!Healed && Type == DollTargetType.Pin)
            {
                var audioSource = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
                audioSource.clip = PinOutClip;
                audioSource.loop = false;
                audioSource.Play();
                Destroy(audioSource.gameObject, 2f);
            }

            if (!Healed && Type == DollTargetType.Wound)
            {
                var audioSource = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
                audioSource.clip = WoundHealClip;
                audioSource.loop = false;
                audioSource.Play();
                Destroy(audioSource.gameObject, 2f);
            }

            if (!Healed && Type == DollTargetType.Crack)
            {
                var audioSource = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
                audioSource.clip = CrackHealClip;
                audioSource.loop = false;
                audioSource.Play();
                Destroy(audioSource.gameObject, 2f);
            }

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

    public float Tap()
    {
        heldDown = true;
        heldDownTimer = 0f;
        var damage = 0f;

        switch (Type)
        {
            case DollTargetType.Crack:
                damage = GameController.instance.WoundDamage;
                break;
            case DollTargetType.Dirt:
                damage = GameController.instance.FeatherDamage;
                break;
            case DollTargetType.Pin:
                damage = GameController.instance.MagnetAmount;
                break;
            case DollTargetType.Wound:
                damage = GameController.instance.WoundDamage;
                break;
        }

        takenDPS = damage;

        AudioSource audioSource;

        switch (Type)
        {
            case DollTargetType.Crack:
                audioSource = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
                audioSource.clip = CrackDamageClip;
                audioSource.loop = false;
                audioSource.Play();
                Destroy(audioSource.gameObject, 2f);
                break;
            case DollTargetType.Dirt:
                audioSource = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
                audioSource.clip = DirtClip;
                audioSource.loop = false;
                audioSource.Play();
                Destroy(audioSource.gameObject, 2f);
                break;
            case DollTargetType.Pin:
                audioSource = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
                audioSource.clip = PinPullingClip;
                audioSource.loop = false;
                audioSource.Play();
                Destroy(audioSource.gameObject, 2f);
                break;
            case DollTargetType.Wound:
                audioSource = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
                audioSource.clip = WoundDamageClip;
                audioSource.loop = false;
                audioSource.Play();
                Destroy(audioSource.gameObject, 2f);
                break;
        }

        TapEffects();

        Health += damage;
        return damage;
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

        return 0f;
    }

    private void TapEffects()
    {
        if (PinParticleEffectPrefab != null && DirtParticleEffectPrefab != null && WoundParticleEffectPrefab != null && CrackParticleEffectPrefab != null)
        {
            GameObject effects;
            if (Type == DollTargetType.Dirt)
            {
                effects = Instantiate(DirtParticleEffectPrefab);
            }
            else if (Type == DollTargetType.Crack)
            {
                effects = Instantiate(CrackParticleEffectPrefab);
            }
            else if (Type == DollTargetType.Pin)
            {
                effects = Instantiate(PinParticleEffectPrefab);
            }
            else
            {
                effects = Instantiate(WoundParticleEffectPrefab);
            }

            effects.transform.position = transform.position;
        }
    }
}
