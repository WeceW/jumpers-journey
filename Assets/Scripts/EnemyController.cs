using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 speed = new Vector2(2, 2);
    public Vector2 direction = new Vector2(-1, 0);
    
    private Vector2 movement;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity.x == 0)
        {
            direction.x = direction.x*-1;
            spriteRenderer.flipX = direction.x > 0;
        }

        movement = new Vector2(
            speed.x * direction.x,
            rigidbody.velocity.y);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = movement;
    }

}
