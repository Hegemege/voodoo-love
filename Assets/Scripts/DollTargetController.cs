using System.Collections;
using System.Collections.Generic;
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
    }

    public void Tap()
    {
        heldDown = true;
    }

    public void Drag()
    {
        
    }

    public void Release()
    {
        heldDown = false;
    }
}
