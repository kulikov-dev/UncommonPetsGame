using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour {
    public bool IsLightOn = true;
    public float DailyCycleTime = 10.0f;    

    private bool IsDay = true;
    private float CurrentTime = 0.0f;

    /*NEW*/
    //Трансформ индикатора на календаре
  //  public Transform DayIndicatorTransform;
    //На сколько сдвигать индикатор на календаре за сутки
    public Vector3 DayIndicatorOffset;
    //Сколько дней осталось до завершения игры
    public int DaysLeft = 5;
    /*NEW*/

    private RoomScript[] Rooms;
	// Use this for initialization
	void Start () {
        Rooms = FindObjectsOfType<RoomScript>();
	}

    /*NEW*/
    public bool IsFullDarkness()
    {
        return !(IsDay || IsLightOn);
    }
    /*NEW*/

    // Update is called once per frame
    void Update () {
        if (DaysLeft == 0)
            return;

        CurrentTime += Time.deltaTime;
        if (CurrentTime > DailyCycleTime)
        {
            IsDay = true;
            CurrentTime -= DailyCycleTime;
            //Включаем день у всех комнат
            UpdateRooms();
            DaysLeft -= 1;
            //DayIndicatorTransform.position += DayIndicatorOffset;
            if (DaysLeft == 0)
            {
                //Вышло время игры, отсылаем девочку домой и перестаем менять время суток
                var girl = FindObjectOfType<Girl>();
                girl.NeedToLeaveHouse = true;
            }
        }
        else if (IsDay && CurrentTime > DailyCycleTime * 0.5f)
        {
            IsDay = false;
            //Включаем ночь  у всех комнат и  обновляем свет
            UpdateRooms();
        }
    }

    public void SetLightOn(bool newValue)
    {
        IsLightOn = newValue;
        //Обновляем включение света во всех комнатах
        UpdateRooms();
    }

    private void UpdateRooms()
    {
        foreach(var room in Rooms)
        {
            room.SetDayNight(IsDay, IsLightOn);
        }
    }

}
