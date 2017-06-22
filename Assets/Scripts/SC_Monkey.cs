using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Класс обезъяны </summary>
public class SC_Monkey : Animal, ICleanable, ITouchable
{

    /// <summary> Указатель на предмет, который находится у обезьяны в руках.  </summary>
    private SC_BaseMonkeyItem ItemInHand = null;
    public Transform MonkeyHandTransform;

    private SC_Bubble bubble;

    private float LastStealingTime;
    /// <summary> Время в течении которого точно не начнет таскать  предметы </summary>
    private float TimeBeforeNewStealing = 5f;
    /// <summary> Время в течении которого точно должен начать таскать предметы </summary>
    private float NewStealingTime = 25f;

    internal new void Start()
    {
        base.Start();

        bubble = gameObject.GetComponentInChildren<SC_Bubble>();
        LastStealingTime = Time.time;
    }

    /// <summary> Реализация метода помыть -  Заставляет макаку выбрать себе другой target. Это позволит отгонять её от опасных предметов. </summary>
    void ICleanable.Clean()
    {
        SelectNewTarget();

        bubble.Hide();
    }

    /// <summary> Метод “нажатие мышью” - вызывается, когда на животное ткнули мышью. при наличии предмета, заставляет обезьяну бросить его. </summary>
    void ITouchable.Touch()
    {
        if (ItemInHand != null)
        {
            ItemInHand.DropItem();
            LastStealingTime = Time.time;
            ItemInHand = null;
        }
    }

    /// <summary> Метод “встреча с другим существом”. В случае, если обезьяна встречает другое существо и у нее в руках опасный предмет - запускается мини-игра по защите существа от бросков обезьяны. </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        var animal = other.gameObject.GetComponent<Animal>();
        if (animal != null && ItemInHand != null && ItemInHand is SC_DangerousMonkeyItem)
        {
            Debug.Log("Start fight with MONKEY!");
            //TODO сделать запуск мини-игры
        }
    }

    public override bool NeedSelectNewTarget(Transform oldTarget)
    {
        var newItemPoint = oldTarget.gameObject.GetComponent<SC_BaseMonkeyItem>();
        if (newItemPoint != null)
        {
            Debug.Log("I'm a super THIEF!");
            ItemInHand = newItemPoint;
            ItemInHand.GetItem(MonkeyHandTransform);

            bubble.Hide();

            LastStealingTime = Time.time;
            return base.NeedSelectNewTarget(oldTarget);
        }
        else if (ItemInHand == null)
        {
            if (UnityEngine.Random.value < Mathf.Min((Time.time - LastStealingTime - TimeBeforeNewStealing) / NewStealingTime, 1.0f))
            {
                var items = FindObjectsOfType<SC_BaseMonkeyItem>();
                var random = UnityEngine.Random.Range(0, items.Length - 1);
                for (var i = random; i < items.Length + random; i++)
                {
                    var item = items[i % items.Length];
                    if (Level == item.ItemLevel)
                    {
                        Debug.Log("Go and steal items!");
                        bubble.Show();
                        SetTarget(item.transform);
                        return false;
                    }
                }
            }
        }

        return base.NeedSelectNewTarget(oldTarget);
    }
}
