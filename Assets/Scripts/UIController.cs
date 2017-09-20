using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject MagnetUp;
    public GameObject WoundUp;
    public GameObject FeatherUp;

    public Text MagnetPriceText;
    public Text MagnetAmountText;
    public Text WoundPriceText;
    public Text WoundAmountText;
    public Text FeatherPriceText;
    public Text FeatherAmountText;

    public Text ManaAmount;
    public Text DollAmount;

    void Awake() 
    {
        
    }

    void Start() 
    {
        
    }
    
    void Update()
    {
        MagnetUp.SetActive(GameController.instance.MagnetUpgradeable);
        WoundUp.SetActive(GameController.instance.WoundUpgradeable);
        FeatherUp.SetActive(GameController.instance.FeatherUpgradeable);
    }

    public void OnWoundClick()
    {
        
    }

    public void OnMagnetClick()
    {
        
    }

    public void OnFeatherClick()
    {
        
    }
}
