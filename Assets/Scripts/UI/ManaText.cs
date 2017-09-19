using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ManaText : MonoBehaviour
{
    Text manaText;

    void Awake()
    {
        manaText = GetComponent<Text>();
    }

    void Update()
    {
        manaText.text = FormatMana(GameController.instance.Mana);
    }

    static string FormatMana(float mana)
    {
        if (mana < 1000) return mana.ToString("2F");
        if (mana < 1000000) return (mana / 1000).ToString("F2") + " K";
        if (mana < 1000000000) return (mana / 1000000).ToString("F2") + " M";
        return (mana / 1000000000).ToString("F2") + " B";
    }
}