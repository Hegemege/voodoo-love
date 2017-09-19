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

    public int DollTargetCount;

    public GameObject DollTargetContainer;
    public GameObject DollTargetPrefab;
    

    // [HideInInspector] but easier to debug without
    [Space(20)]
    public bool Dead;
    public bool Finished;

    // Privates
    private SpriteRenderer sr;
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
        CheckAllInput();
        UpdateState();
    }

    void FixedUpdate()
    {

    }

    private void UpdateState()
    {
        // Updates all game state variables (love etc)
        if (Dead || Finished) return;

        Love += GameController.instance.LovePerSecond;

        if (Love > MaxLove)
        {
            Love = MaxLove;
            Finished = true;
            GameController.instance.DollFinished();
        }

        if (Love < 0f)
        {
            Love = 0f;
            Dead = true;
            GameController.instance.DollFailed();
        }
    }

    private void CheckAllInput()
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

    private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
    {
        Vector2 touchCoords = new Vector2(touchPosition.x, touchPosition.y);
        switch (touchPhase)
        {
            case TouchPhase.Began:
                //sr.color = Color.red;
                // Test which target is being hit, if any
                var hit = Physics2D.Raycast(touchCoords, Vector2.zero);
                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("Doll")) break;

                    // TODO: Interaction
                    Love += GameController.instance.LovePerTap;
                }
                break;
            case TouchPhase.Moved:
                //sr.color = Color.yellow;
                break;
            case TouchPhase.Ended:
                //sr.color = Color.green;
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
