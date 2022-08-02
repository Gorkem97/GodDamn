using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTarget : MonoBehaviour
{
    Vector2 DamageRange;
    GameObject player;

    public List<GameObject> enemyList = new List<GameObject>();
    public float DamageEnchance = 1;

    Coroutine co;
    Animator adam;
    bool AttackStart = false;
    void Start()
    {
        player = GameObject.Find("Player");
        co = StartCoroutine(PlaceHolder());
        adam = player.GetComponent<Animator>();
    }

    void Update()
    {
        if (AttackStart)
        {
            adam.SetBool("Ataking", true);
        }
        if (!AttackStart)
        {
            adam.SetBool("Ataking", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!enemyList.Contains(other.gameObject))
            {
                enemyList.Add(other.gameObject);
            }

        }


    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Enemy")
        {
            if (enemyList.Contains(other.gameObject))
            {
                enemyList.Remove(other.gameObject);
            }

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
        yield return new WaitForSeconds(0.2f);
        AttackStart = false;
    }
    IEnumerator PlaceHolder()
    {
        yield return new WaitForSeconds(1);
    }
    public void AttackStats(float range,float damage)
    {
        DamageRange = new Vector2(damage, range);
        int a = 0;
        foreach (var enemy in enemyList)
        {
            a += 1;
            Vector3 thisfar = enemy.transform.position - transform.position;
            if (Mathf.Abs(thisfar.x) <= DamageRange.y)
            {
                GameObject.Find("Slash").GetComponent<AudioSource>().Play();
                enemy.gameObject.GetComponent<EnemyBehaviour>().HealthGo(DamageRange.x * DamageEnchance);
                enemy.gameObject.GetComponent<EnemyBehaviour>().OnAttack();
                TimeScaler(0.04f, 0.1f);
            }
            if (a >= enemyList.Count)
            {
                DamageRange = new Vector2(0, 0);
            }
        }
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
    IEnumerator EndFrame()
    {
        yield return new WaitForEndOfFrame();
        DamageRange = new Vector2(0, 0);
    }
    void Allah()
    {
        foreach (GameObject emine in enemyList)
        {
        }

    }
}