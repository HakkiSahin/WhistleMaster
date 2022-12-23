using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElephantSDK;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Menus")] [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private int trailerPanelcount = 1;
    [SerializeField] private GameObject trailerPanel = null;

    private LevelController _levelController;


    void Start()
    {
        _levelController = GameObject.Find("LevelManager").GetComponent<LevelController>();

        Debug.Log(trailerPanelcount);
    }


    void Update()
    {
        if (trailerPanelcount == 0)
        {
            Time.timeScale = 0f;
        }
    }

    public void NextLevel()
    {
        _levelController.LevelEnd();
    }


    public void LevelAgain()
    {
        _levelController.FailEnd();
    }


    public void OpenGameEndMenu(bool menuType)
    {
        Time.timeScale = 0f;
        if (menuType)
        {
            winMenu.SetActive(true);
            SDKHelper(true);
        }
        else
        {
            loseMenu.SetActive(true);
            SDKHelper(false);
        }
    }

    private bool fooProt = false;
    void SDKHelper(bool ww)
    {
        if(fooProt) return;
        
        if (ww)
        {
            //won
            Elephant.LevelCompleted(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            //fail
            Elephant.LevelFailed(SceneManager.GetActiveScene().buildIndex);
        }

        fooProt = true;
    }

    public void StartGame()
    {
        trailerPanelcount = 1;
        trailerPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}