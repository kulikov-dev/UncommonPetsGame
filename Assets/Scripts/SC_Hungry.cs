using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Hungry : MonoBehaviour
{
    public SpriteRenderer BackgroundSprite;

    public void Hide()
    {
        foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
            sprite.enabled = false;

    }

    public void Show()
    {
        foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
            sprite.enabled = true;

    }
}
