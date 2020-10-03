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
    public TextMeshProUGUI healthText;
    public GameObject winTextObject;
    public GameObject jumpTextObject;
    public InputActionMap player;
    public float jumpPower = 0;
    public Vector3 gravity;

    private Rigidbody rb;
    private int count;
    private int health;
    private float movementX;
    private float movementY;
    
    
    bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        health = 10;

        SetCountText();
        SetHealthText();
        winTextObject.SetActive(false);
        jumpTextObject.SetActive(false);
        player.Enable();

        Physics.gravity = gravity;
    }

    //Detects and collects movement data
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    //Updates count text with current data, displays win text
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 27)
        {
            winTextObject.SetActive(true);
        }
    }

    //Updates health text with current health
    void SetHealthText()
    {
        healthText.text = "Health: " + health.ToString();
        if (health < 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //Detects jump input, calls Jump function
    void OnJump()
    {
        Jump();
    }

    //Adds vertical force if the player is on a platform
    void Jump()
    {
        if (onGround)
        { 
        rb.AddForce(Vector3.up * jumpPower);
        }
    }

    /// <summary>
    /// Is called every frame
    /// Stores a variety of uses that need constant update
    /// </summary>
    private void Update()
    {
        //Detects if the player is on the ground
        onGround = Physics.Raycast(transform.position, Vector3.down, .51f);

        //If the player falls below the level, reloads the scene to reset the game
        if (transform.position.y < -10f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //Allows the player to spring by nearly doubling speed if LShift is used
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            speed = 130f;
        }
        else
        {
            speed = 70f;
        }
    }

    /// <summary>
    /// Is called last every frame
    /// Receives movement data and transfers data into Vector3 force according to current speed value
    /// </summary>
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    /// <summary>
    /// Detects collisions with physical static bodies
    /// Removes one from health when obstacles are collided with
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            health--;
            print("Health =" + health);
            SetHealthText();
        }
    }

    /// <summary>
    /// Detects collisions with trigger colliders
    /// If colliding with a regular pick up, adds one to Count 
    /// If colliding with the Jump pick up, adds jump power to the player allowing jumping
    ///     Activates the instruction text
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
        else if (other.gameObject.CompareTag("PUJump"))
        {
            other.gameObject.SetActive(false);
            jumpPower = 3000;
            jumpTextObject.SetActive(true);
        }  
    }
    
}
