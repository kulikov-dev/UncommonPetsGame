using System;
using UnityEngine;

/// <summary> Точка с проводкой </summary>
public class SC_PointEnergy : MonoBehaviour, ICleanable
{
    public SpriteRenderer FireSprite;

    /// <summary> Кол-во кликов для починки </summary>
    public int ConstTryToAttempt = 3;
    private int currentTryToAttmpt = 3;

    private bool isLightBroken = false;
    public bool IsLightBroken
    {
        get { return isLightBroken; }
        private set
        {
            isLightBroken = value;
            if (isLightBroken)
            {
                currentTryToAttmpt = ConstTryToAttempt;
                FireSprite.transform.localScale = new Vector3(0.5659826f, 0.5659826f);
                FireSprite.enabled = true;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetIsLightBroken(bool newValue)
    {
        IsLightBroken = newValue;
        var dayNightController = FindObjectOfType<DayNightController>();
        dayNightController.SetLightOn(!newValue);
    }

    /// <summary> Проводка сломана 
    /// отмечает контроллеру дня и ночи, что свет в доме не работает
    /// </summary>
    public void DestroyEnergy()
    {
        SetIsLightBroken(true);        
    }

    void ICleanable.Clean()
    {
        --currentTryToAttmpt;
        switch (currentTryToAttmpt)
        {
            case 2:
                FireSprite.transform.localScale = new Vector3(0.4659826f, 0.4659826f);
                break;
            case 1:
                FireSprite.transform.localScale = new Vector3(0.2659826f, 0.2659826f);
                break;
            case 0:
                FireSprite.enabled = false;
                SetIsLightBroken(false);
                break;
        }

    }
}
