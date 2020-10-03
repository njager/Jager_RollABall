﻿using System.Collections;
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
    private float movementX;
    private float movementY;
    private int health;
    
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
    void SetHealthText()
    {
        healthText.text = "Health: " + health.ToString();
        if (health < 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    /// <summary>
    /// Function to perform the jump action.
    /// Recognizes input, adds upwards velocity
    /// </summary>
    void OnJump()
    {
        Jump();
    }

    void Jump()
    {
        if (onGround)
        { 
        rb.AddForce(Vector3.up * jumpPower);
        }
    }

    private void Update()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, .51f);

        if (transform.position.y < -10f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            speed = 130f;
        }
        else
        {
            speed = 70f;
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            health--;
            print("Health =" + health);
            SetHealthText();
        }
    }

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
