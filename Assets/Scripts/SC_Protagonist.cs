
using UnityEngine;

public class SC_Protagonist : MonoBehaviour
{

    private enum_ToolType ToolType = enum_ToolType.Shower;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseAction(MonoBehaviour animal)
    {
        switch (ToolType)
        {
            case enum_ToolType.Shower:
                var cleanable = animal as ICleanable;
                if (cleanable != null)
                    cleanable.Clean();
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
