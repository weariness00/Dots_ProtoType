using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerState currentState;

    public Animator anim;
    public Rigidbody rigid;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        currentState = new IdleState();
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            ChangeState(new MovingState());
        }
        
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(new RollingState());
        }

        else
        {
            ChangeState(new IdleState());
        }
        
        currentState.UpdateState(this);
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void Idle()
    {
        rigid.velocity = Vector3.zero;
    }

    public void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            rigid.velocity = inputDir * speed;
            transform.LookAt(transform.position + inputDir);
        }
    }
}
