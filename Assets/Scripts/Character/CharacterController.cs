using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Spriter2UnityDX;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    public Stats characterStats;
    private Vector2 lookDirectionVector = new Vector2(0, -1);
    private Vector2 moveVector = Vector2.zero;
    private InputActions inputActions;
    public int moveSpeed = 70;
    // public int maxHealth = 5;
    private float _currentHealth;
    public int maxInvinsibleTime = 2;
    private float _currentInvisibleTime;
    private bool isInvisible = false;
    public float timeBounceBack = 2;
    public float powerBounceBack = 20;
    private bool isAttacking = false;
    public static CharacterController instance;
    public float currentHealth
    {
        get { return _currentHealth; }
    }
    #region Singleton
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        inputActions = new InputActions();
        inputActions.Character.Enable();
        inputActions.Character.Direction.started += move;
        inputActions.Character.Direction.performed += move;
        inputActions.Character.Direction.canceled += stopMove;
        inputActions.Character.Attack.performed += attack;

        _currentHealth = characterStats.healthPoint;
        _currentInvisibleTime = maxInvinsibleTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (isInvisible)
        {
            coutdownInvisibleMode();
        }
    }
    void FixedUpdate()
    {
        animator.SetFloat("Speed", moveVector.magnitude);
        if (Mathf.Approximately(Mathf.Abs(lookDirectionVector.x), Mathf.Abs(lookDirectionVector.y)))
        {
            animator.SetFloat("Horizontal", Mathf.Sign(lookDirectionVector.x));
            animator.SetFloat("Vertical", Mathf.Sign(lookDirectionVector.y));
        }
        else
        {
            animator.SetFloat("Horizontal", lookDirectionVector.x);
            animator.SetFloat("Vertical", lookDirectionVector.y);
        }
        // if (Mathf.Abs(lookDirectionVector.x) >= Mathf.Abs(lookDirectionVector.y))
        // {
        //     animator.SetFloat("Horizontal", lookDirectionVector.x);
        //     animator.SetFloat("Vertical", 0);
        // }
        // else
        // {
        //     animator.SetFloat("Horizontal", 0);
        //     animator.SetFloat("Vertical", lookDirectionVector.y);
        // }

        // rb.MovePosition(new Vector2(transform.position.x + moveVector.x * moveSpeed * Time.deltaTime, transform.position.y + moveVector.y * moveSpeed * Time.deltaTime));
        rb.velocity = new Vector2(moveVector.x, moveVector.y).normalized * moveSpeed * Time.deltaTime;
    }
    private void move(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        if (!Mathf.Approximately(moveVector.x, 0.0f) || !Mathf.Approximately(moveVector.y, 0.0f))
        {
            lookDirectionVector.Set(moveVector.x, moveVector.y);
            lookDirectionVector.Normalize();
        }
    }
    private void stopMove(InputAction.CallbackContext context)
    {
        moveVector = Vector2.zero;
    }
    private void attack(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Attack");
    }
    private void changeHealth(int value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, characterStats.healthPoint);
        // if die
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Enemy" && other.otherCollider.gameObject.tag == "Player")
        {
            if (!isInvisible)
            {
                enterInvisibleMode();
                // changeHealth()
                StartCoroutine(animationGetHit(other.transform));
            }
        }
    }
    public IEnumerator animationGetHit(Transform hitByObject)
    {
        animator.SetTrigger("GetHit");
        float timeCur = 0;
        while (timeBounceBack > timeCur)
        {
            timeCur += Time.deltaTime;
            Vector2 vector2 = hitByObject.position - transform.position;
            vector2 = vector2.normalized;
            rb.AddForce(-vector2 * powerBounceBack);
        }
        yield return 0;
    }
    private void enterInvisibleMode()
    {
        isInvisible = true;
        _currentInvisibleTime = maxInvinsibleTime;
        changeOpacityCharacter(0.5f);
    }

    private void coutdownInvisibleMode()
    {
        if (_currentInvisibleTime > 0)
        {
            _currentInvisibleTime -= Time.deltaTime;
        }
        else if (_currentInvisibleTime <= 0)
        {
            isInvisible = false;
            changeOpacityCharacter(1f);
        }
    }

    private void changeOpacityCharacter(float value)
    {
        GetComponent<EntityRenderer>().Color = new Color(1, 1, 1, value);
    }
}