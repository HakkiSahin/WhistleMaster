using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool isEnemy;
    [SerializeField] private GameObject slowActive;
    [SerializeField] private GameObject touchParticle;
    private Animator _animator;

    [SerializeField] private Material _gray;

    private bool slowEffect = false;
    private UIController _uıController;

    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _uıController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null && this.transform.parent.childCount == 1)
        {
            slowActive.SetActive(true);
            slowEffect = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Whistle" && isEnemy)
        {
            this.transform.parent = null;
            Invoke("GetAnim", 0.2f);
        }

        if (other.tag == "Whistle" && !isEnemy)
        {
            //Invoke("GetAnim", 0.2f);
            StartCoroutine(HostageAnim());
        }
    }


    public void GetAnim()
    {
        if (slowEffect) Time.timeScale = 0.5f;

        Instantiate(touchParticle, transform.GetChild(0).GetChild(2).transform.position, quaternion.identity);
        for (int i = 0; i < transform.GetChild(0).childCount - 1; i++)
        {
            if (transform.GetChild(0).GetChild(i).GetComponent<SkinnedMeshRenderer>().enabled)
            {
                transform.GetChild(0).GetChild(i).GetComponent<SkinnedMeshRenderer>().material = _gray;
            }
        }

        this.transform.GetComponent<BoxCollider>().enabled = false;
        _animator.SetTrigger("isDie");
    }


    IEnumerator HostageAnim()
    {
        _animator.SetTrigger("isDie");
        yield return new WaitForSeconds(2f);
        _uıController.OpenGameEndMenu(false);
    }
}