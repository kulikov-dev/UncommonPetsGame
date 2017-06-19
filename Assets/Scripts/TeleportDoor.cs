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
            var delta = TeleportTarget.position - animal.transform.position;
            animal.transform.Translate(new Vector3(delta.x, delta.y, 0.0f));
            animal.Level = targetDoor.Level;
            animal.StartMoving();
            return true;
        }
        return false;
    }
}
