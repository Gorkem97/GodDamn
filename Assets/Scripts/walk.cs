using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class walk : MonoBehaviour
{
    CharacterController controller;
    public BodyTarget AttackSequance;
    public float Health = 100;

    [Header("Gravity and Jump")]
    public float GravityScale;
    public float GravitationalSpeed;
    public float JumpForce;
    public bool isWalking;
    public float raycastDistance = 0.2f;
    bool waitingJump;
    [Space(5)]

    [Header("CharacterMovement")]
    public float speed;
    public float smoothness = 60f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [Space(5)]

    [Header("Input Systemt")]
    public PlayerCommands playerControls;
    private InputAction move;
    private InputAction fire;
    private InputAction jump;
    private InputAction slide;

    GameObject SlideIgnore;

    Animator CharacterAnimator;
    Vector2 moveDirection = Vector2.zero;
    Vector3 walkRoute;
    bool hareketmi = false;

    public float rollSpeed;
    bool rolling;
    void Start()
    {
        CharacterAnimator = this.gameObject.GetComponent<Animator>();
    }
    private void Awake()
    {
        playerControls = new PlayerCommands();
        controller = this.gameObject.GetComponent<CharacterController>();
        StartCoroutine(IdleController());
    }

    // Update is called once per frame
    void Update()
    {
        Health += 4 * Time.deltaTime;


        if (CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("draw") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") 
            || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("roll"))
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
        }
        if (Health<1)
        {
            Health = 0;
        }

        GameObject.Find("HealthBar").GetComponent<Slider>().value = Health / 100;

        RaycastHit hit;
        Ray GroundLay = new Ray(transform.position, Vector3.down);
        GravitationalSpeed += GravityScale;
        if (Physics.Raycast(GroundLay, out hit, raycastDistance))
        {
            if (hit.collider.tag == "Ground" && !waitingJump)
            {
                isWalking = true;
                CharacterAnimator.SetBool("yerdemi", true);
                GravitationalSpeed = 0.2f;
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
        if (rolling && hit.gameObject.tag == "Enemy")
        {
            SlideIgnore = hit.gameObject;
            Physics.IgnoreCollision(SlideIgnore.GetComponent<Collider>(), GetComponent<Collider>());
        }

    }
    private void FixedUpdate()
    {

        if (hareketmi)
        {
            moveDirection = move.ReadValue<Vector2>();
            walkRoute = new Vector3(moveDirection.x, 0, 0);

            Vector3 direction = walkRoute.normalized;
            if (direction.magnitude > 0)
            {
                controller.Move(walkRoute * speed * Time.deltaTime);
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
            controller.Move(transform.forward * rollSpeed * Time.deltaTime);
        }
        controller.Move(new Vector3(0, -1, 0) * GravitationalSpeed * Time.deltaTime);
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
    }
    private void OnDisable()
    {
        move = playerControls.Player.Move;
        move.Disable();
        fire.Disable();
        jump.Disable();
    }
    private void Fire(InputAction.CallbackContext context)
    {
        AttackSequance.Attack(CharacterAnimator);
        Health -= 10;
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (isWalking)
        {
            CharacterAnimator.SetTrigger("jump");
            CharacterAnimator.SetBool("yerdemi", false);
            GravitationalSpeed = -JumpForce;
            if (isWalking)
            {

                controller.Move(new Vector3(0, 1, 0) * 0.3f);
                Debug.Log("jump");
            }
        }
    }
    private void Slide(InputAction.CallbackContext context)
    {
        if (CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            StartCoroutine(Slide());
        }
    }
    IEnumerator Slide()
    {
        rolling = true;
        CharacterAnimator.SetBool("roll",true);
        yield return new WaitForSeconds(0.4f);
        CharacterAnimator.SetBool("roll", false);
        yield return new WaitForSeconds(0.4f);
        Physics.IgnoreCollision(SlideIgnore.GetComponent<Collider>(), GetComponent<Collider>(), false);
        rolling = false;
    }
    public void kDamageStart(float range, float damage)
    {
        AttackSequance.AttackStats(range,damage);
    }

}

