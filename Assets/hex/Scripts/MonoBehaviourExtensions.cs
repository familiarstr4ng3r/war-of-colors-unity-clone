using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void CallWithDelay(this MonoBehaviour mono, Action call, float delay)
    {
        mono.StartCoroutine(Routine(call, delay));
    }

    private static IEnumerator Routine(Action call, float delay)
    {
        yield return new WaitForSeconds(delay);
        call?.Invoke();
    }
}
