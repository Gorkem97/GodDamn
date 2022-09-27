using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class walk : MonoBehaviour
{
    public GameObject speedEffect;
    Rigidbody PlayerRb;
    public float Health = 100;
    public float MaxHealth = 100;

    [Header("AfterDeath")]
    public Vector3 BeforeTransform;
    public List<GameObject> enemies = new List<GameObject>();
    [Space(5)]


    [Header("Gravity and Jump")]
    public float JumpForce = 10;
    public bool isWalking;
    bool GravityPull;
    public float raycastDistance = 0.2f;
    bool waitingJump;
    int jumpWait =0;
    [Space(5)]

    [Header("CharacterMovement")]
    public float speed;
    float StartSpeed;
    public float acceleration;
    public float decceleration;
    public float velPower;
    public bool Cayeote = false;
    int allah = 0;
    float OriginalSpeed;
    public float smoothness = 60f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    Vector2 moveDirection = Vector2.zero;
    public bool AmIwalking = false;
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
    public GameObject atakCizgi;
    bool attackGo = false;
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
    public GameObject ParyEnemy;
    public bool haveparried = false;
    public bool onEnemy = false;
    [Space(5)]

    public float speedWait = 0;

    public GameObject bit;
    Quaternion rotationknow;
    void Start()
    {
        StartSpeed = speed;
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(item);
        }
        BeforeTransform = transform.position;
        CharacterAnimator = this.gameObject.GetComponent<Animator>();
        SlideIgnore = GameObject.Find("EmptyCollider");
        ParyParticle = GameObject.Find("ParryParticle");
        ParryLit = ParyParticle.transform.GetChild(0);
        OriginalSpeed = speed;
        Debug.Log(transform.forward);
        Debug.Log(Vector3.right);
    }
    private void Awake()
    {
        playerControls = new PlayerCommands();
        PlayerRb = this.gameObject.GetComponent<Rigidbody>();
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

        if (CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("roll") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("blok") || CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Great Sword Impact"))
        {
            hareketmi = false;
        }
        else
        {
            hareketmi = true;
        }
        if (CharacterAnimator.GetCurrentAnimatorStateInfo(1).IsName("UpperIdle"))
        {
            CharacterAnimator.SetLayerWeight(1, 0);
            atakCizgi.SetActive(false);
        }
        if (CharacterAnimator.GetCurrentAnimatorStateInfo(1).IsName("Attack1") || CharacterAnimator.GetCurrentAnimatorStateInfo(1).IsName("blok")|| CharacterAnimator.GetCurrentAnimatorStateInfo(1).IsName("Stabbing"))
        {
            atakCizgi.SetActive(true);
            CharacterAnimator.SetLayerWeight(1, 1);
        }

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
            GameObject.Find("HealthBar").GetComponent<Slider>().value = 1;
        }
        if (Health<=0)
        {
            Health = MaxHealth;
            GameObject.Find("HealthBar").GetComponent<Slider>().value = 0;
            this.gameObject.transform.position = BeforeTransform;
            foreach (GameObject item in enemies)
            {
                item.SetActive(true);
            }
        }
        else
        {
            GameObject.Find("HealthBar").GetComponent<Slider>().value = Health / MaxHealth;
        }
        if (PlayerRb.velocity.y<=0 && GravityPull)
        {
            StopUpwards();
            GravityPull = false;
        }
        RaycastHit hit;
        Ray GroundLay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(GroundLay, out hit, raycastDistance))
        {
            if (hit.collider.tag == "Ground" && !waitingJump)
            {
                Physics.gravity = new Vector3(0, -20F, 0);
                jumpWait = 0;
                isWalking = true;
                GravityPull = true;
                CharacterAnimator.SetBool("yerdemi", true);
            }
        }
        else
        {
                isWalking = false;
                CharacterAnimator.SetBool("yerdemi", false);
        }


    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (rolling)
            {
                SlideIgnore = collision.gameObject;
                Physics.IgnoreCollision(SlideIgnore.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
        if (collision.gameObject == ParyEnemy)
        {
            onEnemy = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CannonBall")
        {
            TakeDamage(40);
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Light")
        {
            Health += 60 * Time.deltaTime;
            BeforeTransform = transform.position;
        }
        if (other.gameObject.name == "PlayerDetection")
        {
            other.gameObject.GetComponentInParent<EnemyBehaviour>().shouldWait = false;
        }
    }
    private void FixedUpdate()
    {
        if (Cayeote && isWalking)
        {
            CharacterAnimator.SetTrigger("jump");
            GameObject.Find("Jump").GetComponent<AudioSource>().Play();
            CharacterAnimator.SetBool("yerdemi", false);
            PlayerRb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            //Cayeote = false;
        }
        Health -= 12 * Time.fixedDeltaTime;
        if (hareketmi)
        {
            int b = allah;
            moveDirection = move.ReadValue<Vector2>();
            if (moveDirection.x>0)
            {
                allah = 1;
                if (transform.forward == Vector3.left)
                {
                    Quaternion rotation = Quaternion.LookRotation(new Vector3(90, 0, 0).normalized);
                    transform.rotation = rotation;
                }
            }
            if (moveDirection.x < 0)
            {
                allah = -1;
                if (transform.forward == Vector3.right)
                {
                    Quaternion rotation = Quaternion.LookRotation(new Vector3(-90, 0, 0).normalized);
                    transform.rotation = rotation;
                }
            }


            if (moveDirection.x >= 0.8f)
            {
                speedWait += 1*Time.fixedDeltaTime;
            }
            if (moveDirection.x <= -0.8f)
            {
                speedWait += 1 * Time.fixedDeltaTime;
            }
            if(moveDirection.x > -0.8f && moveDirection.x < 0.8f)
            {
                speedWait = 0;
            }

            if (speedWait>7)
            {
                speedEffect.SetActive(true);
                speed = 1.3f*StartSpeed;
                CharacterAnimator.speed = 1.3f;
            }
            else
            {
                speedEffect.SetActive(false);
                speed = StartSpeed;
                CharacterAnimator.speed = 1f;
            }

            if (moveDirection.x == 0)
            {
                allah = 0;
            }
            if (Mathf.Abs(b) - Mathf.Abs(allah) == 2)
            {
                CharacterAnimator.SetTrigger("turn");
            }
            walkRoute = new Vector3(allah, 0);
            Vector3 direction = walkRoute.normalized;

            float targetSpeed = allah * speed;
            float speedDif = targetSpeed - PlayerRb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.1) ? acceleration : decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            PlayerRb.AddForce(movement * Vector3.right);
            if (direction.magnitude > 0)
            {
                AmIwalking = true;
                /*
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                */
                CharacterAnimator.SetBool("yuruyormu", true);
            }
            if (direction.magnitude == 0)
            {
                CharacterAnimator.SetBool("yuruyormu", false);
                AmIwalking = false;
            }
        }
        if (rolling)
        {
            PlayerRb.MovePosition(transform.position + transform.forward * Time.deltaTime * rollSpeed);
        }
        if (attackGo)
        {
            PlayerRb.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
            CharacterAnimator.SetBool("yuruyormu", true);
        }
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
        jump.started += Jump;
        jump.canceled += JumpEnd;
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
        StartCoroutine(JumpCheck());
    }
    private void JumpEnd(InputAction.CallbackContext context)
    {
        if (!isWalking)
        {
            StopUpwards();
        }
    }
    private void StopUpwards()
    {
        Physics.gravity = new Vector3(0, -40F, 0);
    }
    IEnumerator JumpCheck()
    {
        Cayeote = true;
        yield return new WaitForSeconds(0.3f);
        Cayeote = false;
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
        CharacterAnimator.speed = 1f;
        speed = OriginalSpeed;
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
        transform.rotation = rotationknow;
        if (SlideIgnore)
        {
            Physics.IgnoreCollision(SlideIgnore.GetComponent<Collider>(), GetComponent<Collider>(), false);
        }
    }
    public void AttackDamage(float a)
    {
        damage = a;
        GameObject.Find("Swoosh").GetComponent<AudioSource>().Play();
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
                speedWait = 0;
                if (!isBlocking)
                {
                    Health -= Nooo;
                    GameObject.Find("Slash").GetComponent<AudioSource>().Play();
                    //CharacterAnimator.SetTrigger("OuchStand");
                    GameObject.Find("Hurt").GetComponent<AudioSource>().Play();
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

        GameObject.Find("Seath").GetComponent<AudioSource>().Play();
        ParyParticle.GetComponent<ParticleSystem>().Play();
        CameraShake.Instance.ShakeCamera(4,1);
        ParryLit.GetComponent<Light>().intensity = 10;
        yield return new WaitForSeconds(1);
        ParryLit.GetComponent<Light>().intensity = 0;
    }
    public void parriedObject(GameObject Penemy)
    {
        ParyEnemy = Penemy;
        haveparried = true;
    }
    public void Stabbed()
    {
        onEnemy = false;
        Health = MaxHealth;
        GameObject.Find("Slash").GetComponent<AudioSource>().Play();
        ParyEnemy.GetComponent<EnemyBehaviour>().HealthGo(ParyEnemy.GetComponent<EnemyBehaviour>().EnemyMaxHealth);
    }
    public void CameraShaker()
    {
        GameObject.Find("Slash").GetComponent<AudioSource>().Play();
        CameraShake.Instance.ShakeCamera(10, 2);
    }
    public void FootStep()
    {
        if (isWalking)
        {
            GameObject.Find("Step").GetComponent<AudioSource>().Play();
        }
    }
    public void ShouldLeap()
    {
        if (!AmIwalking)
        {
            StartCoroutine(attackGoing());
        }

    }
    IEnumerator attackGoing()
    {
        attackGo = true;
        yield return new WaitForSeconds(0.2f);
        attackGo = false;
    }
}

