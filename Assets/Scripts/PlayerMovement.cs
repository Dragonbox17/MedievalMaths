using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [SerializeField] float speed;
    public Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveX, moveY, 0).normalized;
    }

    private void FixedUpdate()
    {
        
        if (movement != Vector3.zero)
        {
            GetComponent<SpriteRenderer>().sortingOrder = -Mathf.FloorToInt(transform.position.y) - 16;
            rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y, 0) + (movement * speed * Time.fixedDeltaTime));
            
        }
    }
}
