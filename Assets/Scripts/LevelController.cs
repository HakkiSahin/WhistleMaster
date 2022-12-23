using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cinemachine;
using ElephantSDK;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _cameras;

    [SerializeField] private GameObject levels;
    int levelIndex = 0;

    [Header("PlayerProperty")] [SerializeField]
    private PlayerController _player;

    [SerializeField] private List<GameObject> _yakas;

    [SerializeField] private PathCreator _pathCreator;

    private bool isMoving = false;

    [SerializeField] private TextMeshProUGUI text;


    public UIController _uıController;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        text.text = "LEVEL " + (PlayerPrefs.GetInt("Level") + 1).ToString();
        _uıController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    public bool prot = false;
    int currentSS = 0;
    void ScreenShot()
    {
        if(Input.GetKeyDown(KeyCode.K)) 
        {
            ScreenCapture.CaptureScreenshot("game" + PlayerPrefs.GetInt("s") + ".png");
            PlayerPrefs.SetInt("s", PlayerPrefs.GetInt("s") +1);
            Time.timeScale=0.001f;
            Debug.Log("Selection");
        }
        if(Input.GetKeyDown(KeyCode.A))Time.timeScale=1;
    } 
    // Update is called once per frame
    void Update()
    {
        ScreenShot();
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!prot)
            {
                Debug.Log("level started");
                Elephant.LevelStarted(SceneManager.GetActiveScene().buildIndex);
                prot = true;
            }
        }
        
        if (levelIndex <= levels.transform.childCount)
        {
            if (levels.transform.GetChild(levelIndex).transform.childCount <= 0)
            {
                Invoke("NextState", 2f);
            }

            // if (levels.transform.GetChild(levelIndex).transform.childCount <= 0 && levelIndex < levels.transform.childCount)
            // {
            //     Invoke("NextState", 2f);
            // }


            // yakas Move
            if (_yakas[levelIndex].GetComponent<NavMeshAgent>().velocity != Vector3.zero) isMoving = true;
            if (_yakas[levelIndex].GetComponent<NavMeshAgent>().velocity == Vector3.zero && isMoving)
            {
                isMoving = false;
                Again();
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            Again();
        }


        // if (levels.transform.childCount <= levelIndex)
        // {
        //     Invoke("LevelEnd", 2f);
        //     //LevelEnd();
        // }
    }


    public void ChangeCamera(bool cameraIndex)
    {
        // if (cameraIndex)
        // {
        //     _cameras[levelIndex + 1].SetActive(false);
        //     _cameras[0].SetActive(true);
        // }
        // else
        // {
        //     _cameras[0].SetActive(true);
        //     _cameras[levelIndex].SetActive(false);
        // }
    }


    public void CharacterMove(bool isMove)
    {
        _player.isCharacterMovement = true;
    }


    public void SetLevel()
    {
        _pathCreator._lineRenderer.enabled = true;
        _yakas[levelIndex].SetActive(true);
    }

    public void Again()
    {
        _pathCreator._lineRenderer.enabled = true;
        _cameras[0].SetActive(true);
        _cameras[levelIndex + 1].SetActive(false);
    }

    public void LevelEnd()
    {
        StartCoroutine(LevelEndEnum());
        //Debug.Log("Level Ended, win");
    }

    IEnumerator LevelEndEnum()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings
            ? Random.Range(1, SceneManager.sceneCountInBuildSettings)
            : SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }

    public void FailEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Debug.Log("Level Ended, fail");
    }

    private void NextState()
    {
        _yakas[levelIndex].SetActive(false);
        _cameras[0].GetComponent<CameraFollow>().CameraStartPos();

        if (levelIndex + 1 == levels.transform.childCount)
        {
            // _uıController.OpenGameEndMenu(true);
        }

        levelIndex++;
        CharacterMove(false);
    }
}