using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DollController : MonoBehaviour
{
    public int Level;
    public float Love;
    public float MaxLove;

    [HideInInspector]
    public bool Dead;

    public int DollTargetCount;

    private SpriteRenderer sr;

    public GameObject DollTargetContainer;
    public GameObject DollTargetPrefab;
    private List<DollTargetController> dollTargets;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        Level = Random.Range(1, 3); // TODO: Calculate level by player progress
        MaxLove = Math.Abs(Level) + 1 * 100f;
        Love = Random.Range(0.25f, 0.75f) * MaxLove;

        dollTargets = new List<DollTargetController>();
        
        GenerateDollTargets();
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

    private void GenerateDollTargets()
    {
        var generatedTargets = 0;

        var failsafeTries = 0;
        var failsafeTriesMax = 20;

        while (generatedTargets < DollTargetCount && failsafeTries < failsafeTriesMax)
        {
            if (true) // TODO: check if the target is within the sprite with raycast or smth
            {
                var newTarget = Instantiate(DollTargetPrefab);
                newTarget.transform.parent = DollTargetContainer.transform;
                newTarget.transform.localPosition = Vector3.zero;

                var targetController = newTarget.GetComponent<DollTargetController>();
                // TODO; proper values
                targetController.Type = DollTargetType.Wound;
                targetController.Initialize();

                dollTargets.Add(targetController);
                generatedTargets += 1;
            }
            else
            {
                failsafeTries += 1;
            }            
        }
    }
}
