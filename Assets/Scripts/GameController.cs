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
        Debug.Log("Doll has been finished");
    }

    public void DollFailed()
    {
        
    }
}
