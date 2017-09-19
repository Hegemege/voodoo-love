using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DollHealthBar : MonoBehaviour
{
    public DollController Doll;
    
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        slider.maxValue = Doll.MaxLove;
    }

    void Update()
    {
        slider.value = Doll.Love;
    }
    
    void OnParticleCollision(GameObject other)
    {
        
    }
}