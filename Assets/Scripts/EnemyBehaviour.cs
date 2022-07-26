using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform Player;
    public GameObject bit;
    public float goBac = 100;
    public float EnemyCurrentHealth;
    public float EnemyMaxHealth = 100;
    public GameObject HealthBar;
    Animator EnemyAnimation;
    Rigidbody rb;
    Vector3 drag;

    public bool bekle =false;
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

        if (this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("cut"))
        {
            if (Player.position.x < transform.position.x)
            {
                drag = new Vector3(-90, 0, 0).normalized;
                Quaternion rotation = Quaternion.LookRotation(drag);
                transform.rotation = rotation;
            }
            if (Player.position.x > transform.position.x)
            {
                drag = new Vector3(90, 0, 0).normalized;
                Quaternion rotation = Quaternion.LookRotation(drag);
                transform.rotation = rotation;
            }
        }
        if (Mathf.Abs(Player.position.x - transform.position.x) <4 && !bekle)
        {
            StartCoroutine(Vuruyongm());
        }

        if (EnemyCurrentHealth<=0)
        {
            HealthBar.GetComponent<Slider>().value = 0;
            GameObject.Find("AttackAndCam").GetComponent<BodyTarget>().TheOne = GameObject.Find("AttackAndCam");
            Player.GetComponent<walk>().TakeDamage(-70);
            Destroy(this.gameObject);
        }
        else
        {
            HealthBar.GetComponent<Slider>().value = EnemyCurrentHealth / EnemyMaxHealth;
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
    }
    IEnumerator Vuruyongm()
    {
        EnemyAnimation.SetTrigger("vur");
        bekle = true;
        yield return new WaitForSeconds(1.5f);
        bekle = false;
    }
    public void DealDamage()
    {
        if (Mathf.Abs(Player.position.x - transform.position.x) < 3)
        {
            Player.GetComponent<walk>().TakeDamage(30);
        }
    }
}
