using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DollTargetType
{
    Wound,
    Pin
}

public class DollTargetController : MonoBehaviour
{
    public float Health;
    public float LoveOnFinish;
    public float LPSIncreaseOnFinish;

    public DollTargetType Type;

    public List<Sprite> WoundSprites;
    public List<Sprite> PinSprites;

    private SpriteRenderer sr;

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
            default:
                Debug.LogWarning("Doll Type is not updated in code to reflect correct sprites");
                choices = WoundSprites;
                break;
        }

        var randomSprite = choices[Random.Range(0, choices.Count)];

        sr.sprite = randomSprite;
    }
}
