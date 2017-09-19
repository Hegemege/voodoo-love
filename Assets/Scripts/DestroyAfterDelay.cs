using System.Collections;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float Seconds;

    void Start()
    {
        StartCoroutine(DestroyAfterSeconds(Seconds));
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}