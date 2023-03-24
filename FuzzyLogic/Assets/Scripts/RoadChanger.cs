using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChanger : MonoBehaviour
{
    [SerializeField] LevelSO levelNumbRef;
    [SerializeField] GameObject roadRef;

    private int currentLevel;
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
                    break;
                }
            case 3:
                {
                    break;
                }
        }
    }

    private void MoveRoadSlow()
    {

    }

    private void MoveRoadFast()
    {

    }
}
