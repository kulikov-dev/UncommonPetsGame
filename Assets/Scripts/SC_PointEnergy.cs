using UnityEngine;

/// <summary> Точка с проводкой </summary>
public class SC_PointEnergy : MonoBehaviour
{
    /// <summary> Спрайт для работающей проводки </summary>
    public Sprite Sprite_EnergyWork;
    /// <summary> Спрайт для сломаной проводки </summary>
    public Sprite Sprite_EnergyBroken;

    private bool isLightBroken = false;
    private bool IsLightBroken
    {
        get { return isLightBroken; }
        set
        {
            isLightBroken = value;
            transform.gameObject.GetComponent<SpriteRenderer>().sprite = value ? Sprite_EnergyBroken : Sprite_EnergyWork;
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

    /// <summary> Проводка сломана 
    /// отмечает контроллеру дня и ночи, что свет в доме не работает
    /// </summary>
    public void DestroyEnergy()
    {
        IsLightBroken = true;
        var dayNightController = FindObjectOfType<DayNightController>();
        dayNightController.SetLightOn(false);
    }

    /// <summary> Попытаться починить проводку </summary>
    public void TryToRepairEnergy()
    {
        //TODO запускает мини-игру, если она прошла успешно то отмечает что свет в доме работает
        IsLightBroken = false;
    }
}
