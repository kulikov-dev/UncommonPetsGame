using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAnimalPoint : MonoBehaviour {

    public GameObject[] Animals;
    public float SpawnDuration = 1.0f;

    private List<GameObject> AnimalsList = new List<GameObject>();
    // Use this for initialization
    void Start () {
        AnimalsList.AddRange(Animals);
    }

    public bool CanSpawnAnimal()
    {
        return AnimalsList.Count > 0;
    }

    public void SpawnAnimal()
    {
        if (CanSpawnAnimal())
            StartCoroutine(SpawnAnimalWithDelay());
    }

    IEnumerator SpawnAnimalWithDelay()
    {
        yield return new WaitForSeconds(SpawnDuration);
        Debug.Log("Spawn animal");
        Instantiate(AnimalsList[0], transform.position, transform.rotation);
        AnimalsList.RemoveAt(0);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
