using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollidingWithRock : MonoBehaviour
{
    [SerializeField] Text collisionNumberText;
    [SerializeField] Text resultNumberText;
    [SerializeField] Text resultTimeText;
    [SerializeField] TimeOnRoad timeOnReadRef;
    [SerializeField] int collisionNumber = 0;
    [SerializeField] int totalCollisionNumber = 0;
    [SerializeField] float timeframe = 10;

    private float timer = 0;
    private float result = 0;
    private float timeResult = 0;
    private int runNumb = 0;
    private int[] collsionNumbArray = new int[10];
    private float[] timeArray = new float[10];
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("car"))
        {
            collisionNumber++;
            totalCollisionNumber++;
        }
    }

    private void Update()
    {
        collisionNumberText.text = "Total collisions with rocks: " + totalCollisionNumber.ToString();

        if(timer > timeframe)
        {
            RunEnd();
        }

        timer += Time.deltaTime;
    }


    private void RunEnd()
    {
        timer = 0;
        collsionNumbArray[runNumb] = collisionNumber;
        timeArray[runNumb] = timeOnReadRef.GetTime();
        timeOnReadRef.ResetTime();
        collisionNumber = 0;
        runNumb++;

        if (runNumb == 10)
        {
            runNumb = 0;
            float collSum = 0;
            float timeSum = 0;
            for(int i = 0; i < collsionNumbArray.Length; i++)
            {
                collSum += collsionNumbArray[i];
                timeSum += timeArray[i];
            }
            result = collSum / collsionNumbArray.Length;
            timeResult = timeSum / timeArray.Length;
            resultNumberText.text = "Avarage collisions in 30 seconds: " + result;
            resultTimeText.text = "Avg time on road in 30 seconds: " + timeResult;
        }
    }
}
