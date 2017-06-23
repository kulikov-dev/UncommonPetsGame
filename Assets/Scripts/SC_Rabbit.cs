using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Rabbit : Animal
{
    public float DelayBetweenMurders = 5.0f;

    private DayNightController DayNightControllerInst;

    private SC_Bubble bubble;

    /*NEW*/
    private bool IsKilling = false;
    /*NEW*/

    // Use this for initialization
    internal new void Start()
    {
        base.Start();

        DayNightControllerInst = FindObjectOfType<DayNightController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsKilling && DayNightControllerInst.IsFullDarkness())
        {
            var animal = other.gameObject.GetComponent<Animal>();
            if (animal != null)
            {
                if (!(animal is Girl))
                {
                    animal.Kill();
                    FeedCreature();
                    IsKilling = true;
                    StopMoving();
                    StartCoroutine(Relaxation(DelayBetweenMurders));
                }
            }
        }
    }

    IEnumerator Relaxation(float duration)
    {
        yield return new WaitForSeconds(duration);
        IsKilling = false;
        StartMoving();
    }

    public override void OnDeath()
    {
        StopAllCoroutines();
        base.OnDeath();
    }
}
