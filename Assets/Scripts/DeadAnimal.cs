using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAnimal : MonoBehaviour {

    /*NEW*/
    public float DeclineOfMindValue = 5.0f;
    /*NEW*/

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*NEW*/
    void OnTriggerEnter2D(Collider2D other)
    {
        var girl = other.gameObject.GetComponent<Girl>();
        if (girl != null)
        {
            girl.ReduceReasonLevel(DeclineOfMindValue);
        }
    }
    /*NEW*/
}
