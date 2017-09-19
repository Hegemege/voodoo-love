using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollController : MonoBehaviour
{
    public int Level;
    public float Love;
    public float MaxLove;

    [HideInInspector]
    public bool Dead;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        Love = Random.Range(0.25f, 0.75f) * MaxLove;
    }

    void Start() 
    {
        // On spawn, tell game manager that this is the current active doll
        GameController.instance.CurrentDoll = this;
    }
    
    void Update() 
    {
        // Handle native touch events
        foreach (Touch touch in Input.touches)
        {
            HandleTouch(touch.fingerId, Camera.main.ScreenToWorldPoint(touch.position), touch.phase);
        }

        // Simulate touch events from mouse events
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began);
            }
            if (Input.GetMouseButton(0))
            {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Moved);
            }
            if (Input.GetMouseButtonUp(0))
            {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended);
            }
        }
    }

    void FixedUpdate()
    {

    }

    private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
    {
        switch (touchPhase)
        {
            case TouchPhase.Began:
                sr.color = Color.red;
                break;
            case TouchPhase.Moved:
                sr.color = Color.yellow;
                break;
            case TouchPhase.Ended:
                sr.color = Color.green;
                break;
        }
    }
}
