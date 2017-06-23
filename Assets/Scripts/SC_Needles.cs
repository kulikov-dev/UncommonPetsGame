using UnityEngine;

/// <summary> Иголки, которые дикообраз разбрасывает по комнатам </summary>
public class SC_Needles : MonoBehaviour, ITouchable
{
    /// <summary> Урон, которое получит существо, при встрече с иголкой </summary>
    public float Damage;
    /// <summary> Ссылка на господина дикообразыча. </summary>
    public Animal Parent;

        void OnTriggerEnter2D(Collider2D other)
    {
        var animal = other.gameObject.GetComponent<Animal>();
        if (animal != null && animal != Parent)
        {
            animal.GetDamage(Damage);
        }
    }

    /// <summary> Уничтожить иголки, при нажатии мышью </summary>
    void ITouchable.Touch()
    {
        Destroy(gameObject);
    }
}
