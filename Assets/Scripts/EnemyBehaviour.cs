using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform Player;
    public Transform StartTransform;
    public GameObject bit;
    public GameObject EnemyUI;
    public float goBac = 100;
    public float EnemyCurrentHealth;
    public float EnemyMaxHealth = 100;
    public bool yokosunmu = false;
    public GameObject HealthBar;
    Animator EnemyAnimation;
    Vector3 drag;

    public bool StateFollow = false;
    public bool run = false;
    bool HaveBeenParried = false;
    NavMeshAgent EnemyAgent;
    

    public bool bekle =false;
    private void Awake()
    {
        EnemyAgent = GetComponent<NavMeshAgent>();
        StartTransform = transform;
    }
    void Start()
    {
        EnemyAnimation = GetComponent<Animator>();
        EnemyCurrentHealth = EnemyMaxHealth;
    }
    void Update()
    {

        if (yokosunmu)
        {
            StartCoroutine(Destiny());
        }
        EnemyUI.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
        HealthBar.GetComponent<Slider>().value = EnemyCurrentHealth / EnemyMaxHealth;
        if (StateFollow)
        {
            EnemyAnimation.SetBool("run", true);
            if (!this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("cut"))
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
                if (Mathf.Abs(Player.position.x - transform.position.x) > 2.5f)
                {
                    EnemyAgent.destination = Player.transform.position;
                }
            }
            if (Mathf.Abs(Player.position.x - transform.position.x) <= 2.5f)
            {
                EnemyAgent.destination = transform.position;
            }
            if (Mathf.Abs(Player.position.x - transform.position.x) < 4 && !bekle)
            {
                StartCoroutine(Vuruyongm());
            }

        }

        if (!StateFollow)
        {
            EnemyAgent.destination = StartTransform.position;
            transform.LookAt(StartTransform);
        }

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
        transform.position = transform.position + transform.forward*-2f;
    }
    public void HealthGo(float range)
    {
        EnemyCurrentHealth -= range;
        EnemyAnimation.SetTrigger("OuchStand");
        StateFollow = true;
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
            if (Player.GetComponent<walk>().isParry)
            {
                EnemyAnimation.SetBool("Parried",true);
                HaveBeenParried = true;
                StartCoroutine(parryWait());
                Player.GetComponent<walk>().parriedObject(this.gameObject);
            }
        }
    }
    IEnumerator Destiny()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
    public void Restart()
    {
        EnemyCurrentHealth = EnemyMaxHealth;
        EnemyAgent.enabled = !EnemyAgent.enabled;
        transform.position = StartTransform.position;
        EnemyAgent.enabled = !EnemyAgent.enabled;
        StateFollow = false;
    }
    IEnumerator parryWait()
    {
        yield return new WaitForSeconds(4);
        EnemyAnimation.SetBool("Parried", false);
        walk annen = Player.GetComponent<walk>();
        annen.onEnemy = false;
        annen.haveparried = false;
        annen.ParyEnemy = Player.gameObject;
    }
}
