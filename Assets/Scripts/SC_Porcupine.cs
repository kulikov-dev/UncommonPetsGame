using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SC_Porcupine : Animal, ICleanable
{
    private Coroutine C_DropNeedles;
    private Coroutine C_Gnawing;
    
    public GameObject P_Needles;

    private bool isGnawing;

    private float LastGnowingTime;
    /// <summary> Время в течении которого точно не начнет грызть проводку </summary>
    private float TimeBeforeNewGnowing = 10f;
    /// <summary> Время в течении которого точно должен начать грызть проводку </summary>
    private float NewGnowingTime = 30f;

    /// <summary> Показывает, что дикообраз грызет проводку </summary>
    private SC_PointEnergy currentPointEnergy;

    private Animator PorcupineAnimator;

    internal new void Start()
    {
        base.Start();

        StartDropNeedles();
        LastGnowingTime = Time.time;
        PorcupineAnimator = GetComponent<Animator>();
    }

    void ICleanable.Clean()
    {
        if (isGnawing)
        {
            StopCoroutine(C_Gnawing);   // Остановить перегрызение проводки
            currentPointEnergy = null;

            StartDropNeedles();     // начнем снова сбрасывать иголки, с горя.
            SelectNewTarget();      // отправим слоняться в другое место. 
            isGnawing = false;
            PorcupineAnimator.SetBool("IsGnowing", false);

            Debug.Log("Go away, porcupine :(");
        }
    }

    /// <summary> Запуск таймера сбрасывания иголок </summary>
    private void StartDropNeedles()
    {
        C_DropNeedles = StartCoroutine(DropNeedlesFunc(UnityEngine.Random.Range(5, 15)));
    }
    public override bool NeedSelectNewTarget(Transform oldTarget)
    {
        var newEnergyPoint = oldTarget.gameObject.GetComponent<SC_PointEnergy>();
        if (newEnergyPoint != null)
        {
            Debug.Log("Start gnowing");
            isGnawing = true;            
            StopCoroutine(C_DropNeedles);
            C_Gnawing = StartCoroutine(DestroyEnergyFunc(4));
            PorcupineAnimator.SetBool("IsGnowing", true);
            LastGnowingTime = Time.time;
            return false;
        }
        else
        {
            newEnergyPoint = FindObjectOfType<SC_PointEnergy>();
            if (Level == 1 && newEnergyPoint != null && !newEnergyPoint.IsLightBroken && UnityEngine.Random.value < Mathf.Min((Time.time - LastGnowingTime - TimeBeforeNewGnowing) / NewGnowingTime, 1.0f))
            {
                Debug.Log("Go gnowing");
                currentPointEnergy = newEnergyPoint;
                SetTarget(newEnergyPoint.transform);
                return false;

            }
        }

        return base.NeedSelectNewTarget(oldTarget);
    }

    IEnumerator DropNeedlesFunc(float duration)
    {
        while (true) // stopping condition?
        {
            Debug.Log("Drop needles");
            duration = UnityEngine.Random.Range(5, 15);
            yield return new WaitForSeconds(duration);
            var needles = Instantiate(P_Needles, transform.position, transform.rotation);
            var script = needles.GetComponent<SC_Needles>();
            script.Parent = this;
        }

    }
    IEnumerator DestroyEnergyFunc(float duration)
    {
        while (true) // stopping condition?
        {
            yield return new WaitForSeconds(duration);
            isGnawing = false;
            PorcupineAnimator.SetBool("IsGnowing", false);
            currentPointEnergy.DestroyEnergy();
            currentPointEnergy = null;
            StopCoroutine(C_Gnawing);
            StartDropNeedles();
            SelectNewTarget();
        }

    }

}
