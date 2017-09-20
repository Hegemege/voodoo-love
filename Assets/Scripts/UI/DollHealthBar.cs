using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DollHealthBar : MonoBehaviour
{
    public DollController Doll;
    
    Slider slider;
    Animator anim;

    void Awake()
    {
        slider = GetComponent<Slider>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        slider.maxValue = Doll.MaxLove;
        slider.value = Doll.Love;
    }
    
    void OnParticleCollision(GameObject other)
    {
        Destroy(other);
        anim.SetTrigger("Hit");
    }
}