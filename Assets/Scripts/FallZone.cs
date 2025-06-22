using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Transform pos = GameManager.Instance.GetCurrentCheckPoint();
            collision.gameObject.transform.SetPositionAndRotation(pos.position, collision.gameObject.transform.rotation);
        }
    }
}
