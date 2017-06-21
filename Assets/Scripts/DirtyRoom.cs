using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyRoom : MonoBehaviour {
    public List<DirtySpot> HiddenDirtySpots = new List<DirtySpot>();
    public List<DirtySpot> VisibleDirtySpots = new List<DirtySpot>();
    private bool CanBeDestroyed = false;

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

    public void Initialize()
    {
        var dirtySpots = GetComponentsInChildren<DirtySpot>();
        foreach (var spot in dirtySpots)
        {
            spot.DirtyRoom = this;
            HiddenDirtySpots.Add(spot);            
        }
    }

    // Use this for initialization
    void Start () {        
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
