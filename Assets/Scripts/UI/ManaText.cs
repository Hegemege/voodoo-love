using UnityEngine;
using UnityEngine.UI;

public static class TextHelper
{
    public static string FormatText(float mana)
    {
        if (mana < 1000) return mana.ToString("F2");
        if (mana < 1000000) return (mana / 1000).ToString("F2") + " K";
        if (mana < 1000000000) return (mana / 1000000).ToString("F2") + " M";
        return (mana / 1000000000).ToString("F2") + " B";
    }
}

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
        manaText.text = TextHelper.FormatText(GameController.instance.Mana);
    }
}