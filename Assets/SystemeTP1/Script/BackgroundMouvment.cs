using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMouvment : MonoBehaviour
{
    // Backgrounds
    [SerializeField] private Transform[] backgrounds1;
    [SerializeField] private Transform[] backgrounds2;
    private int backgroundsLength;
    private float backgroundsSizex;

    // Speeds
    [SerializeField] private float m_SpeedSlow;
    [SerializeField] private float m_SpeedFast;
    private float m_SpeedMultiplier;

    // Physics
    [SerializeField] private float JumpForce;

    // Components
    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;

    // Inputs
    private bool m_LeftInput;
    private bool m_RightInput;
    private bool m_LeftShift;
    private bool m_Jump;


    private int m_JumpCount;

    void Start()
    {
        // Serialized
        backgroundsSizex = backgrounds1[0].GetComponent<SpriteRenderer>().bounds.size.x;
        backgroundsLength = backgrounds1.Length;
        m_SpeedMultiplier = m_SpeedSlow;

        // Components
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();

        // Inputs
        m_LeftInput = false;
        m_RightInput = false;
        m_LeftShift = false;
        m_Jump = false;

        m_JumpCount = 0;
    }


    void Update()
    {
        Inputs();
        BackGroundParallax(m_LeftInput, m_RightInput);
        Animate(m_LeftInput, m_RightInput, m_LeftShift);
    }

    private void FixedUpdate()
    {
        Jump();
    }

    private void Inputs()
    {
        m_LeftInput = Input.GetKey(KeyCode.A); // A
        m_RightInput = Input.GetKey(KeyCode.D); // D
        m_LeftShift = Input.GetKey(KeyCode.LeftShift); // LeftShift
        if (Input.GetKeyDown(KeyCode.Space) && m_JumpCount < 1) // Space
        {
            m_Jump = true;
        }
    }

    private void BackGroundParallax(bool left, bool right)
    {
        for (int i = 0; i < backgroundsLength; i++)
        {
            float b1x = backgrounds1[i].position.x;
            float b2x = backgrounds2[i].position.x;

            // Making it Infinite
            // Going Right
            if (b1x < 0 && b2x <= b1x)
            {
                Vector3 newPosition = backgrounds2[i].position;
                newPosition.x += backgroundsSizex;
                backgrounds2[i].position = newPosition;
            }
            if (b2x < 0 && b1x < b2x)
            {
                Vector3 newPosition = backgrounds1[i].position;
                newPosition.x += backgroundsSizex;
                backgrounds1[i].position = newPosition;
            }
            //// Going Left
            if (b1x > 0 && b2x >= b1x)
            {
                Vector3 newPosition = backgrounds2[i].position;
                newPosition.x -= backgroundsSizex;
                backgrounds2[i].position = newPosition;
            }
            if (b2x > 0 && b1x > b2x)
            {
                Vector3 newPosition = backgrounds1[i].position;
                newPosition.x -= backgroundsSizex;
                backgrounds1[i].position = newPosition;
            }

            // Parallax Mouvment
            if (left)
            {
                backgrounds1[i].Translate(Vector3.right * (((backgroundsLength + 1) - i) * m_SpeedMultiplier) * Time.deltaTime);
                backgrounds2[i].Translate(Vector3.right * (((backgrounds1.Length + 1) - i) * m_SpeedMultiplier) * Time.deltaTime);
            }
            if (right)
            {
                backgrounds1[i].Translate(Vector3.left * (((backgroundsLength + 1) - i) * m_SpeedMultiplier) * Time.deltaTime);
                backgrounds2[i].Translate(Vector3.left * (((backgroundsLength + 1) - i) * m_SpeedMultiplier) * Time.deltaTime);
            }
        }
    }

    private void Animate(bool left, bool right, bool shift)
    {
        if (shift)
        {
            m_Animator.SetBool("Run", true);
            m_SpeedMultiplier = m_SpeedFast;
        }
        else
        {
            m_Animator.SetBool("Run", false);
            m_SpeedMultiplier = m_SpeedSlow;
        }
        if (left || right)
        {
            m_Animator.SetBool("Walk", true);
        }
        else
        {
            m_Animator.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            m_SpriteRenderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_SpriteRenderer.flipX = false;
        }
    }

    private void Jump()
    {
        if (m_Jump)
        {
            m_Rigidbody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            m_Animator.SetTrigger("Jump");
            m_Jump = false;
            m_JumpCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            m_JumpCount = 0;
        }
    }
}
