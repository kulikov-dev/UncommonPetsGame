using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {

    public int Level;
    public Transform[] TargetPoints;
    public SpriteRenderer DarkRoom;
    public SpriteRenderer NightRoom;

    // Use this for initialization
    void Start()
    {
        gameObject.tag = "Level_" + Level.ToString();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetDayNight(bool isDay, bool isLightOn)
    {
        if (isDay)
        {
            DarkRoom.enabled = false;
            NightRoom.enabled = false;
        }
        else
        {
            //Видимость спрайтов меняем в зависимости от isLightOn
            DarkRoom.enabled = !isLightOn;
            NightRoom.enabled = isLightOn;
        }
    }

}
