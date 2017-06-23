using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Bubble : MonoBehaviour {

    SpriteRenderer bubbleSprite;
    // Use this for initialization
    void Start()
    {
        bubbleSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*NEW*/
    public bool IsVisible()
    {
        return bubbleSprite.enabled;
    }
    /*NEW*/

    public void Show()
    {
        bubbleSprite.enabled = true;
    }
    public void Hide()
    {
        bubbleSprite.enabled = false;
    }
}
