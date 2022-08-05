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
    public GameObject HealthBar;
    Animator EnemyAnimation;
    Vector3 drag;

    public bool StateFollow = false;
    public bool run = false;
    NavMeshAgent EnemyAgent;
    [SerializeField]
    int sagSol = 0;
    

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
        if (Player.position.x < transform.position.x)
        {
            sagSol = 2;
        }
        if (Player.position.x > transform.position.x)
        {
            sagSol = 1;
        }

        EnemyUI.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
        HealthBar.GetComponent<Slider>().value = EnemyCurrentHealth / EnemyMaxHealth;
        if (StateFollow)
        {
            EnemyAnimation.SetBool("run", true);
            if (this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Great Sword Run"))
            {
                if (Player.position.x < transform.position.x)
                {
                    sagSol = 2;
                    drag = new Vector3(-90, 0, 0).normalized;
                    Quaternion rotation = Quaternion.LookRotation(drag);
                    transform.rotation = rotation;
                }
                if (Player.position.x > transform.position.x)
                {
                    sagSol = 1;
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
        if (!this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("cut") || !this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Impact") || !this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Parried"))
        {
            if (sagSol == 1)
            {
                transform.position = transform.position + Vector3.left * 2f;
            }
            if (sagSol == 2)
            {
                transform.position = transform.position + Vector3.right * 2f;
            }
        }
    }
    public void HealthGo(float range)
    {
        EnemyCurrentHealth -= range;
        CameraShake.Instance.ShakeCamera(2, 0.7f);
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
                StartCoroutine(parryWait());
                Player.GetComponent<walk>().parriedObject(this.gameObject);
            }
        }
    }
    public void Restart()
    {
        EnemyCurrentHealth = EnemyMaxHealth;
        EnemyAgent.enabled = !EnemyAgent.enabled;
        transform.position = StartTransform.position;
        EnemyAgent.enabled = !EnemyAgent.enabled;
        StateFollow = false;
    }
    public void FootStep()
    {
        GameObject.Find("Step").GetComponent<AudioSource>().Play();
    }
    IEnumerator parryWait()
    {
        StateFollow = false;
        yield return new WaitForSeconds(4);
        StateFollow = true;
        StateFollow = true;
        EnemyAnimation.SetBool("Parried", false);
        walk annen = Player.GetComponent<walk>();
        annen.onEnemy = false;
        annen.haveparried = false;
        annen.ParyEnemy = Player.gameObject;
    }
}
