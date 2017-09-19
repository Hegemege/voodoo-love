using System;
using System.Collections;
using UnityEngine;

public class ExtendedMonoBehavior : MonoBehaviour
{
    public void DelayedAction(float seconds, Action action)
    {
        StartCoroutine(RunDelayedAction(seconds, action));
    }

    static IEnumerator RunDelayedAction(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        if (action != null) action();
    }
}