using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DollTargetType
{
    Wound,
    Pin,
    Dirt
}

public class DollTargetController : MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public DollTargetType Type;

    public List<Sprite> WoundSprites;
    public List<Sprite> PinSprites;
    public List<Sprite> DirtSprites;

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
            default:
                Debug.LogWarning("Doll Type is not updated in code to reflect correct sprites");
                choices = WoundSprites;
                break;
        }

        var randomSprite = choices[Random.Range(0, choices.Count)];

        sr.sprite = randomSprite;
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
