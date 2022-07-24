using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform Player;
    public float goBac = 100;
    float EnemyCurrentHealth;
    public float EnemyMaxHealth = 100;
    public GameObject HealthBar;
    Animator EnemyAnimation;
    Rigidbody rb;
    Vector3 drag;
    void Start()
    {
        EnemyAnimation = GetComponent<Animator>();
        EnemyCurrentHealth = EnemyMaxHealth;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        HealthBar.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position);
        HealthBar.GetComponent<Slider>().value = EnemyCurrentHealth / EnemyMaxHealth;
        if (Player.position.x < transform.position.x)
        {
            drag = new Vector3(-90,0, 0).normalized;
            Quaternion rotation = Quaternion.LookRotation(drag);
            transform.rotation = rotation;
        }
        if (Player.position.x > transform.position.x)
        {
            drag = new Vector3(90, 0, 0).normalized;
            Quaternion rotation = Quaternion.LookRotation(drag);
            transform.rotation = rotation;
        }
        
        Vector3 direction = drag;
        float turnSmoothVelocity = 60;
        if (direction.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 10f);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
    public void OnAttack()
    {
        //rb.AddForce(-transform.forward*goBac);
        transform.position = transform.position + transform.forward*-0.5f;
    }
    public void HealthGo(float range)
    {
        EnemyCurrentHealth -= range;
        if (EnemyCurrentHealth<=0)
        {
            EnemyCurrentHealth = 0.001f;
            Debug.Log("oldu");
        }
    }
}
