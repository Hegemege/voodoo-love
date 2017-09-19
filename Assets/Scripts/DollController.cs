using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollController : MonoBehaviour
{
    public int Level;
    public float Love;

    [HideInInspector]
    public bool Dead;

    void Awake()
    {
        // On spawn, tell game manager that this is the current active doll
        GameController.instance.CurrentDoll = this;
    }

    void Start() 
    {
        
    }
    
    void Update() 
    {
        
    }
}
