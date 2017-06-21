using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtySpot : MonoBehaviour, ICleanable {
    public DirtyRoom DirtyRoom;
    public SpriteRenderer SpotSprite;

    public void Clean()
    {
        Debug.Log("Clean dirty spot");
        if (DirtyRoom != null)
            DirtyRoom.DirtyLevelDown(this);
    }

    public void Hide()
    {
        if (SpotSprite != null)
            SpotSprite.enabled = false;
    }

    public void Show()
    {
        if (SpotSprite != null)
            SpotSprite.enabled = true;
    }

    // Use this for initialization
    void Start () {
        SpotSprite = gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
