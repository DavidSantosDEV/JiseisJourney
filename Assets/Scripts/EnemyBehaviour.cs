using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody2D rbody;

    public float enemySpeed;

    public float traceSizeGround = 1;
    public float traceSizeFront = 1;

    public Transform traceGroundPlace;
    public Transform traceFrontPlace;
    public Transform traceUnder;

    public LayerMask TraceLayers;

    bool bIsFlipped = false;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HasTrace(traceUnder, Vector2.down, traceSizeGround))
        {
            if (HasTrace(traceGroundPlace, Vector2.down, traceSizeGround))
            {
                if (HasTrace(traceFrontPlace, (bIsFlipped ? Vector2.left : Vector2.right), traceSizeFront))
                {
                    FlipEnemy();
                }
            }
            else
            {
                FlipEnemy();
            }
        }
        Vector2 val = new Vector2((bIsFlipped ? -1 : 1) * enemySpeed, rbody.velocity.y);
        rbody.velocity = val;
    }

    bool HasTrace(Transform Start, Vector2 direction, float lenght)
    {
        Debug.DrawRay(Start.position, direction * lenght, Color.blue);
        return Physics2D.Raycast(Start.position, direction, lenght, TraceLayers);
    }

    void FlipEnemy()
    {
        gameObject.transform.localScale = bIsFlipped ? Vector3.one : new Vector3(-1, 1, 1);

        bIsFlipped =!bIsFlipped;
    }
}
