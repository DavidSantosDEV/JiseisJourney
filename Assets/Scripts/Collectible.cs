using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{

    public UnityEvent OnPickedUpEvent;

    protected virtual void OnPickedUp()
    {
        OnPickedUpEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPickedUp();
            Destroy(gameObject);
        }
    }
}
