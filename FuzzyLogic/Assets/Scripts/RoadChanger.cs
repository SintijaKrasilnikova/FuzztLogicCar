using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChanger : MonoBehaviour
{
    [SerializeField] LevelSO levelNumbRef;
    [SerializeField] GameObject roadRef;
    public float roadSpeedLevel2 = 10f;
    public float roadSpeedLevel3 = 20f;
    public float roadMovementBounds = 20f;
    private int currentLevel;
    private bool goingRight = true;
    private bool goingLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = levelNumbRef.levelNumber;

        
    }

    // Update is called once per frame
    void Update()
    {
        currentLevel = levelNumbRef.levelNumber;

        switch (currentLevel)
        {
            case 1:
                {
                    //road doesnt move
                    break;
                }
            case 2:
                {
                    MoveRoad(roadSpeedLevel2);
                    break;
                }
            case 3:
                {
                    MoveRoad(roadSpeedLevel3);
                    break;
                }
        }
    }

    private void MoveRoad(float currentSpeed)
    {
        if (roadRef.transform.position.x < roadMovementBounds && goingRight == true)
            roadRef.transform.position = new Vector3(roadRef.transform.position.x + currentSpeed * Time.deltaTime, roadRef.transform.position.y, roadRef.transform.position.z);
        else
        {
            goingLeft = true;
            goingRight = false;
        }


        if (roadRef.transform.position.x > -roadMovementBounds && goingLeft == true)
            roadRef.transform.position = new Vector3(roadRef.transform.position.x - currentSpeed * Time.deltaTime, roadRef.transform.position.y, roadRef.transform.position.z);
        else
        {
            goingLeft = false;
            goingRight = true;
        }

    }

}
