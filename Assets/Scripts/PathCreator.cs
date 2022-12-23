using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public LineRenderer _lineRenderer;

    private List<Vector3> points = new List<Vector3>();


    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    // Start is called before the first frame update

    [SerializeField] private List<GameObject> _cameras;

    private LevelController _levelController;
    public bool goMove = false;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _levelController = GameObject.Find("LevelManager").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            points.Clear();
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Plane")
                {
                    if (DistanceToLastPoint(hit.point) > 1f)
                    {
                        points.Add(hit.point);
                        _lineRenderer.positionCount = points.Count;
                        _lineRenderer.SetPositions(points.ToArray());
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            goMove = true;
            OnNewPathCreated(points);
            ChangeCamera();
        }
    }

    private void ChangeCamera()
    {
        _lineRenderer.enabled = false;
    }

    private float DistanceToLastPoint(Vector3 hitPoint)
    {
        if (!points.Any())
            return Mathf.Infinity;
        return Vector3.Distance(points.Last(), hitPoint);
    }


    public void ClearPoints()
    {
        points.Clear();
    }
}