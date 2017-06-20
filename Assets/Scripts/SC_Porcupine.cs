using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Porcupine : Animal ,SC_ICleanable
{
    /// <summary> Показывает, что дикообраз грызет проводку </summary>
    public bool IsGnawing;

    private Coroutine C_DropNeedles;
    private Coroutine C_Gnawing;

    public GameObject P_Needles;

    internal new void Start()
    {
        base.Start();

        StartDropNeedles();
    }

    void SC_ICleanable.Clean()
    {
        if (IsGnawing)
        {
            StopCoroutine(C_Gnawing);   // Остановить перегрызение проводки
            IsGnawing = false;

            StartDropNeedles();     // начнем снова сбрасывать иголки, с горя.
            SelectNewTarget();      // отправим слоняться в другое место. 
        }
    }

    /// <summary> Запуск таймера сбрасывания иголок </summary>
    private void StartDropNeedles()
    {
        StartCoroutine(TimerUpdate(UnityEngine.Random.Range(10, 50))); 
    }

    System.Collections.IEnumerator TimerUpdate(float duration)
    {
        while (true) // stopping condition?
        {
            duration = UnityEngine.Random.Range(10, 50);
            yield return new WaitForSeconds(duration);
            Instantiate(P_Needles, transform.position, transform.rotation);
        }
    
    }

}
