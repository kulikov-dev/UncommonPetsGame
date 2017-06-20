using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

    //Параметры движения	
    public float MaxVelocity = 2f;
    public float MaxAcceleration = 4f;
    //Точка к которой идет животное/девочка
    public Transform Target = null;
    //Уровень на котором находится животное/девочка	
    public int Level = 0;
    public float CheckTargetDistance;
    public float Acceleration = 4.0f;
    public float Velocity = 0.0f;

    private bool IsMoving = false;

    /// <summary> Параметр здоровья у животного/девочки </summary>
    public float Health = 100f;
    /// <summary> Голод изменяется в промежутке от 0 до 1, если голод больше 0.5 то показывать индикатор, что пора кормить животное </summary>
    public float Hunger = 0f;
    /// <summary> Прирост голода в секунду </summary>
    private float hungerPerSecond = 0.01f;

    public virtual void SelectNewTarget() //Переопределим у девочки, чтобы время от времени она шла на улицу за новой тварью
    {
        var rooms = GameObject.FindGameObjectsWithTag("Level_" + Level.ToString()); //Ищем комнату по этажу
        if (rooms.Length > 0)
        {
            //Выбираем в рандомной комнате рандомную точку, которая не совпадает с предыдущей
            var room = rooms[Random.Range(0, rooms.Length)].GetComponent<RoomScript>();
            if (room != null && room.TargetPoints.Length > 0)
            {
                if (room.TargetPoints.Length > 1)
                {
                    var newTarget = room.TargetPoints[Random.Range(0, room.TargetPoints.Length)];
                    while (CheckDistance(newTarget))
                    {
                        newTarget = room.TargetPoints[Random.Range(0, room.TargetPoints.Length)];
                    }
                    SetTarget(newTarget);
                }
                else
                {
                    SetTarget(room.TargetPoints[0]);
                }
                
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;

        Acceleration = Target.position.x - transform.position.x;
        if (Acceleration != 0.0f)
        {
            Acceleration = Acceleration * MaxAcceleration / Mathf.Abs(Acceleration);
            if (Acceleration * transform.localScale.x < 0.0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
            Acceleration = MaxAcceleration;
    }

    public void StopMoving()
    {
        IsMoving = false;
    }

    public void StartMoving()
    {
        IsMoving = true;
        if (Target == null)
            SelectNewTarget();
    }

    internal void Start()
    {
        StartMoving();
    }

    public virtual bool NeedSelectNewTarget(Transform oldTarget)
    {
        return true;
    }

    internal void Update()
    {
        if (IsMoving && Target != null) //Меняем скорость и позицию со временем
        {
            Velocity = Mathf.Clamp(Velocity + Acceleration * Time.deltaTime, -MaxVelocity, MaxVelocity);
            float distance = Velocity * Time.deltaTime;
            transform.Translate(Vector3.right * distance);
            //Если достигли цели, пробуем телепортироваться		
            if (CheckDistance(Target))
            {
                var teleport = Target.gameObject.GetComponent<TeleportDoor>();
                var oldTarget = Target;
                Target = null;
                //Если не удалось телепортироваться, выбираем себе другую цель
                if ((teleport == null || !teleport.TeleportAnimal(this)) && NeedSelectNewTarget(oldTarget))
                {                    
                    SelectNewTarget();
                }
            }
        }

        if (Hunger != 1)          // прирост голода.
            Hunger = Mathf.Clamp(Time.deltaTime * hungerPerSecond, 0, 1);
        if (Hunger > 0.5)
            ShowHungerMessage();
    }

    private bool CheckDistance(Transform targetTransform)
    {
        return Mathf.Abs(targetTransform.position.x - transform.position.x) < CheckTargetDistance;
    }

    /// <summary> Покормить создание </summary>
    public virtual void FeedCreature()
    {
        Hunger = 0f;
    }
    /// <summary> Показать сообщение, о том что пора покормить создание. </summary>
    public virtual void ShowHungerMessage()
    {

    }
}
