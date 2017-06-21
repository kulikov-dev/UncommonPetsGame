using UnityEngine;

/// <summary> Базовый класс предметов, Которые может воровать обезъяна </summary>
public class SC_BaseMonkeyItem : MonoBehaviour, ITouchable
{
    /// <summary> Предмет находится на своей точке спауна </summary>
    public bool IsOnPlace = true;
    /// <summary> Точка, куда будет возвращаться итем </summary>
    private Vector3 StartPosition;
    /// <summary> Уровень, на котором находится предмет
    /// НЕ ЗАБЫТЬ проставить в редакторе!
    /// </summary>
    public int ItemLevel = -1;

    // Use this for initialization
    void Start()
    {
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary> Обезъяну заставили бросить предмет </summary>
    public virtual void DropItem()
    {
        gameObject.transform.parent = null;
        // TODO бросать на позицию пола.
    }
    /// <summary> Обезъяна подобрала предмет </summary>
    public virtual void GetItem(Transform monkeyAttachPoint)
    {
        gameObject.transform.parent = monkeyAttachPoint.parent;
        gameObject.transform.position = monkeyAttachPoint.position;
        IsOnPlace = false;
    }

    /// <summary> Поставить предмет на место </summary>
    public virtual void RecoverItem()
    {

    }

    private void OnMouseDown()
    {
        var protagonist = FindObjectOfType<SC_Protagonist>();
        protagonist.OnMouseAction(this);
    }
    /// <summary> По щелчку - возвращаем предмет на место </summary>
    public void Touch()
    {
        gameObject.transform.position = StartPosition;
        IsOnPlace = true;
        RecoverItem();
    }
}
