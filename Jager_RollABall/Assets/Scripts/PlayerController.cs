using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public InputActionMap player;
    public float jumpPower;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
        player.Enable();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }
    /// <summary>
    /// Function to perform the jump action.
    /// Recognizes input, adds upwards velocity
    /// </summary>
    /*void Jump()
    {
        if **is on floor**
        if (Input.GetButtonDown ("Jump"))
        {
            print("Hello!");
            rb.velocity = Vector2.up * jumpPower;
        }
    }
    */
    void OnJump()
    {
        Jump();
    }

    void Jump()
    {
        //if (onGround)
        //{ 
        rb.AddForce(Vector3.up * jumpPower);
        //}
    }

    private void Update()
    {
        //onGround = Physics.Raycast(transform.position, Vector3.down, .51f);

        if (transform.position.y < -10f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            speed = 100f;
        }
        else
        {
            speed = 50f;
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
        /*else if (other.gameObject.CompareTag("PUJump"))
            other.gameObject.SetActive(false);
            JumpPower = 5
            
        */   
    }
    
}
