using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour {

    public Transform TeleportTarget;
    public int Level;

    public bool TeleportAnimal(Animal creature)
    {
        var targetDoor = TeleportTarget.gameObject.GetComponent<TeleportDoor>();
        if (TeleportTarget != null && targetDoor != null)
        {
            creature.StopMoving();
            var delta = TeleportTarget.position - transform.position;
            creature.transform.Translate(new Vector3(delta.x, delta.y, 0.0f));
            creature.Level = targetDoor.Level;
            creature.StartMoving();
            return true;
        }
        return false;
    }
}
