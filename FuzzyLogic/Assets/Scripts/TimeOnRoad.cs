using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOnRoad : MonoBehaviour
{

    private float timeOnRoad = 0;
    private bool isOnRoad = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isOnRoad==true)
        {
            timeOnRoad += Time.deltaTime;
            //Debug.Log(timeOnRoad);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("car"))
        {
            isOnRoad = false;
            //Debug.Log("OffRoad");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("car"))
        {
            isOnRoad = true;
            //Debug.Log("OnRoad");
        }
            
    }

    public float GetTime()
    {
        return timeOnRoad;
    }

    public void ResetTime()
    {
        timeOnRoad = 0;
    }
}
