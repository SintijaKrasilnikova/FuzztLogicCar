using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CloseDistanceSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text closeText;
    [SerializeField] LevelSO levelNumbRef;
    [SerializeField] FuzzyBox fuzzyBox;

    private int closeDist;
    //reference for ui slider https://www.youtube.com/watch?v=nTLgzvklgU8

    // Start is called before the first frame update
    void Start()
    {
        closeText.text = "Close distance: " + levelNumbRef.closeDist;
        closeDist = levelNumbRef.closeDist;
        // slider.value = levelNumb;
        slider.value = closeDist;
        slider.onValueChanged.AddListener((v) => { 
            closeText.text = "Close distance: " + v.ToString(); 
            closeDist = (int)v;
            Invoke("ResetLevel", 1);
            levelNumbRef.closeDist = closeDist;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        });
       
    }

    // Update is called once per frame
    void Update()
    {
        levelNumbRef.closeDist = closeDist;
    }
    private void ResetLevel()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        //fuzzyBox.closeDistance = closeDist;
        // Debug.Log("changes");

    }

}
