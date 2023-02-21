using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BackgroundMouvment : MonoBehaviour
{
    // Camera
    private Camera mainCamera;

    // Coroutine
    [SerializeField] private float beginCoroutineDuration;
    [SerializeField] private float beginCoroutineTime;
    [SerializeField] private float beginCoroutineCameraTime;
    [SerializeField] private Vector3 beginCoroutineCameraDestination;
    private Vector3 beginCoroutineCameraPosition;
    private bool beginCoroutineEnd;
    private bool beginCoroutineState;
    private Coroutine beginCoroutine;
    private int beginCoroutineStates;

    // Backgrounds
    [SerializeField] private GameObject backgrounds2Container;
    [SerializeField] private Transform[] backgrounds1;
    [SerializeField] private Transform[] backgrounds2;
    private int backgroundsLength;
    private float backgroundsSizex;

    // Movements
    [FormerlySerializedAs("m_SpeedSlow")] [SerializeField] private float SpeedSlow;
    [FormerlySerializedAs("m_SpeedFast")] [SerializeField] private float SpeedFast;
    private float m_SpeedMultiplier;
    private bool m_IsBlocked;
    private bool m_BlockedRight;
    

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
    
    // Level Cubes
    [SerializeField] private Transform[] m_LevelCubes;
    [SerializeField] private float m_MoveSpeed;
    
    void Start()
    {
        mainCamera = Camera.main;
        // Coroutine
        beginCoroutineCameraPosition = mainCamera.transform.position;
        beginCoroutineEnd = false;
        beginCoroutineState = true;
        beginCoroutineStates = 0;

        // Serialized
        backgroundsSizex = backgrounds1[0].GetComponent<SpriteRenderer>().bounds.size.x;
        backgroundsLength = backgrounds1.Length;
        m_SpeedMultiplier = SpeedSlow;

        // Components
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();

        // Inputs
        m_LeftInput = false;
        m_RightInput = false;
        m_LeftShift = false;
        m_Jump = false;

        m_IsBlocked = false;

        m_JumpCount = 0;
    }


    void Update()
    {
        if (beginCoroutineEnd)
        {
            Inputs();
            CollisionDetector();
            BackGroundParallax(m_LeftInput, m_RightInput);
            GroundMovement(m_LeftInput, m_RightInput, m_LeftShift);
            Animate(m_LeftInput, m_RightInput, m_LeftShift);
        }
        else
        {
            if (beginCoroutineState)
            {
                beginCoroutine = StartCoroutine(AtStartCoroutine());
                beginCoroutineState = false;
            }
            else if (beginCoroutineStates == 3)
            {
                StopCoroutine(beginCoroutine);
                beginCoroutineEnd = true;
            }
        }
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

            // Going Left
            if (b1x > 0 && b2x >= b1x)
            {
                Vector3 newPosition = backgrounds2[i].position;
                newPosition.x -= backgroundsSizex;
                backgrounds2[i].position = newPosition;
            }
            
            // Going right
            if (b2x > 0 && b1x > b2x)
            {
                Vector3 newPosition = backgrounds1[i].position;
                newPosition.x -= backgroundsSizex;
                backgrounds1[i].position = newPosition;
            }

            // Parallax Mouvment
            if (left)
            {
                backgrounds1[i].Translate(Vector3.right * (((i + 1)) * m_SpeedMultiplier) *
                                          Time.deltaTime);
                backgrounds2[i].Translate(Vector3.right * (((i + 1)) * m_SpeedMultiplier) *
                                          Time.deltaTime);
            }

            if (right)
            {
                backgrounds1[i].Translate(Vector3.left * (((i + 1)) * m_SpeedMultiplier) *
                                          Time.deltaTime);
                backgrounds2[i].Translate(Vector3.left * (((i + 1)) * m_SpeedMultiplier) *
                                          Time.deltaTime);
            }
        }
    }

    private void Animate(bool left, bool right, bool shift)
    {
        if (shift)
        {
            m_Animator.SetBool("Run", true);
            m_SpeedMultiplier = SpeedFast;
        }
        else
        {
            m_Animator.SetBool("Run", false);
            m_SpeedMultiplier = SpeedSlow;
        }

        if (left || right)
        {
            if (!backgrounds2Container.activeInHierarchy)
            {
                backgrounds2Container.SetActive(true);
            }

            m_Animator.SetBool("Walk", true);
        }
        else
        {
            m_Animator.SetBool("Walk", false);
        }

        if (left)
        {
            m_SpriteRenderer.flipX = true;
        }

        if (right)
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


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 8)
        {
            m_IsBlocked = true;
            if (m_RightInput)
            {
                m_BlockedRight = true;
            }

            if (m_LeftInput)
            {
                m_BlockedRight = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            m_IsBlocked = false;
        }
    }

    private void CollisionDetector()
    {
        if (m_RightInput && m_LeftInput)
        {
            m_RightInput = false;
            m_LeftInput = false;
        }
        if (m_IsBlocked)
        {
            if (m_BlockedRight)
            {
                m_RightInput = false;
            }
    
            if (!m_BlockedRight)
            {
                m_LeftInput = false;
            }
        }
    }
    

    private IEnumerator AtStartCoroutine()
    {
        while (true)
        {
            beginCoroutineTime += Time.deltaTime;
            if (beginCoroutineTime > 1)
            {
                beginCoroutineStates++;
                beginCoroutineTime = 0;
            }

            if (beginCoroutineStates == 1)
            {
                mainCamera.transform.position = beginCoroutineCameraPosition +
                                                (beginCoroutineCameraDestination - beginCoroutineCameraPosition) *
                                                beginCoroutineTime;
            }
            else if (beginCoroutineStates == 2)
            {
                foreach (Transform mLevelCube in m_LevelCubes)
                {
                    Color currentColor = mLevelCube.GetComponent<SpriteRenderer>().color;
                    currentColor.a = 1 * beginCoroutineTime;
                    mLevelCube.GetComponent<SpriteRenderer>().color = currentColor;
                }
            }
            
            yield return null;
        }
    }

    private void GroundMovement(bool left, bool right, bool shift)
    {
        if (left)
        {
            foreach (Transform mLevelCube in m_LevelCubes)
            {
                mLevelCube.Translate(Vector3.right * (m_SpeedMultiplier * m_MoveSpeed) * Time.deltaTime);
            }
        }
        else if (right)
        {
            foreach (Transform mLevelCube in m_LevelCubes)
            {
                mLevelCube.Translate(Vector3.left * (m_SpeedMultiplier * m_MoveSpeed) * Time.deltaTime);
            }
        }
    }
}