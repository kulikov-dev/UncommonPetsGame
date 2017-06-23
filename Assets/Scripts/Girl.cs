using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : Animal {
    public float NewAnimalTime = 5.0f;
    public float TimeBeforeNewAnimal = 3.0f;

    /*NEW*/
    //Пора ли девочке уходить совсем
    public bool NeedToLeaveHouse = false;
    //Ушла ли девочка из дома совсем или нет
    public bool IsLeftHouse = false;
    //Уровень рассудка при котором надо это показать игроку
    public float AlertReasonLevel = 30.0f;
    private SC_Bubble ReasonBubble;
    private float ReasonLevel = 100.0f;    
    /*NEW*/

    private float LastAnimalTime;
    private bool TryFindNewAnimals = true;

    /*NEW*/
    public override void OnDeath()
    {
        ReasonBubble.Hide();
        base.OnDeath();
        //Game over
    }

    public void ReduceReasonLevel(float value)
    {
        if(ReasonLevel > 0.0f)
        {
            ReasonLevel -= value;
            if (ReasonLevel < AlertReasonLevel && !ReasonBubble.IsVisible())
            {
                ReasonBubble.Show();
            }
            if (ReasonLevel < 0.0f)
            {
                ReasonLevel = 0.0f;
                //Game over
            }
        }        
    }
    /*NEW*/

    // Use this for initialization
    internal new void Start()
    {
        /*NEW*/
        ReasonBubble = gameObject.GetComponentInChildren<SC_Bubble>();
        /*NEW*/
        LastAnimalTime = Time.time - NewAnimalTime;
        base.Start();
    }

    // Update is called once per frame
    internal new void Update()
    {
        base.Update();
    }

    public override bool NeedSelectNewTarget(Transform oldTarget)
    {
        var newAnimapPoint = oldTarget.gameObject.GetComponent<NewAnimalPoint>();
        if(newAnimapPoint != null)
        {
            /*NEW*/
            //Покинули дом и никуда больше не идем, отмечаем протоганисту, что можно мочить всех вокруг
            if (NeedToLeaveHouse)
            {
                IsLeftHouse = true;
                var protagonist = FindObjectOfType<SC_Protagonist>();
                protagonist.SetGunActive(true);
                return false;
            }
            /*NEW*/
            SelectNewTarget();
            newAnimapPoint.SpawnAnimal();
            LastAnimalTime = Time.time;
        }
        else
        {
            /*CHANGED*/
            if (Level == 0)
            {
                if(NeedToLeaveHouse)
                {
                    var newAnimalTarget = FindObjectOfType<NewAnimalPoint>();
                    if (newAnimalTarget != null)
                    {
                        SetTarget(newAnimalTarget.transform);
                        return false;
                    }
                }
                if (TryFindNewAnimals && Random.value < Mathf.Min((Time.time - LastAnimalTime - TimeBeforeNewAnimal) / NewAnimalTime, 1.0f))
                {
                    var newAnimalTarget = FindObjectOfType<NewAnimalPoint>();
                    if (newAnimalTarget != null && newAnimalTarget.CanSpawnAnimal())
                    {
                        SetTarget(newAnimalTarget.transform);
                        return false;
                    }
                    else
                    {
                        TryFindNewAnimals = false;
                    }
                }
            }
            /*CHANGED*/
        }

        return base.NeedSelectNewTarget(oldTarget);
    }
}
