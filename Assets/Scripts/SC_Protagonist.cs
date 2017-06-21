
using UnityEngine;
using UnityEngine.UI;

public class SC_Protagonist : MonoBehaviour
{
    public Button ButtonFood;
    public Button ButtonShower;

    public Texture2D HandCursorTexture;
    public Texture2D ShowerCursorTexture;
    public Texture2D FoodCursorTexture;

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

    public void SetActionHand()
    {
        ToolType = enum_ToolType.Hand;
        Cursor.SetCursor(HandCursorTexture, Vector2.zero, CursorMode.Auto);
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
        }
    }

    private enum enum_ToolType
    {
        Hand,
        Shower,
        Food
    }
}
