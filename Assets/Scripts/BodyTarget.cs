using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTarget : MonoBehaviour
{
    Vector2 DamageRange;
    Vector3 kiki;
    public GameObject target;
    public GameObject TheOne;
    GameObject player;

    public float DamageEnchance = 1;

    Coroutine co;
    Animator adam;
    bool AttackStart = false;
    void Start()
    {
        player = GameObject.Find("Player");
        adam = player.GetComponent<Animator>();
        kiki = new Vector3(1000, 0, 0);
        co = StartCoroutine(PlaceHolder());
    }

    void Update()
    {
        target.transform.position = new Vector3(TheOne.transform.position.x, TheOne.transform.position.y + 1.6f, TheOne.transform.position.z);
        if (AttackStart)
        {
            adam.SetBool("Ataking", true);
        }
        if (!AttackStart)
        {
            adam.SetBool("Ataking", false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Vector3 thisfar = other.transform.position - transform.position;
            if (other.gameObject == TheOne || Mathf.Abs(thisfar.x) < Mathf.Abs(kiki.x))
            {
                kiki = thisfar;
                TheOne = other.gameObject;
                target.transform.position = TheOne.transform.position;
            }
            if (Mathf.Abs(thisfar.x) <= DamageRange.y)
            {
                GameObject.Find("Slash").GetComponent<AudioSource>().Play();
                other.GetComponent<EnemyBehaviour>().HealthGo(DamageRange.x*DamageEnchance);
                other.GetComponent<EnemyBehaviour>().OnAttack();
                TimeScaler(0.01f, 0.2f);
            }
            DamageRange = new Vector2(0, 0);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TheOne)
        {
            kiki = new Vector3(1000, 0, 0);
            TheOne = this.gameObject;
        }
    }
    public void Attack(Animator Character)
    {
        adam = Character;
        StopCoroutine(co);
        co = StartCoroutine(AttackControl());
    }
    IEnumerator AttackControl()
    {
        AttackStart = true;
        yield return new WaitForSeconds(0.5f);
        AttackStart = false;
    }
    IEnumerator PlaceHolder()
    {
        yield return new WaitForSeconds(1);
    }
    public void AttackStats(float range,float damage)
    {
        DamageRange = new Vector2(damage, range);
    }

    void TimeScaler(float timeScaling, float TimeSequance)
    {
        Time.timeScale = timeScaling;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        StartCoroutine(TimeWait(timeScaling, TimeSequance));
    }
    IEnumerator TimeWait(float scaling, float sequance)
    {
        yield return new WaitForSeconds(scaling * sequance);
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}

