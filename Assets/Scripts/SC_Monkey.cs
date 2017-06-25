using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> Класс обезъяны </summary>
public class SC_Monkey : Animal, ICleanable, ITouchable
{

    /// <summary> Указатель на предмет, который находится у обезьяны в руках.  </summary>
    public SC_BaseMonkeyItem ItemInHand = null;
    public Transform MonkeyHandTransform;

    private SC_Bubble bubble;

    private float LastStealingTime;
    /// <summary> Время в течении которого точно не начнет таскать  предметы </summary>
    private float TimeBeforeNewStealing = 5f;
    /// <summary> Время в течении которого точно должен начать таскать предметы </summary>
    private float NewStealingTime = 25f;

    /*NEW*/
    /// <summary> Сдвиг с которым будут бросаться предметы </summary>
    public Vector3 DropOffset;
    /// <summary> Урон, который наносит обезьяна с оружием </summary>
    public float Damage = 10.0f;

    private Animator MonkeyAnimator;
    /*NEW*/
    private float defaultHungerPerSecond;
    public bool GoToStealItem = false;


    internal new void Start()
    {
        base.Start();

        defaultHungerPerSecond = HungerPerSecond;

        MonkeyAnimator = GetComponent<Animator>();
        bubble = gameObject.GetComponentInChildren<SC_Bubble>();
        LastStealingTime = Time.time;
    }

    /// <summary> Реализация метода помыть -  Заставляет макаку выбрать себе другой target. Это позволит отгонять её от опасных предметов. </summary>
    void ICleanable.Clean()
    {
        if(GoToStealItem)
        {
            GoToStealItem = false;
            SelectNewTarget();
            bubble.Hide();
        }
        else
        {
            (this as ITouchable).Touch();
        }        
    }

    /// <summary> Метод “нажатие мышью” - вызывается, когда на животное ткнули мышью. при наличии предмета, заставляет обезьяну бросить его. </summary>
    void ITouchable.Touch()
    {
        if (ItemInHand != null)
        {
            /*CHANGED*/
            ItemInHand.DropItem(transform.position + DropOffset);
            /*CHANGED*/
            LastStealingTime = Time.time;
            ItemInHand = null;
            HungerPerSecond = defaultHungerPerSecond;
            MonkeyAnimator.SetBool("IsStealing", false);
        }
    }

    /// <summary> Метод “встреча с другим существом”. В случае, если обезьяна встречает другое существо и у нее в руках опасный предмет - запускается мини-игра по защите существа от бросков обезьяны. </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        var animal = other.gameObject.GetComponent<Animal>();
        if (animal != null && ItemInHand != null && ItemInHand is SC_DangerousMonkeyItem && animal.Health != 0)
        {
            Debug.Log("Start fight with MONKEY!");
            /*NEW*/
            animal.GetDamage(Damage);
            /*NEW*/
        }
    }

    public override bool NeedSelectNewTarget(Transform oldTarget)
    {
        var newItemPoint = oldTarget.gameObject.GetComponent<SC_BaseMonkeyItem>();
        if (newItemPoint != null)
        {
            Debug.Log("I'm a super THIEF!");
            ItemInHand = newItemPoint;
            GoToStealItem = false;
            ItemInHand.GetItem(MonkeyHandTransform);
            if (ItemInHand is SC_HouseMonkeyItem && ((SC_HouseMonkeyItem)ItemInHand).ItemType == enum_ToolType.Food)
            {
                Hunger = 0f;
                HungerPerSecond = 0f;
            }

            MonkeyAnimator.SetBool("IsStealing", true);

            bubble.Hide();

            LastStealingTime = Time.time;
            return base.NeedSelectNewTarget(oldTarget);
        }
        else if (ItemInHand == null)
        {
            if (UnityEngine.Random.value < Mathf.Min((Time.time - LastStealingTime - TimeBeforeNewStealing) / NewStealingTime, 1.0f))
            {
                /*CHANGED*/
                var items = FindObjectsOfType<SC_BaseMonkeyItem>().Where(item => item.ItemLevel == Level).ToArray();
                if (items.Length > 0)
                {
                    var item = items[UnityEngine.Random.Range(0, items.Length)];
                    Debug.Log("Go and steal items!");
                    GoToStealItem = true;
                    bubble.Show();
                    SetTarget(item.transform);
                    return false;
                }
                //var random = UnityEngine.Random.Range(0, items.Length - 1);
                //for (var i = random; i < items.Length + random; i++)
                //{
                //    var item = items[i % items.Length];
                //    if (Level == item.ItemLevel)
                //    {
                //        Debug.Log("Go and steal items!");
                //        bubble.Show();
                //        SetTarget(item.transform);
                //        return false;
                //    }
                //}
                /*CHANGED*/
            }
        }

        return base.NeedSelectNewTarget(oldTarget);
    }

    public override void OnDeath()
    {
        bubble.Hide();
        StopAllCoroutines();
        base.OnDeath();
    }
}
