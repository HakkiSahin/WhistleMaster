using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isCharacterMovement = true;
    private Rigidbody myRigid;
    [SerializeField] private LevelController _levelController;

    [SerializeField] private Animator _animator;
    private bool isAnim = true;
    [SerializeField] private List<GameObject> _cameras;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        _levelController = GameObject.Find("LevelManager").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharacterMovement)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            CharacterMovement();
        }

        if (isCharacterMovement && isAnim)
        {
            Time.timeScale = 1f;
            _animator.SetTrigger("Running");
            isAnim = false;
            _cameras[1].SetActive(false);
            _cameras[0].SetActive(true);
        }
    }

    private void CharacterMovement()
    {
        transform.Translate(0, 0, Time.deltaTime * 10f);
        //myRigid.AddForce(Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LevelStart")
        {
            other.transform.gameObject.SetActive(false);
            _animator.SetTrigger("Idle");
            isCharacterMovement = false;
            isAnim = true;
            this.transform.GetChild(0).gameObject.SetActive(false);
            _levelController.SetLevel();
        }

        if (other.tag == "LevelEnd")
        {
            _levelController._uıController.OpenGameEndMenu(true);
        }

        if (other.tag == "OpenDoor")
        {
            StartCoroutine(OpenDoor(other.transform.parent.gameObject));
        }
    }


    IEnumerator OpenDoor(GameObject obj)
    {
        float i = 0.0f;
        float rate = (1.0f / 2) * 3;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            obj.transform.GetChild(0).localScale = Vector3.Lerp(obj.transform.GetChild(0).localScale,
                new Vector3(0, 100, 100), i);
            obj.transform.GetChild(1).localScale = Vector3.Lerp(obj.transform.GetChild(1).localScale,
                new Vector3(0, 100, 100), i);
            yield return null;
        }
    }
}