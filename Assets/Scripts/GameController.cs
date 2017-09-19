using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    [HideInInspector]
    public static GameController instance = null; // Singleton

    public int Level;
    public float Mana;
    public float ManaPerSecond;

    [HideInInspector]
    public DollController CurrentDoll;

    void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (!instance.Equals(this))
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {

    }
    
    void FixedUpdate()
    {
        if (!CurrentDoll || CurrentDoll.Dead) return;
    }
}
