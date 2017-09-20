using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    [HideInInspector]
    public static GameController instance = null; // Singleton

    public int CuredDolls;
    public int Level;
    public float Mana;
    public float ManaPerSecond;

    public float LovePerSecond;
    public float LovePerTap;

    public float TapHealthyFactor;

    public GameObject AudioPlayerPrefab;
    public AudioClip DollFinishedClip;
    public AudioClip NewDollClip;

    public GameObject DollPrefab;

    public bool WoundUpgradeable;
    public bool MagnetUpgradeable;
    public bool FeatherUpgradeable;

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
        
        Load();
    }

    void Start() 
    {

    }
    
    void FixedUpdate()
    {
        if (!CurrentDoll || CurrentDoll.Dead) return;


    }

    void OnDestroy()
    {
        Save();
    }

    void Load()
    {
        CuredDolls = PlayerPrefs.GetInt("CuredDolls", 0);
        Level = PlayerPrefs.GetInt("Level", 1);
        Mana = PlayerPrefs.GetFloat("Mana", 0);
        ManaPerSecond = PlayerPrefs.GetFloat("ManaPerSecond", 0);
        LovePerTap = PlayerPrefs.GetFloat("LovePerTap", 1);
        LovePerSecond = PlayerPrefs.GetFloat("LovePerSecond", 0);
    }

    void Save()
    {
        PlayerPrefs.SetInt("CuredDolls", 0);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetFloat("Mana", 0);
        PlayerPrefs.SetFloat("ManaPerSecond", 0);
        PlayerPrefs.SetFloat("LovePerTap", 1);
        PlayerPrefs.SetFloat("LovePerSecond", 0);
    }

    public void DollFinished()
    {
        NewDoll();

        // Spawn audio effect
        var soundEffect = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
        soundEffect.clip = DollFinishedClip;
        soundEffect.Play();
        Destroy(soundEffect.gameObject, 3f);
    }

    private void NewDoll()
    {
        // Destroy old
        if (CurrentDoll != null)
        {
            // Move doll away and destroy
            CurrentDoll.MoveAway = true;
            Destroy(CurrentDoll.gameObject, 5f);
        }

        var newDoll = Instantiate(DollPrefab);
        newDoll.transform.position = new Vector3(15f, 0f, 0f);
        CurrentDoll = newDoll.GetComponent<DollController>();
        CurrentDoll.MoveIn = true;

        // Spawn audio effect
        var soundEffect = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
        soundEffect.clip = NewDollClip;
        soundEffect.Play();
        Destroy(soundEffect.gameObject, 3f);
    }

    public void DollFailed()
    {
        
    }
}
