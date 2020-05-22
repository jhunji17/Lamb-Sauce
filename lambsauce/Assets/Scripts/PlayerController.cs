using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private enum State {idle, running, jumping, falling};
    private State state = State.idle;
    private Collider2D collider;
    [SerializeField]private LayerMask Ground;
    [SerializeField]private float speed = 5f;
    [SerializeField]private float jforce = 10f;

    public int cherries = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }
      

    private void Update()
    {
        handleInput();
        SetState();
        anim.SetInteger("State",(int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("one");
        if(collision.tag == "Collectible")
        {
            Debug.Log("two");
            Destroy(collision.gameObject);
            cherries += 1;
        }
    }



    private void handleInput(){
        float hdirection = Input.GetAxis("Horizontal");
        //Moving left
        if (hdirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }
        //Moving right
        else if (hdirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //Jumping
        if (Input.GetButtonDown("Jump") && collider.IsTouchingLayers(Ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jforce);
            state = State.jumping;
        }
    }

    private void SetState(){
        if(state == State.jumping){
            if(rb.velocity.y < 0.1f){
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if(collider.IsTouchingLayers(Ground))
            {
                state = State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > 2f){
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }


}
