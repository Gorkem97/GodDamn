using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class walk : MonoBehaviour
{
    CharacterController controller;
    public float Health = 100;

    [Header("Gravity and Jump")]
    public float GravityScale = 0.1f;
    public float GravityHolder;
    bool GravityPull = true;
    public float GravitationalSpeed = 5;
    public float JumpForce = 10;
    bool isWalking;
    float raycastDistance = 0.08f;
    bool waitingJump;
    [Space(5)]

    [Header("CharacterMovement")]
    public float speed;
    public float smoothness = 60f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    Vector2 moveDirection = Vector2.zero;
    Vector3 walkRoute;
    [Space(5)]

    [Header("Input System")]
    public PlayerCommands playerControls;
    private InputAction move;
    private InputAction fire;
    private InputAction jump;
    private InputAction slide;
    private InputAction block;
    [Space(5)]


    Animator CharacterAnimator;
    bool hareketmi = false;

    [Header("Attack")]
    float range=0;
    float damage=0;
    public BodyTarget AttackSequance;
    [Space(5)]

    [Header("Roll")]
    public float rollSpeed;
    public bool rolling;
    GameObject SlideIgnore;
    [Space(5)]


    [Header("Block/Parry")]
    public bool isBlocking= false;
    public bool isParry = false;
    GameObject ParyParticle;
    Transform ParryLit;
    [Space(5)]


    public GameObject bit;
    Quaternion rotationknow;
    void Start()
    {
        GravityHolder = GravityScale;
        CharacterAnimator = this.gameObject.GetComponent<Animator>();
        SlideIgnore = GameObject.Find("EmptyCollider");
        ParyParticle = GameObject.Find("ParryParticle");
        ParryLit = ParyParticle.transform.GetChild(0);
    }
    private void Awake()
    {
        playerControls = new PlayerCommands();
        controller = this.gameObject.GetComponent<CharacterController>();
        StartCoroutine(IdleController());
    }

    void Update()
    {
        if (!ParyParticle.GetComponent<ParticleSystem>().IsAlive())
        {
            ParyParticle.transform.position = transform.position + transform.forward + new Vector3(0, 1.6f, 0);
        }
        if (damage>0)
        {
            AttackSequance.AttackStats(range, damage);
            damage = 0;
            range = 0;
        }

        if (CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("draw") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") 
            || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("roll") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("blok") 
            || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Great Sword Impact"))
        {
            hareketmi = false;
        }
        else
        {
            hareketmi = true;
        }

        
        if (Health >= 100)
        {
            Health = 100;
            GameObject.Find("HealthBar").GetComponent<Slider>().value = 1;
        }
        if (Health<=0)
        {
            Health = 0;
            GameObject.Find("HealthBar").GetComponent<Slider>().value = 0;
            hareketmi = false;
            bit.SetActive(true);
        }
        else
        {
            GameObject.Find("HealthBar").GetComponent<Slider>().value = Health / 100;
        }
        if (GravitationalSpeed>=0 && GravityPull)
        {
            GravityScale = GravityScale*8/10;
            GravityPull = false;
        }


        RaycastHit hit;
        Ray GroundLay = new Ray(transform.position, Vector3.down);
        GravitationalSpeed += GravityScale;
        if (Physics.Raycast(GroundLay, out hit, raycastDistance))
        {
            if (hit.collider.tag == "Ground" && !waitingJump)
            {
                isWalking = true;
                GravityPull = true;
                CharacterAnimator.SetBool("yerdemi", true);
                GravitationalSpeed = 0.2f;
                GravityScale = GravityHolder;
            }
        }
        else
        {
            isWalking = false;
            CharacterAnimator.SetBool("yerdemi", false);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            if (rolling)
            {
                SlideIgnore = hit.gameObject;
                Physics.IgnoreCollision(SlideIgnore.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
        if (hit.gameObject.tag == "Light")
        {
            Health += 9;
        }

    }
    
    private void FixedUpdate()
    {

        Health -= 8 * Time.fixedDeltaTime;
        if (hareketmi)
        {
            moveDirection = move.ReadValue<Vector2>();
            walkRoute = new Vector3(moveDirection.x, 0, 0);
            Vector3 direction = walkRoute.normalized;
            if (direction.magnitude > 0)
            {
                controller.Move(walkRoute * speed * Time.fixedDeltaTime);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                CharacterAnimator.SetBool("yuruyormu", true);
            }
            if (direction.magnitude == 0)
            {
                CharacterAnimator.SetBool("yuruyormu", false);
            }
        }
        if (rolling)
        {
            controller.Move(transform.forward * rollSpeed * Time.fixedDeltaTime);
        }
        controller.Move(new Vector3(0, -1, 0) * GravitationalSpeed * Time.fixedDeltaTime);
    }

    IEnumerator IdleController()
    {
        int idleWait = Random.Range(5, 10);
        yield return new WaitForSeconds(idleWait);
        CharacterAnimator.SetTrigger("SwitchToIdleTwo");
        StartCoroutine(IdleController());
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        slide = playerControls.Player.Slide;
        slide.Enable();
        slide.performed += Slide;
        block = playerControls.Player.Block;
        block.Enable();
        block.performed += Block;
        block.canceled += BlockRelease;
    }
    private void OnDisable()
    {
        move = playerControls.Player.Move;
        move.Disable();
        fire.Disable();
        jump.Disable();
        block.Disable();
    }
    private void Fire(InputAction.CallbackContext context)
    {
        AttackSequance.Attack(CharacterAnimator);
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (isWalking)
        {
            CharacterAnimator.SetTrigger("jump");
            CharacterAnimator.SetBool("yerdemi", false);
            GravitationalSpeed = -JumpForce;
            controller.Move(new Vector3(0, 1, 0) * 0.3f);        }
    }
    private void Slide(InputAction.CallbackContext context)
    {
        if (CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            StartCoroutine(Sliding());
        }
    }
    private void Block(InputAction.CallbackContext context)
    {
        CharacterAnimator.SetBool("blocking", true);
    }
    private void BlockRelease(InputAction.CallbackContext context)
    {
        CharacterAnimator.SetBool("blocking", false);
        isBlocking = false;
        isParry = false;
    }
    IEnumerator Sliding()
    {
        rotationknow = transform.rotation;
        rolling = true;
        CharacterAnimator.SetBool("roll",true);
        yield return new WaitForSeconds(0.4f);
        CharacterAnimator.SetBool("roll", false);
        yield return new WaitForSeconds(0.4f);
        rolling = false;
        Physics.IgnoreCollision(SlideIgnore.GetComponent<Collider>(), GetComponent<Collider>(), false);
        transform.rotation = rotationknow;
    }
    public void AttackDamage(float a)
    {
        damage = a;
    }
    public void AttackRange(float a)
    {
        range = a;
    }
    public void TakeDamage(float Nooo)
    {
        if (!rolling)
        {
            if (!isParry)
            {
                if (!isBlocking)
                {
                    Health -= Nooo;
                    GameObject.Find("Slash").GetComponent<AudioSource>().Play();
                    //CharacterAnimator.SetTrigger("OuchStand");
                }
                else
                {
                    Health -= Nooo / 2;
                    GameObject.Find("Block").GetComponent<AudioSource>().Play();
                }
            }
            if (isParry)
            {
                StartCoroutine(ParryEffect());
            }

        }
    }
    public void BlockYeah()
    {
        isBlocking = true;
        isParry = true;
    }
    public void BlockNo()
    {
        isParry = false;
    }

    IEnumerator ParryEffect()
    {
        Health += 30;
        GameObject.Find("Parry").GetComponent<AudioSource>().Play();
        ParyParticle.GetComponent<ParticleSystem>().Play();
        ParryLit.GetComponent<Light>().intensity = 3;
        yield return new WaitForSeconds(1);
        ParryLit.GetComponent<Light>().intensity = 0;
    }
}

