using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Sloth : Animal
{

    private float LastSleepTime;
    /// <summary> Время в течении которого точно не пойдет спать </summary>
    private float TimeBeforeNewSleep = 5f;
    /// <summary> Время в течении которого точно должен пойти спать </summary>
    private float NewSleepTime = 15f;

    private SC_Bubble bubble;

    // Use this for initialization
    internal new void Start()
    {
        base.Start();

        bubble = gameObject.GetComponentInChildren<SC_Bubble>();
        LastSleepTime = Time.time;
    }

    public override bool NeedSelectNewTarget(Transform oldTarget)
    {
        var newItemPoint = oldTarget.gameObject.GetComponent<SC_Fun>();
        if (newItemPoint != null)
        {
            bubble.Show();
            // TODO висит на вентиляторе
            Debug.Log(newItemPoint.gameObject.transform.position);

            gameObject.transform.position = newItemPoint.SlothPoint.position;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, -gameObject.transform.localScale.y, gameObject.transform.localScale.z);

            return false;
        }
        else if (Level == 2 && UnityEngine.Random.value < Mathf.Min((Time.time - LastSleepTime - TimeBeforeNewSleep) / NewSleepTime, 1.0f))
        {
            var item = FindObjectOfType<SC_Fun>();
            Debug.Log("go sleep");
            SetTarget(item.transform);
            return false;
        }

        return base.NeedSelectNewTarget(oldTarget);
    }
}
