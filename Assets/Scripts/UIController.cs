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

    public GameObject AudioPlayerPrefab;
    public AudioClip PurchaseClip;

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

        MagnetPriceText.text = TextHelper.FormatText(GameController.instance.MagnetPurchaseCost);
        MagnetAmountText.text = GameController.instance.MagnetAmount.ToString();

        FeatherPriceText.text = TextHelper.FormatText(GameController.instance.FeatherPurchaseCost);
        FeatherAmountText.text = GameController.instance.FeatherAmount.ToString();

        WoundPriceText.text = TextHelper.FormatText(GameController.instance.WoundPurchaseCost);
        WoundAmountText.text = GameController.instance.WoundAmount.ToString();
    }

    public void OnWoundClick()
    {
        if (GameController.instance.WoundUpgradeable)
        {
            GameController.instance.WoundUpgraded();
            PlayPurchase();
        }
    }

    public void OnMagnetClick()
    {
        if (GameController.instance.MagnetUpgradeable)
        {
            GameController.instance.MagnetUpgraded();
            PlayPurchase();
        }
    }

    public void OnFeatherClick()
    {
        if (GameController.instance.FeatherUpgradeable)
        {
            GameController.instance.FeatherUpgraded();
            PlayPurchase();
        }
    }

    private void PlayPurchase()
    {
        var soundObject = Instantiate(AudioPlayerPrefab).GetComponent<AudioSource>();
        soundObject.clip = PurchaseClip;
        soundObject.Play();

        Destroy(soundObject.gameObject, 2f);
    }
}
