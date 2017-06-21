using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hippo : Animal, ICleanable
{
    public void Clean()
    {

    }

    // Use this for initialization
    internal new void Start () {
        base.Start();
	}

    // Update is called once per frame
    internal new void Update () {
        base.Update();
	}
}
