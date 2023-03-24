using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text levelNumberText;
    [SerializeField] LevelSO levelNumbRef;

    private int levelNumb;
    //reference for ui slider https://www.youtube.com/watch?v=nTLgzvklgU8

    // Start is called before the first frame update
    void Start()
    {
        levelNumb = levelNumbRef.levelNumber;
       // slider.value = levelNumb;
        slider.onValueChanged.AddListener((v) => { levelNumberText.text = "Difficulty: " + v.ToString(); levelNumb = (int)v; });
        slider.value = levelNumb;
    }

    // Update is called once per frame
    void Update()
    {
        levelNumbRef.levelNumber = levelNumb;
    }
}
