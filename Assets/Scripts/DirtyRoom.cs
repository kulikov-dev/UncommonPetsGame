using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyRoom : MonoBehaviour {
    public List<DirtySpot> HiddenDirtySpots = new List<DirtySpot>();
    public List<DirtySpot> VisibleDirtySpots = new List<DirtySpot>();
    /*NEW*/
    public float DamagePerSecond = 1.0f;
    private List<Animal> DamagedAnimals = new List<Animal>();
    /*NEW*/
    public bool CanBeDestroyed = false;

    /*NEW*/
    void OnTriggerEnter2D(Collider2D other)
    {
        var animal = other.gameObject.GetComponent<Animal>();
        if (animal != null)
        {
            if(!(animal is SC_Hippo))
            {
                DamagedAnimals.Add(animal);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var animal = other.gameObject.GetComponent<Animal>();
        if (animal != null)
        {
            DamagedAnimals.Remove(animal);
        }
    }
    /*NEW*/

    public void SetCanBeDestroyed()
    {
        CanBeDestroyed = true;
        if(VisibleDirtySpots.Count == 0)
            Destroy(gameObject);
    }

    public bool DirtyLevelUp()
    {
        if (HiddenDirtySpots.Count == 0)
            return false;
        var spot = HiddenDirtySpots[Random.Range(0, HiddenDirtySpots.Count)];
        spot.Show();
        HiddenDirtySpots.Remove(spot);
        VisibleDirtySpots.Add(spot);
        return true;
    }

    public void DirtyLevelDown(DirtySpot spot)
    {
        VisibleDirtySpots.Remove(spot);
        spot.Hide();
        HiddenDirtySpots.Add(spot);
        if(CanBeDestroyed && VisibleDirtySpots.Count == 0)
            Destroy(gameObject);
    }

    public void Initialize(bool isBloody = false)
    {
        var dirtySpots = GetComponentsInChildren<DirtySpot>();
        foreach (var spot in dirtySpots)
        {
            spot.DirtyRoom = this;
            /*CHANGED*/
            if(isBloody)
                VisibleDirtySpots.Add(spot); 
            else
                HiddenDirtySpots.Add(spot);
            /*CHANGED*/
        }
    }

    // Use this for initialization
    void Start () {
        /*NEW*/
        StartCoroutine(Damage(1.0f));
        /*NEW*/
    }

    /*NEW*/
    IEnumerator Damage(float duration)
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            foreach(var animal in DamagedAnimals)
            {
                animal.GetDamage(DamagePerSecond);
            }
        }
    }
    /*NEW*/

    // Update is called once per frame
    void Update () {
		
	}
}
