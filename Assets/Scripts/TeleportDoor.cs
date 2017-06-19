using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour {

    public Transform TeleportTarget;
    public int Level;

    public bool TeleportAnimal(Animal animal)
    {
        var targetDoor = TeleportTarget.gameObject.GetComponent<TeleportDoor>();
        if (TeleportTarget != null && targetDoor != null)
        {
            animal.StopMoving();
            animal.transform.Translate(TeleportTarget.position - animal.transform.position);
            animal.Level = targetDoor.Level;
            animal.StartMoving();
            return true;
        }
        return false;
    }
}
