using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : Animal {
    public float NewAnimalTime = 5.0f;
    public float TimeBeforeNewAnimal = 3.0f;
    private float LastAnimalTime;

    // Use this for initialization
    internal new void Start()
    {
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

        }
        else
        {
            if (Level == 0 && Random.value < Mathf.Min((Time.time - LastAnimalTime - TimeBeforeNewAnimal) / NewAnimalTime, 1.0f))
            {
                var newAnimalTarget = FindObjectOfType<NewAnimalPoint>();
                if(newAnimalTarget != null)
                {
                    SetTarget(newAnimalTarget.transform);
                    return false;
                }
            }
        }

        return base.NeedSelectNewTarget(oldTarget);
    }
}
