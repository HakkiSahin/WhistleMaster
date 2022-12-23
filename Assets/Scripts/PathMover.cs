using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathMover : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();

    public bool isEnd = false;
    [SerializeField] GameObject _camera;
    [SerializeField] private Transform camPos;


    [SerializeField] private Vector3 testPos;
    private bool testMove = false;

    [SerializeField] private PathCreator _creator;

    [SerializeField] private Animator _animator;

    private bool animStart;
    private int count = 0;
    public GameObject arrowList;

    private LevelController _level;
    private UIController _uıController;

    private void Awake()
    {
        testPos = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        FindObjectOfType<PathCreator>().OnNewPathCreated += SetPoints;
        _level = GameObject.Find("LevelManager").GetComponent<LevelController>();
        _uıController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    private void SetPoints(IEnumerable<Vector3> obj)
    {
        pathPoints = new Queue<Vector3>(obj);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePathing();

        if (_agent.velocity != Vector3.zero)
        {
            testMove = true;
        }


        if (_creator.goMove)
        {
            _agent.isStopped = false;
        }

        if (!animStart && testMove)
        {
            animStart = true;
            _animator.SetTrigger("Yaka");
        }

        if (_agent.velocity == Vector3.zero && testMove)
        {
            arrowList.transform.GetChild(count).gameObject.SetActive(false);
            count++;
            animStart = false;
            _animator.SetTrigger("Idle");
            _agent.SetDestination(testPos);
            _creator.goMove = false;
            testMove = false;

            //Position Zero
            this.transform.position = testPos;
            this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }

        if (count >= 3)
        {
            _uıController.OpenGameEndMenu(false);
        }
    }

    private void UpdatePathing()
    {
        if (ShouldSetDestination())
        {
            _agent.SetDestination(pathPoints.Dequeue());
        }
    }

    private bool ShouldSetDestination()
    {
        if (pathPoints.Count == 0)
        {
            return false;
        }

        if (_agent.hasPath == false || _agent.remainingDistance < 0.5f)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "SlowSpeed")
        {
            _camera.GetComponent<CameraFollow>().ChangeCamare(transform.GetChild(1).gameObject,
                transform.GetChild(0));
        }
    }
}