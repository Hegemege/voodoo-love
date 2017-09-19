using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DollHealthBar : MonoBehaviour
{
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        slider.maxValue = GameController.instance.CurrentDoll.MaxLove;
    }

    void Update()
    {
        slider.value = GameController.instance.CurrentDoll.Love;
    }
}