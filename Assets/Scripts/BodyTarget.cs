using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTarget : MonoBehaviour
{
    public Vector2[] attackDamageAndRange;
    Vector2 initiatorDamageRange;
    float intentionalAttackRange = 0;
    float intentionalAttackDamage = 0;
    Vector3 kiki;
    public GameObject target;
    GameObject TheOne;
    GameObject player;

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
        if (this.adam.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            initiatorDamageRange = attackDamageAndRange[1];
        }
        if (this.adam.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            initiatorDamageRange = attackDamageAndRange[2];

        }
        if (this.adam.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            initiatorDamageRange = attackDamageAndRange[3];

        }
        if (this.adam.GetCurrentAnimatorStateInfo(0).IsName("idle") || this.adam.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            initiatorDamageRange = attackDamageAndRange[0];
        }
        intentionalAttackDamage = initiatorDamageRange.x;
        intentionalAttackRange = initiatorDamageRange.y;
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
            if (Mathf.Abs(thisfar.x) <= intentionalAttackRange)
            {
                other.GetComponent<EnemyBehaviour>().HealthGo(intentionalAttackDamage);
            }
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

    }
}

