using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SC_Porcupine : Animal, SC_ICleanable
{
    private Coroutine C_DropNeedles;
    private Coroutine C_Gnawing;

    public GameObject P_Needles;

    private float LastGnowingTime;
    /// <summary> Время в течении которого точно не начнет грызть проводку </summary>
    private float TimeBeforeNewGnowing = 10f;
    /// <summary> Время в течении которого точно должен начать грызть проводку </summary>
    private float NewGnowingTime = 30f;

    /// <summary> Показывает, что дикообраз грызет проводку </summary>
    private SC_PointEnergy currentPointEnergy;

    internal new void Start()
    {
        base.Start();

        StartDropNeedles();
        LastGnowingTime = Time.time;
    }

    void SC_ICleanable.Clean()
    {
        if (currentPointEnergy != null)
        {
            StopCoroutine(C_Gnawing);   // Остановить перегрызение проводки
            currentPointEnergy = null;

            StartDropNeedles();     // начнем снова сбрасывать иголки, с горя.
            SelectNewTarget();      // отправим слоняться в другое место. 
        }
    }

    /// <summary> Запуск таймера сбрасывания иголок </summary>
    private void StartDropNeedles()
    {
        C_DropNeedles = StartCoroutine(DropNeedlesFunc(UnityEngine.Random.Range(5, 20)));
    }
    public override bool NeedSelectNewTarget(Transform oldTarget)
    {
        var newEnergyPoint = oldTarget.gameObject.GetComponent<SC_PointEnergy>();
        if (newEnergyPoint != null)
        {
            Debug.Log("Start gnowing");
            StopCoroutine(C_DropNeedles);
            C_Gnawing= StartCoroutine(DestroyEnergyFunc(4));
            LastGnowingTime = Time.time;
            return false;
        }
        else
        {
            if (Level == 1 && UnityEngine.Random.value < Mathf.Min((Time.time - LastGnowingTime - TimeBeforeNewGnowing) / NewGnowingTime, 1.0f))
            {
                newEnergyPoint = FindObjectOfType<SC_PointEnergy>();
                if (newEnergyPoint != null)
                {
                    Debug.Log("Go gnowing");
                    currentPointEnergy = newEnergyPoint;
                    SetTarget(newEnergyPoint.transform);
                    return false;
                }

            }
        }

        return base.NeedSelectNewTarget(oldTarget);
    }

    IEnumerator DropNeedlesFunc(float duration)
    {
        while (true) // stopping condition?
        {
            duration = UnityEngine.Random.Range(10, 50);
            yield return new WaitForSeconds(duration);
            Instantiate(P_Needles, transform.position, transform.rotation);
        }

    }
    IEnumerator DestroyEnergyFunc(float duration)
    {
        while (true) // stopping condition?
        {
            yield return new WaitForSeconds(duration);
            currentPointEnergy.DestroyEnergy();
            StopCoroutine(C_Gnawing);
            StartDropNeedles();
            SelectNewTarget();
        }

    }

}
