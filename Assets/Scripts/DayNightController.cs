using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour {
    public bool IsLightOn = true;
    public float DailyCycleTime = 10.0f;    

    private bool IsDay = true;
    private float CurrentTime = 0.0f;

    private RoomScript[] Rooms;
	// Use this for initialization
	void Start () {
        Rooms = FindObjectsOfType<RoomScript>();
	}
	
	// Update is called once per frame
	void Update () {
        CurrentTime += Time.deltaTime;
        if (CurrentTime > DailyCycleTime)
        {
            IsDay = true;
            CurrentTime -= DailyCycleTime;
            //Включаем день у всех комнат
            UpdateRooms();
        }
        else if (CurrentTime > DailyCycleTime * 0.5f)
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
