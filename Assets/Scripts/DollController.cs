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

    public float MinTargetDistance;

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

        Love += GameController.instance.LovePerSecond * Time.deltaTime;

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
        var hits = Physics2D.RaycastAll(touchCoords, Vector2.zero);

        foreach (var hit in hits)
        {
            switch (touchPhase)
            {
                case TouchPhase.Began:
                    //sr.color = Color.red;
                    // Test which target is being hit, if any
                    if (hit.collider != null)
                    {
                        if (!hit.collider.CompareTag("Doll") && !hit.collider.CompareTag("DollHead") && !hit.collider.CompareTag("DollTarget")) break;

                        if (hit.collider.CompareTag("DollTarget"))
                        {
                            Love += hit.collider.transform.parent.parent.GetComponent<DollTargetController>().Tap(GameController.instance.LovePerTap);
                        }
                        else
                        {
                            // Tap anywhere to gain some love
                            Love += GameController.instance.LovePerTap * GameController.instance.TapHealthyFactor;
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    if (hit.collider != null)
                    {
                        if (!hit.collider.CompareTag("Doll") && !hit.collider.CompareTag("DollHead") && !hit.collider.CompareTag("DollTarget")) break;

                        if (hit.collider.CompareTag("DollTarget"))
                        {
                            Love += hit.collider.transform.parent.parent.GetComponent<DollTargetController>().Drag();
                        }
                        else
                        {
                            // Tap anywhere to gain some love
                            Love += GameController.instance.LovePerSecond * Time.deltaTime * GameController.instance.TapHealthyFactor;
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    if (hit.collider != null)
                    {
                        if (!hit.collider.CompareTag("Doll") && !hit.collider.CompareTag("DollHead") && !hit.collider.CompareTag("DollTarget")) break;

                        if (hit.collider.CompareTag("DollTarget"))
                        {
                            Love += hit.collider.transform.parent.parent.GetComponent<DollTargetController>().Release();
                        }
                    }
                    break;
            }
        }
    }

    private void GenerateDollTargets()
    {
        var generatedTargets = 0;

        var failsafeTries = 0;
        var failsafeTriesMax = 50;

        while (generatedTargets < DollTargetCount && failsafeTries < failsafeTriesMax)
        {
            var bounds = sr.bounds;
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var ren in renderers)
            {
                if (ren.CompareTag("Doll") || ren.CompareTag("DollHead"))
                {
                    bounds.Encapsulate(ren.bounds);
                }
            }

            var randomCoords = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));

            var hit = Physics2D.Raycast(randomCoords, Vector2.zero);

            // Test random coords against all current targets, if too close, discard
            var tooClose = false;
            foreach (var target in dollTargets)
            {
                if (Vector2.Distance(new Vector2(target.transform.position.x, 
                    target.transform.position.y), randomCoords) < MinTargetDistance)
                {
                    tooClose = true;
                }
            }

            if (!hit.collider || (!hit.collider.CompareTag("Doll") && !hit.collider.CompareTag("DollHead")) || tooClose)
            {
                failsafeTries += 1;
                continue;
            }

            var newTarget = Instantiate(DollTargetPrefab);
            newTarget.transform.parent = DollTargetContainer.transform;
            newTarget.transform.position = new Vector3(randomCoords.x, randomCoords.y, transform.position.z - 0.05f);

            var targetController = newTarget.GetComponent<DollTargetController>();
            // TODO; proper values
            targetController.Type = DollTargetType.Wound;

            List<DollTargetType> choices;

            if (hit.collider.CompareTag("DollHead"))
            {
                choices = new List<DollTargetType>() {DollTargetType.Crack, DollTargetType.Dirt};
            }
            else
            {
                choices = new List<DollTargetType>() { DollTargetType.Wound, DollTargetType.Pin, DollTargetType.Dirt };
            }

            var randomType = choices[Random.Range(0, choices.Count)];
            targetController.Type = randomType;

            targetController.Initialize();

            dollTargets.Add(targetController);
            generatedTargets += 1;
            failsafeTries = 0;
         
        }
    }
}
