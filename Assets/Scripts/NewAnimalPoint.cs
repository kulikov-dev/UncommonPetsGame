using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAnimalPoint : MonoBehaviour {

    public GameObject[] Animals;
    public float SpawnDuration = 1.0f;
    private int AnimalNum = 0;

    //private List<GameObject> AnimalsList = new List<GameObject>();
    // Use this for initialization
    void Start () {
        //AnimalsList.AddRange(Animals);
    }

    public bool CanSpawnAnimal()
    {
        return AnimalNum < Animals.Length;
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
        Instantiate(Animals[AnimalNum], transform.position, transform.rotation);
        AnimalNum++;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
