using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SwordAttack : MonoBehaviour
{
    public bool AttackStart = false;
    public bool DamageControl = false;
    public float attackdelay;
    public int HowManyAttacks =0;
    Quaternion rotationknow;
    GameObject player;
    Animator adam;
    Coroutine co;
    void Start()
    {
        player = GameObject.Find("Player");
        adam = player.GetComponent<Animator>();
        co = StartCoroutine(PlaceHolder());
    }

    void Update()
    {

        if (AttackStart)
        {
            adam.SetBool("Ataking",true);
        }
        if (!AttackStart)
        {
            adam.SetBool("Ataking", false);
        }
        if (this.adam.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && HowManyAttacks < 1)
        {
            rotationknow = player.transform.rotation;
            DamageControl = false;
            HowManyAttacks += 1;
            StartCoroutine(Damaging(attackdelay,0.25f));
        }
        if (this.adam.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && HowManyAttacks < 2)
        {
            DamageControl = false;
            HowManyAttacks += 1;
            StartCoroutine(Damaging(attackdelay, 0.25f));
        }
        if (this.adam.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && HowManyAttacks < 3)
        {
            DamageControl = false;
            HowManyAttacks += 1;
            StartCoroutine(Damaging(attackdelay, 1.3f));
        }
        if (this.adam.GetCurrentAnimatorStateInfo(0).IsName("idle") || this.adam.GetCurrentAnimatorStateInfo(0).IsName("walk") )
        {
            if (HowManyAttacks>0)
            {
                player.transform.rotation = rotationknow;
                HowManyAttacks = 0;
                DamageControl = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && DamageControl)
        {
            other.gameObject.GetComponent<EnemyBehaviour>().OnAttack();
            GameObject.Find("Slash").GetComponent<AudioSource>().Play();
            TimeScaler(0.01f, 0.2f);
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

    void TimeScaler(float timeScaling,float TimeSequance)
    {
        Time.timeScale = timeScaling;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        StartCoroutine(TimeWait(timeScaling,TimeSequance));
    }
    IEnumerator TimeWait(float scaling, float sequance)
    {
        yield return new WaitForSeconds(scaling*sequance);
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    IEnumerator Damaging(float waittime, float endtime)
    {
        yield return new WaitForSeconds(waittime);
        DamageControl = true;
        yield return new WaitForSeconds(endtime);
        DamageControl = false;
    }
}
