using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{

    //Параметры движения	
    public float MaxVelocity = 2f;
    public float MaxAcceleration = 4f;
    //Точка к которой идет животное/девочка
    public Transform Target = null;
    //Уровень на котором находится животное/девочка	
    public int Level = 0;
    public float CheckTargetDistance = 1f;
    public float ScaleFactor = 1.0f;
    public float SpawnHeight = 0.0f;

    private bool IsMoving = false;
    private float Acceleration = 4.0f;
    private float Velocity = 0.0f;

    /// <summary> Параметр здоровья у животного/девочки </summary>
    public float Health = 100f;
    private SC_Hungry HungryIcon;
    
    /// <summary> Голод изменяется в промежутке от 0 до 1, если голод больше 0.5 то показывать индикатор, что пора кормить животное </summary>
    public float Hunger = 0f;
    /// <summary> Прирост голода в секунду </summary>
    public float AddHungerPerSecond = 0.05f;

    /*NEW*/
    /// <summary> Насколько снижать рассудок девочке при смерти </summary>
    public float DeclineOfMindValue = 25.0f;
    /// <summary> Урон в секунду, когда голод = 0 </summary>
    public float DamageFromHungerPerSecond = 10.0f;
    /// <summary> Голодает ли животное сейчас или нет </summary>
    private bool IsHungry = false;

    private Coroutine C_HungryDamage;

    public GameObject BloodyRoomPrefab;

    protected AudioSource HungerSoundSource;
    protected AudioSource DamageSoundSource;
    protected AudioSource DeathSoundSource;
    protected AudioSource EatingSoundSource;    

    /*NEW*/

    public virtual void OnDeath()
    {   
        var deadAnimal = gameObject.GetComponent<SC_DeadAnimal>();
        if (deadAnimal == null)
        {
            deadAnimal = gameObject.AddComponent<SC_DeadAnimal>();
            if (HungryIcon != null)
                HungryIcon.Hide();

            enabled = false;

            /*NEW*/
            SetIsHungry(false);

            var animator = GetComponent<Animator>();
            
            if (animator != null)
                animator.SetBool("IsDead", true);
            //Если девочка еще не ушла, отнимаем у нее рассудок, иначе проверяем условие победы
            var girl = FindObjectOfType<Girl>();
            if (girl.IsLeftHouse)
            {
                var protagonist = FindObjectOfType<SC_Protagonist>();
                protagonist.ChackVictoryConditions();
            }
            else
            {
                girl.ReduceReasonLevel(DeclineOfMindValue);
            }
            /*NEW*/
        }
    }

    /*NEW*/
    public void Kill()
    {
        if (Health > 0)
        {
            GetDamage(1000.0f);

            if (BloodyRoomPrefab != null)
            {
                //Вот тут надо создать комнату с кровищей, но для этого нужно понять в какой мы комнате
                //Как вариант - найти комнату, которая нам ближе всех сейчас, должно проканать
                var rooms = GameObject.FindGameObjectsWithTag("Level_" + Level.ToString()); //Ищем комнату по этажу
                if (rooms.Length > 0)
                {
                    var currentRoom = rooms[0];
                    float minDist = Vector3.Distance(transform.position, currentRoom.transform.position);
                    for (int i = 1; i < rooms.Length; ++i)
                    {
                        var distance = Vector3.Distance(transform.position, rooms[i].transform.position);
                        if (distance < minDist)
                        {
                            minDist = distance;
                            currentRoom = rooms[i];
                        }
                    }
                    var room = currentRoom.GetComponent<RoomScript>();
                    if (room != null && room.GetComponentInChildren<DirtyRoom>() == null)
                    {
                        var bloodyRoom = Instantiate(BloodyRoomPrefab);
                        bloodyRoom.transform.parent = room.transform;
                        bloodyRoom.transform.position = room.transform.position;
                        var bloodyRoomScript = bloodyRoom.GetComponent<DirtyRoom>();
                        if (bloodyRoomScript != null)
                        {
                            bloodyRoomScript.Initialize(true);
                        }
                    }
                }
            }
        }
    }
    /*NEW*/

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
                    for (int i = 0; i < room.TargetPoints.Length; ++i)
                    {
                        if (!CheckDistance(newTarget))
                            break;
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
            if (Acceleration * (transform.localScale.x * ScaleFactor) < 0.0f)
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
        transform.position += new Vector3(0.0f, SpawnHeight, 0.0f);
        StartMoving();

        HungryIcon = gameObject.GetComponentInChildren<SC_Hungry>();
        if (HungryIcon != null)
            HungryIcon.Hide();

        HungerSoundSource = gameObject.AddComponent<AudioSource>();
        HungerSoundSource.clip = Resources.Load<AudioClip>("Hunger");
        HungerSoundSource.volume = 0.2f;

        DamageSoundSource = gameObject.AddComponent<AudioSource>();
        DamageSoundSource.clip = Resources.Load<AudioClip>("AnimalDamage");
        DamageSoundSource.volume = 0.3f;

        DeathSoundSource = gameObject.AddComponent<AudioSource>();
        DeathSoundSource.clip = Resources.Load<AudioClip>("Death");
        DeathSoundSource.volume = 0.2f;

        EatingSoundSource = gameObject.AddComponent<AudioSource>();
        EatingSoundSource.clip = Resources.Load<AudioClip>("Eating");
        EatingSoundSource.volume = 0.2f;
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

        if (Hunger < 1.0f)          // прирост голода.
        {
            var hungerDelta = Time.deltaTime * AddHungerPerSecond;
            if (Hunger < 0.3f && Hunger + hungerDelta >= 0.3f)
            {
                if (HungryIcon != null)
                    HungryIcon.Show();
            }

            if (Hunger < 0.75f && Hunger + hungerDelta >= 0.75f)
            {
                PlaySound(HungerSoundSource);
            }

            Hunger += Mathf.Clamp(hungerDelta, 0.0f, 1.0f);
        }
        else
            SetIsHungry(true);

        if (HungryIcon != null)
            UpdateHungrySprite();
    }

    private bool CheckDistance(Transform targetTransform)
    {
        return Mathf.Abs(targetTransform.position.x - transform.position.x) < CheckTargetDistance;
    }

    private void UpdateHungrySprite()
    {
        var hunger = Hunger * 2.0f;
        //spritecolor - это цвет спрайта индикатора
        //yellow, green, red - цвета, между которыми будет плавно меняться цвет индикатора
        if (hunger > 1.0f)
            HungryIcon.BackgroundSprite.color = Color.Lerp(Color.yellow, Color.red, hunger - 1.0f);
        else
            HungryIcon.BackgroundSprite.color = Color.Lerp(Color.green, Color.yellow, hunger);

    }

    /// <summary> Покормить создание </summary>
    public virtual void FeedCreature()
    {
        if (Hunger <= 0.3f)
            return;

        PlaySound(EatingSoundSource);
        Hunger = 0f;        
        SetIsHungry(false);
        if (HungryIcon != null)
            HungryIcon.Hide();
    }

    /*NEW*/
    void SetIsHungry(bool newValue)
    {
        if (IsHungry == newValue)
            return;

        IsHungry = newValue;
        if (IsHungry)
            C_HungryDamage = StartCoroutine(HungryDamage(1.0f));
        else
            StopCoroutine(C_HungryDamage);
    }

    IEnumerator HungryDamage(float duration)
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            GetDamage(DamageFromHungerPerSecond);
        }
    }
    /*NEW*/

    /// <summary> Показать сообщение, о том что пора покормить создание. </summary>
    public virtual void ShowHungerMessage()
    {

    }

    public virtual void GetDamage(float damage)
    {
        if (Health <= 0.0f)
            return;

        Health = Mathf.Clamp(Health - damage, 0, 100);
        if (Health <= 0.0f)
        {
            Health = 0.0f;
            PlaySound(DeathSoundSource);
            OnDeath();
        }
        else
            PlaySound(DamageSoundSource);
    }

    protected void PlaySound(AudioSource source)
    {
        if (source.isPlaying)
            return;
        source.Play();
    }
}
