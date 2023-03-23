using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollidingWithRock : MonoBehaviour
{
    public Text collisionNumberText;
    public int collisionNumber = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("car"))
        {
            collisionNumber++;
        }
    }

    private void Update()
    {
        collisionNumberText.text = "Collisions with rocks: " + collisionNumber.ToString();
    }
}
