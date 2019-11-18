using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    /*[SerializeField] to see the variable in unity*/ private Rigidbody2D rb;
    /*[SerializeField]*/ private Animator anim;
    private enum State {idle,running,jumping};
    private State state = State.idle;
    // Start is called before the first frame updateasas
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float hdirection = Input.GetAxis("Horizontal");

        if (hdirection<0)
        {
            rb.velocity = new Vector2(-3, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hdirection>0)
        {
            rb.velocity = new Vector2(3, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else if (hdirection==0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, 7f);
            state = State.jumping;
        }

        VelocityState();
        anim.SetInteger("state",(int)state);
    }

    private void VelocityState()
    {
        if (state ==State.jumping){

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
