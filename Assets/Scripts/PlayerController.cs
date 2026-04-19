using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Rotation")]
    public float rotateDuration = 0.12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isUpsideDown = false;

    private bool isRotating = false;
    private float rotationStartZ;
    private float rotationTargetZ;
    private float rotationTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
        {
            Jump();
            StartRotate90Right();
        }

        UpdateRotation();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (!isUpsideDown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpForce);
        }
    }

    void StartRotate90Right()
    {
        if (isRotating) return;

        isRotating = true;
        rotationTime = 0f;
        rotationStartZ = transform.eulerAngles.z;
        rotationTargetZ = rotationStartZ - 90f;
    }

    void UpdateRotation()
    {
        if (!isRotating) return;

        rotationTime += Time.deltaTime;
        float t = rotationTime / rotateDuration;

        if (t >= 1f)
        {
            t = 1f;
            isRotating = false;
        }

        float z = Mathf.LerpAngle(rotationStartZ, rotationTargetZ, t);
        transform.rotation = Quaternion.Euler(0f, 0f, z);
    }

    public void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;
        rb.gravityScale *= -1f;

        Vector3 scale = transform.localScale;
        scale.y *= -1f;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            RestartLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;

            if (currentScene + 1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(currentScene + 1);
            }
            else
            {
                FindObjectOfType<WinUI>().ShowWin();
            }
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}