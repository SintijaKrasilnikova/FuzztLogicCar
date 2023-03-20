using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{

    public Transform roadTranfRef;
    public Transform spawnTranfRef;
    [SerializeField] private GameObject rockRef;
    [SerializeField] private MeshRenderer roadSizeRef;

    public float timeBetweenRocks=1f;
    public float rockMaxDistanceFromRoad=3f;
    public float rockSpeed=3f;

    private float spawnTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SpawnPrefab()
    {
       
        float xPos = Random.Range(roadTranfRef.position.x - rockMaxDistanceFromRoad - (roadSizeRef.bounds.size.x / 2), roadTranfRef.position.x + rockMaxDistanceFromRoad + (roadSizeRef.bounds.size.x / 2));
        //float yPos = Random.Range(roadTranfRef.position.y - rockMaxDistanceFromRoad, roadTranfRef.position.y + rockMaxDistanceFromRoad);

        Instantiate(rockRef, new Vector3(xPos, 0.1f, spawnTranfRef.position.z), Quaternion.Euler(90,0,180));
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > timeBetweenRocks)
        {
            SpawnPrefab();
            spawnTimer = 0f;

        }

    }

}
