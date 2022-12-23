using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float radius;
    public float power;

    public MeshRenderer _mesh;

    void Start()
    {
        _mesh = transform.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Whistle")
        {
            Explosion();
        }
    }

    public void Explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        Destroy(gameObject.transform.GetChild(0).gameObject);
        _mesh.enabled = false;
        this.transform.parent.GetChild(1).gameObject.SetActive(true);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.transform.tag == "Enemy")
            {
                rb.transform.parent = null;
                // rb.transform.GetChild(0).GetComponent<Animator>().SetTrigger("isDie");
                rb.transform.GetComponent<EnemyController>().GetAnim();
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
        }
       
    }
}