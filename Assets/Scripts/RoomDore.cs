using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDore : MonoBehaviour {

    private int ObjectsCounter = 0; //Если = 0, то дверь закрыта
    public SpriteRenderer DoorSprite;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnCollisionEnter2D");
        if (other.gameObject.GetComponent<Animal>() != null)
        {
            ObjectsCounter++;
            if (!DoorSprite.enabled)
                DoorSprite.enabled = true;
        }            
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Animal>() != null)
        {
            ObjectsCounter--;
            if(ObjectsCounter <= 0)
            {
                ObjectsCounter = 0;
                DoorSprite.enabled = false;
            }
        }            
    }
}
