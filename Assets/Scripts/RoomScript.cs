using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {

    public int Level;
    public Transform[] TargetPoints;

    // Use this for initialization
    void Start()
    {
        gameObject.tag = "Level_" + Level.ToString();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
