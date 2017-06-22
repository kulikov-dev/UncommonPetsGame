using Anima2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hippo : Animal, ICleanable
{
    ///<summary>Цвет бегемота, которого надо облить</summary>
    public Color DirtyColor = Color.red;
    ///<summary>Насколько грязный бегемот</summary>
    public float DirtyLevel = 0.0f;
    ///<summary>Насколько сильно марвется в секунду</summary>
    public float DirtyPerSecond = 0.1f;
    ///<summary>Количество урона во время бешенства</summary>
    public float DamageInRange = 20.0f;
    ///<summary>Во сколько увеличится скорость и ускорение при бешенстве</summary>
    public float RangeScaleFactor = 1.5f;
    ///<summary>Насколько наполняется желудок за одно кормление</summary>
    public float StomachFullnessPerFeed = 0.25f;
    ///<summary>Сколько раз пытаться покакать</summary>
    public int MaxPoopingTriesCount = 15;
    ///<summary>Промежутки межу разбросом какашек</summary>
    public float PoopingDuration = 0.2f;

    public GameObject DirtyRoomPrefab;

    /// <summary>Дефолтовая скорость бегемота</summary>
    private float DefaultMaxVecity;
    /// <summary>Дефолтовое ускорение бегемота</summary>
    private float DefaultMaxAcceleration;
    /// <summary>Находится ли в режиме бешенства</summary>
    private bool IsInRageMode = false;
    /// <summary>Гадит ли сейчас бегемот или нет</summary>
    private bool IsPooping = false;
    /// <summary>Наполненность желудка</summary>
    private float StomachFullness = 0.0f;

    private Animator HippoAnimator;

    private AudioSource PoopsSoundSource;
    private SpriteMeshInstance[] HippoSprites;

    public override void FeedCreature()
    {
        base.FeedCreature();
        StomachFullness = Mathf.Min(StomachFullnessPerFeed + StomachFullness, 1.0f);
        Debug.Log("StomachFullness = " + StomachFullness);
    }

    public void Clean()
    {
        DirtyLevel = 0.0f;
        StopRageMode();
    }

    public bool StartRageMode()
    {
        if (IsInRageMode || IsPooping)
            return false;
        IsInRageMode = true;
        HippoAnimator.SetBool("IsInRage", true);
        MaxVelocity = DefaultMaxVecity * RangeScaleFactor;
        MaxAcceleration = DefaultMaxAcceleration * RangeScaleFactor * RangeScaleFactor;
        return true;
    }

    public void StopRageMode()
    {
        IsInRageMode = false;
        HippoAnimator.SetBool("IsInRage", false);
        MaxVelocity = DefaultMaxVecity;
        MaxAcceleration = DefaultMaxAcceleration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInRageMode)
            return;
        var otherAnimal = other.gameObject.GetComponent<Animal>();
        if(otherAnimal != null)
        {
            otherAnimal.GetDamage(DamageInRange);
        }
    }

    private void StartPooping(RoomScript room)
    {
        IsPooping = true;
        HippoAnimator.SetBool("IsPooping", true);
        if (PoopsSoundSource != null)
        {
            PoopsSoundSource.loop = true;
            PoopsSoundSource.Play();
        }
        var dirtyRoom = Instantiate(DirtyRoomPrefab);
        dirtyRoom.transform.parent = room.transform;
        dirtyRoom.transform.position = room.transform.position;
        var dirtyRoomScript = dirtyRoom.GetComponent<DirtyRoom>();
        if (dirtyRoomScript != null)
        {
            dirtyRoomScript.Initialize();
            StartCoroutine(PoopingFunc(dirtyRoomScript));
        }
    }

    IEnumerator PoopingFunc(DirtyRoom dirtyRoom)
    {        
        for (int i = 0; i < MaxPoopingTriesCount; ++i) // stopping condition?
        {
            yield return new WaitForSeconds(PoopingDuration);
            if (!dirtyRoom.DirtyLevelUp())
                break;
        }
        dirtyRoom.SetCanBeDestroyed();
        StopPooping();
    }

    private void StopPooping()
    {
        IsPooping = false;
        HippoAnimator.SetBool("IsPooping", false);
        if (PoopsSoundSource != null)
        {
            PoopsSoundSource.Stop();
        }
        SelectNewTarget();
        StomachFullness = 0.0f;
    }

    public override bool NeedSelectNewTarget(Transform oldTarget)
    {
        if(StomachFullness >= 1.0f && !IsInRageMode && oldTarget.parent != null)
        {
            var room = oldTarget.parent.gameObject.GetComponent<RoomScript>();
            if(room != null && room.GetComponentInChildren<DirtyRoom>() == null)
            {
                StartPooping(room);
                return false;
            }
        }

        return base.NeedSelectNewTarget(oldTarget);
    }

    // Use this for initialization
    internal new void Start () {
        base.Start();
        DefaultMaxVecity = MaxVelocity;
        DefaultMaxAcceleration = MaxAcceleration;
        PoopsSoundSource = GetComponent<AudioSource>();
        HippoAnimator = GetComponent<Animator>();
        HippoSprites = GetComponentsInChildren<SpriteMeshInstance>();
    }

    // Update is called once per frame
    internal new void Update () {
        base.Update();

        if(DirtyLevel < 1.0f)
        {
            DirtyLevel += DirtyPerSecond * Time.deltaTime;
            if (DirtyLevel > 1.0f)
            {
                DirtyLevel = 1.0f;
                if (!StartRageMode())
                    DirtyLevel = 0.9f;
            }
        }
        Color hippoColor = Color.Lerp(Color.white, DirtyColor, DirtyLevel);
        foreach(var sprite in HippoSprites)
        {
            sprite.color = hippoColor;
        }
    }
}
