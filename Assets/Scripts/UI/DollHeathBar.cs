using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DollHeathBar : MonoBehaviour
{
    DollController doll;
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = doll.MaxLove;
    }

    void Update()
    {
        slider.value = doll.Love;
    }
}