using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    /*[SerializeField] to see the variable in unity*/ 

    // Start variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //FSM
    private enum State {idle,running,jumping,falling};
    private State state = State.idle;

    //Inspector Variables
    [SerializeField] private LayerMask floor;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 7f;

    // Start is called before the first frame updateasas
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        AnimationState();
        anim.SetInteger("state", (int)state);//set animation based on Enumerator state
    }

    private void Movement()
    {
        float hdirection = Input.GetAxis("Horizontal");
        //Moving Left
        if (hdirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //Moving Rigth
        else if (hdirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //Stop Moving
        else if (hdirection == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        //Jump
        if (Input.GetButtonDown("Vertical") && coll.IsTouchingLayers(floor))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;
        }
    }

    private void AnimationState()
    {
        if (state ==State.jumping)
        {
            if (rb.velocity.y< .1f)
            {
                state = State.falling;
            }

        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(floor))
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x)>1f)
        {
            //Moving
            state = State.running;
        }

        else
        {
            state = State.idle;
        }

    }
}
