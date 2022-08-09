using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpecialEnemyThings : MonoBehaviour
{
    public Transform Player;
    public Transform StartTransform;
    Animator EnemyAnimation;
    Vector3 drag;
    public bool bekle = false;
    public bool StateFollow = false;
    public bool run = false;
    EnemyBehaviour enemyComponent;

    public bool anna = false;
    [Header("PatrolDecetion")]
    public bool emptyRoad=false;
    public bool wall=false;
    public bool jumpable = false;

    Rigidbody rb;
    public float Speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartTransform = transform;
    }
    void Start()
    {
        enemyComponent = GetComponent<EnemyBehaviour>();
        EnemyAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.forward);
        StateFollow = !enemyComponent.shouldWait;
        if (StateFollow)
        {
           EnemyAnimation.SetBool("run", true);
            if (this.EnemyAnimation.GetCurrentAnimatorStateInfo(0).IsName("Great Sword Run"))
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
                if (!emptyRoad )
                {
                    if (Mathf.Abs(Player.position.x - transform.position.x) > 2.5f)
                    {
                        rb.MovePosition(transform.position + transform.forward * Time.deltaTime * Speed);
                    }
                    if (Player.position.y - transform.position.y > 1f && jumpable)
                    {
                        rb.AddForce(Vector3.up);
                    }

                }
            }
            if (Mathf.Abs(Player.position.x - transform.position.x) < 3 && !bekle)
            {
                StartCoroutine(Vuruyongm());
            }
            if (Mathf.Abs(Player.position.x - transform.position.x) > 14)
            {
                enemyComponent.shouldWait = true;
            }

        }
        if (!StateFollow)
        {
                if (emptyRoad || wall)
                {
                    transform.eulerAngles= transform.eulerAngles + 180f * Vector3.up;
                }

            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * Speed);
        }
    }
    IEnumerator Vuruyongm()
    {
        if ( Mathf.Abs(Player.position.y - transform.position.y) < 1)
        {
            EnemyAnimation.SetTrigger("vur");
            bekle = true;
            yield return new WaitForSeconds(1.5f);
            bekle = false;
        }
    }
}
