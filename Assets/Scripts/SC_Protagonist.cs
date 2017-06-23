
using UnityEngine;
using UnityEngine.UI;

public class SC_Protagonist : MonoBehaviour
{
    public Button ButtonFood;
    public Button ButtonShower;
    public Button ButtonHand;
    public Button ButtonGun;

    public Texture2D HandCursorTexture;
    public Texture2D ShowerCursorTexture;
    public Texture2D FoodCursorTexture;
    public Texture2D GunCursorTexture;

    private enum_ToolType ToolType = enum_ToolType.Shower;

    // Use this for initialization
    void Start()
    {
        SetActionHand();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetShowerActive(bool isActive)
    {
        if (!isActive && ToolType == enum_ToolType.Shower)
            SetActionHand();
        ButtonShower.interactable = isActive;
    }

    public void SetFoodActive(bool isActive)
    {
        if (!isActive && ToolType == enum_ToolType.Food)
            SetActionHand();
        ButtonFood.interactable = isActive;
    }

    public void SetGunActive(bool isActive)
    {
        if (!isActive && ToolType == enum_ToolType.Gun)
            SetActionHand();
        ButtonFood.interactable = isActive;
        if(isActive)
        {
            //Проверяем, есть ли еще живые животные, если нет - конец игры
            ChackVictoryConditions();
        }
    }

    //Проверка, остался ли кто живой
    public void ChackVictoryConditions()
    {
        var animals = FindObjectsOfType<Animal>();
        bool success = true;
        foreach(var animal in animals)
        {
            if(!(animal is Girl) && animal.Health > 0.0f)
            {
                success = false;
                break;
            }
        }
        if(success)
        {
            //Game over
        }
    }

    public void SetActionHand()
    {
        if (ButtonHand.interactable)
        {
            ToolType = enum_ToolType.Hand;
            Cursor.SetCursor(HandCursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    public void SetActionShower()
    {
        if (ButtonShower.interactable)
        {
            ToolType = enum_ToolType.Shower;
            Cursor.SetCursor(ShowerCursorTexture, Vector2.zero, CursorMode.Auto);
        }        
    }

    public void SetActionFood()
    {
        if (ButtonFood.interactable)
        {
            ToolType = enum_ToolType.Food;
            Cursor.SetCursor(FoodCursorTexture, Vector2.zero, CursorMode.Auto);
        }        
    }

    public void SetActionGun()
    {
        if (ButtonGun.interactable)
        {
            ToolType = enum_ToolType.Gun;
            Cursor.SetCursor(GunCursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnMouseAction(MonoBehaviour item)
    {
        switch (ToolType)
        {
            case enum_ToolType.Shower:
                var cleanable = item as ICleanable;
                if (cleanable != null)
                    cleanable.Clean();
                break;
            case enum_ToolType.Hand:
                var touchable = item as ITouchable;
                if (touchable != null)
                    touchable.Touch();
                break;
            case enum_ToolType.Food:
                var animal = item as Animal;
                if (animal != null)
                    animal.FeedCreature();
                break;
            case enum_ToolType.Gun:
                var anim = item as Animal;
                if (anim != null)
                    anim.Kill();
                break;
        }
    }
}
