using UnityEngine;

public class SC_HouseMonkeyItem : SC_BaseMonkeyItem
{

    public enum_ToolType ItemType;
    private void ActivateItemType(bool isActive)
    {
        var protagonist = FindObjectOfType<SC_Protagonist>();
        if (protagonist == null)
            Debug.Log("protagonist == null");
        switch (ItemType)      
        {
            case enum_ToolType.Food:
                protagonist.SetFoodActive(isActive);
                break;
            case enum_ToolType.Shower:
                protagonist.SetShowerActive(isActive);
                break;
        }
    }

    public override void GetItem(Transform monkeyAttachPoint)
    {
        base.GetItem(monkeyAttachPoint);
        ActivateItemType(false);
    }

    public override void RecoverItem()
    {
        ActivateItemType(true);
    }
}
