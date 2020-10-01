using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public InputActionMap player;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    [Range(1, 10)]
    public float jumpPower;

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
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
        //Jump();
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
