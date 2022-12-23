using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform startPos;

    private GameObject _activeCamera;
    [SerializeField] private Camera mainCam;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    public void ChangeCamare(GameObject yakaCam, Transform camPos)
    {
        _activeCamera = yakaCam;
        StartCoroutine(CameraMove(this.transform.position, camPos.position, 2f));
    }

    IEnumerator CameraMove(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * 3;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            this.transform.position = Vector3.Lerp(a, b, i);
            yield return null;
        }
     
        _activeCamera.SetActive(true);
        transform.position = startPos.position;
    }

    public void CameraStartPos()
    {
        transform.rotation = startPos.rotation;
        ReturnCamera();
    }

    public void ReturnCamera()
    {
        this.transform.position = startPos.position;
    }
}