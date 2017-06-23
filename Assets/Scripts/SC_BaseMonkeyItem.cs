using UnityEngine;

/// <summary> Базовый класс предметов, Которые может воровать обезъяна </summary>
public class SC_BaseMonkeyItem : MonoBehaviour, ITouchable
{

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
    /*CHANGED*/
    public virtual void DropItem(Vector3 position)
    {
        transform.parent = null;
        transform.position = position;
        gameObject.transform.rotation = Quaternion.identity;
        // TODO бросать на позицию пола.
    }
    /*CHANGED*/
    /// <summary> Обезъяна подобрала предмет </summary>
    public virtual void GetItem(Transform monkeyAttachPoint)
    {
        gameObject.transform.parent = monkeyAttachPoint.parent;
        gameObject.transform.position = monkeyAttachPoint.position;        
    }

    /// <summary> Поставить предмет на место </summary>
    public virtual void RecoverItem()
    {

    }

    /// <summary> По щелчку - возвращаем предмет на место </summary>
    public void Touch()
    {
        if(transform.parent == null)
        {
            gameObject.transform.position = StartPosition;
            gameObject.transform.rotation = Quaternion.identity;
            RecoverItem();
        }       
    }
}
