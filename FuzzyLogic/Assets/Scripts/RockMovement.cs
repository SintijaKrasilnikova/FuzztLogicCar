using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);

        if(transform.position.z <= -20)
        {
            Destroy(this.gameObject);
        }
    }

}
