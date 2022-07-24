using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform Player;
    public float goBac = 100;
    public float EnemyHealth = 100;
    Rigidbody rb;
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {

        if (Player.position.x < transform.position.x)
        {
            Vector3 drag = new Vector3(-90,0, 0).normalized;
            Quaternion rotation = Quaternion.LookRotation(drag);
            transform.rotation = rotation;
        }
        if (Player.position.x > transform.position.x)
        {
            Vector3 drag = new Vector3(90, 0, 0).normalized;
            Quaternion rotation = Quaternion.LookRotation(drag);
            transform.rotation = rotation;
        }
    }
    public void OnAttack()
    {
        //rb.AddForce(-transform.forward*goBac);
        transform.position = transform.position + transform.forward*-0.5f;
    }
    public void HealthGo(float range)
    {
        EnemyHealth -= 20;
    }
}
