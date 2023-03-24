using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReRock : MonoBehaviour
{

    [SerializeField] Transform roadTranfRef;
    [SerializeField] Transform spawnTranfRef;
   // [SerializeField] private GameObject rockRef;
    [SerializeField] private MeshRenderer roadSizeRef;

   // public float timeBetweenRocks = 1f;
    public float rockMaxDistanceFromRoad = 3f;
    public float rockSpeed = 3f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - rockSpeed * Time.deltaTime);

        if (transform.position.z <= -20)
        {
            float xPos = Random.Range(roadTranfRef.position.x - rockMaxDistanceFromRoad - (roadSizeRef.bounds.size.x / 2), roadTranfRef.position.x + rockMaxDistanceFromRoad + (roadSizeRef.bounds.size.x / 2));
            //float yPos = Random.Range(roadTranfRef.position.y - rockMaxDistanceFromRoad, roadTranfRef.position.y + rockMaxDistanceFromRoad);

            transform.position = new Vector3(xPos, 0.1f, spawnTranfRef.position.z);
            //Destroy(this.gameObject);
        }
    }

}
