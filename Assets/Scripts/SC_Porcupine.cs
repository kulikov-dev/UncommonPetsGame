using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SC_Porcupine : Animal
{

    public GameObject P_Needles;

    private Timer dropNeedlesTimer;

    internal new void Start()
    {
        base.Start();

        StartCoroutine(TimerUpdate(Random.Range(10, 50))); // will be called once every 2 seconds

    }

    System.Collections.IEnumerator TimerUpdate(float duration)
    {
        while (true) // stopping condition?
        {
        
            yield return new WaitForSeconds(duration);
            duration = Random.Range(10,50);
            Instantiate(P_Needles, transform.position, transform.rotation);
        }
    
    }

}
