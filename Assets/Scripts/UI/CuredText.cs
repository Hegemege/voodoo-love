using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CuredText : MonoBehaviour
{
    Text curedText;

    void Awake()
    {
        curedText = GetComponent<Text>();
    }

    void Update()
    {
        curedText.text = GameController.instance.CuredDolls.ToString();
    }
}