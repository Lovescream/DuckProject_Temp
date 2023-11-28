using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP : MonoBehaviour {

    public Vector2 Velocity { get; private set; }


    private float moveSpeed = 2.5f;
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator animator;

    void Awake() {
        rigid = this.GetComponent<Rigidbody2D>();
        spriter = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
    }

    void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Velocity = new Vector2(x, y).normalized * moveSpeed;

        rigid.MovePosition(rigid.position + Velocity * Time.fixedDeltaTime);

        if (Velocity.x != 0) spriter.flipX = Velocity.x < 0;

        animator.SetFloat("Speed", Velocity.magnitude);
        if (x != 0 || y != 0) {
            animator.SetFloat("X", x);
            animator.SetFloat("Y", y);
        }
    }

}