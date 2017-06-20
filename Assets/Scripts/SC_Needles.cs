using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Needles : MonoBehaviour, ITouchable
{
    /// <summary> Урон, которое получит существо, при встрече с иголкой </summary>
    public float Damage;
    public Animal Parent;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        var protagonist = FindObjectOfType<SC_Protagonist>();
        protagonist.OnMouseAction(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var animal = other.gameObject.GetComponent<Animal>();
        if (animal != null && animal != Parent)
        {
            animal.GetDamage(Damage);
        }
    }

    void ITouchable.Touch()
    {
        Destroy(gameObject);
    }
}
