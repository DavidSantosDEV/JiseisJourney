using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [SerializeField]
    Vector2 ImpulseForce;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player)
            {
                if (!player.GetIsGrounded())
                {
                    player.GetRigidBody()?.AddForce(ImpulseForce);
                }
            }
            //collision.gameObject.GetComponent<Rigidbody2D>()?.AddForce(ImpulseForce);
        }
    }
}
