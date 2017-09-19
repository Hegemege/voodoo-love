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

    [Space(20)]
    public bool Healed;

    private SpriteRenderer sr;
    private bool heldDown;

    private float takenDPS;
    private float heldDownTimer;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Start() 
    {
        
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
        sr.transform.localRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
        sr.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f); // TODO: remove?

        sr.gameObject.AddComponent<PolygonCollider2D>();
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

        return toReturn;
    }
}
