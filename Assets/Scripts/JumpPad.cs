using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [SerializeField]
    Vector2 ImpulseForce;

    Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player)
            {
                if (!player.GetIsGrounded())
                {
                    Rigidbody2D rb = player.GetRigidBody();
                    if (rb)
                    {
                        player.ChangeState(PlayerStates.Falling);
                        if (_anim)
                        {
                            _anim.SetTrigger("Jump");
                        }
                        rb.AddForce(ImpulseForce);
                    }
                    

                }
            }
            //collision.gameObject.GetComponent<Rigidbody2D>()?.AddForce(ImpulseForce);
        }
    }
}
