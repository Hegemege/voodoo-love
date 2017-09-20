using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    [HideInInspector]
    public static GameController instance = null; // Singleton

    public GameObject ManaEffectPrefab;
    public GameObject CuredEffectPrefab;

    public int CuredDolls = 0;
    public int Level = 1;
    public float Mana = 100;
    public float ManaPerSecond;

    public float TapHealthyFactor;

    public GameObject AudioPlayerPrefab;
    public AudioClip DollFinishedClip;
    public AudioClip NewDollClip;

    public GameObject DollPrefab;

    public bool WoundUpgradeable;
    public bool MagnetUpgradeable;
    public bool FeatherUpgradeable;

    public float WoundPurchaseCost;
    public float MagnetPurchaseCost;
    public float FeatherPurchaseCost;

    public int WoundAmount = 1;
    public int MagnetAmount = 1;
    public int FeatherAmount = 1;

    public float WoundDamage;
    public float MagnetDamage;
    public float FeatherDamage;
    public float GeneralDamage;

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

#if (UNITY_EDITOR)
        PlayerPrefs.DeleteAll();
#else
        Load();
#endif


    }

    void Start() 
    {
        CurrentDoll.Initialize();
    }
    
    void FixedUpdate()
    {
        if (!CurrentDoll || CurrentDoll.Dead) return;

        // Updated values
        WoundPurchaseCost = Mathf.Exp(WoundAmount);
        MagnetPurchaseCost = Mathf.Exp(MagnetAmount);
        FeatherPurchaseCost = Mathf.Exp(FeatherAmount);

        ManaPerSecond = CuredDolls * CuredDolls * 5f;

        Mana += ManaPerSecond * Time.fixedDeltaTime;

        WoundUpgradeable = WoundPurchaseCost < Mana;
        MagnetUpgradeable = MagnetPurchaseCost < Mana;
        FeatherUpgradeable = FeatherPurchaseCost < Mana;

        WoundDamage = Mathf.Exp(WoundAmount * 0.75f);
        MagnetDamage = Mathf.Exp(MagnetAmount * 0.75f);
        FeatherDamage = Mathf.Exp(FeatherAmount*0.75f);
        GeneralDamage = Mathf.Exp(0.75f*(FeatherAmount + MagnetAmount + WoundAmount) / 3f);
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

        WoundAmount = PlayerPrefs.GetInt("WoundAmount", 1);
        MagnetAmount = PlayerPrefs.GetInt("MagnetAmount", 1);
        FeatherAmount = PlayerPrefs.GetInt("FeatherAmount", 1);
}

    void Save()
    {
        PlayerPrefs.SetInt("CuredDolls", CuredDolls);
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetFloat("Mana", Mana);

        PlayerPrefs.SetInt("WoundAmount", WoundAmount);
        PlayerPrefs.SetInt("MagnetAmount", MagnetAmount);
        PlayerPrefs.SetInt("FeatherAmount", FeatherAmount);
    }

    public void DollFinished()
    {
        NewDoll();

        // TODO: UPDATE VALUES

        CurrentDoll.Initialize(); // pulls values from gamecontroller

        // Spawn audio effect
        var soundEffect = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
        soundEffect.clip = DollFinishedClip;
        soundEffect.Play();
        
        // Spawn particle effects
        Instantiate(ManaEffectPrefab, transform.position, transform.rotation);
        Instantiate(CuredEffectPrefab, transform.position, transform.rotation);
        
        Destroy(soundEffect.gameObject, 3f);
    }

    private void NewDoll()
    {
        Level += 1;
        CuredDolls += 1;

        Save();

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

    public void MagnetUpgraded()
    {
        Mana -= MagnetPurchaseCost;
        MagnetAmount += 1;
    }

    public void FeatherUpgraded()
    {
        Mana -= FeatherPurchaseCost;
        FeatherAmount += 1;
    }

    public void WoundUpgraded()
    {
        Mana -= WoundPurchaseCost;
        WoundAmount += 1;
    }

    public void DollFailed()
    {
        
    }
}
