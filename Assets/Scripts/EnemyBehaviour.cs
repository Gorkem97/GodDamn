using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform Player;
    public GameObject EnemyUI;
    public float goBac = 100;
    public float EnemyCurrentHealth;
    public float EnemyMaxHealth = 100;
    public GameObject HealthBar;
    Animator EnemyAnimation;
    public bool shouldWait = true;
    [SerializeField]
    int sagSol = 0;

    public bool parriable;
    public float ParryBar;
    public float parryValue;
    Rigidbody rb;

    public bool bekle =false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EnemyAnimation = GetComponent<Animator>();
        EnemyCurrentHealth = EnemyMaxHealth;
    }
    void Update()
    {
        
        if (Player.position.x < transform.position.x)
        {
            sagSol = 2;
        }
        if (Player.position.x > transform.position.x)
        {
            sagSol = 1;
        }
        if (parryValue>0)
        {
            parryValue -= 0.5f*Time.deltaTime;
        }
        EnemyUI.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
        HealthBar.GetComponent<Slider>().value = EnemyCurrentHealth / EnemyMaxHealth;
        if (EnemyCurrentHealth<=0)
        {
            HealthBar.GetComponent<Slider>().value = 0;
            GameObject.Find("ALLAH").GetComponent<fÝNDaNDtERMÝNATE>().TheOne = GameObject.Find("ALLAH");
            GameObject.Find("AttackAndCam").GetComponent<BodyTarget>().enemyList.Remove(this.gameObject);
            Player.GetComponent<walk>().ParyEnemy = Player.gameObject;
            Player.GetComponent<walk>().onEnemy = false;
            this.gameObject.SetActive(false);
        }
        else
        {
            HealthBar.GetComponent<Slider>().value = EnemyCurrentHealth / EnemyMaxHealth;
        }
    }
    public void OnAttack()
    {
        if (this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("cut")  || !this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Parried"))
        {
            if (sagSol == 1)
            {
                rb.AddForce(transform.position + Vector3.left * goBac, ForceMode.Impulse);
                //rb.MovePosition(transform.position + Vector3.left * 2f);
                //transform.position = transform.position + Vector3.left * 2f;
            }
            if (sagSol == 2)
            {
                rb.AddForce(transform.position + Vector3.right * goBac, ForceMode.Impulse);
                //rb.MovePosition(transform.position + Vector3.right * 2f);
                //transform.position = transform.position + Vector3.right * 2f;
            }
        }
    }
    public void HealthGo(float range)
    {
        shouldWait = true;
        EnemyCurrentHealth -= range;
        CameraShake.Instance.ShakeCamera(2, 0.7f);
        EnemyAnimation.SetTrigger("OuchStand");
    }
    public void DealDamage(float damage)
    {
        if (Mathf.Abs(Player.position.x - transform.position.x) < 3)
        {
            Player.GetComponent<walk>().TakeDamage(damage);
            if (Player.GetComponent<walk>().isParry && parriable)
            {
                parryValue += 10;
                if (parryValue>=ParryBar)
                {
                    parryValue = 0;
                    EnemyAnimation.SetBool("Parried", true);
                    StartCoroutine(parryWait());
                    Player.GetComponent<walk>().parriedObject(this.gameObject);
                }
            }
        }
    }
    public void FootStep()
    {
        GameObject.Find("Step").GetComponent<AudioSource>().Play();
    }
    IEnumerator parryWait()
    {
        shouldWait = true;
        yield return new WaitForSeconds(4);
        shouldWait = false;
        EnemyAnimation.SetBool("Parried", false);
        walk annen = Player.GetComponent<walk>();
        annen.onEnemy = false;
        annen.haveparried = false;
        annen.ParyEnemy = Player.gameObject;
    }
}
