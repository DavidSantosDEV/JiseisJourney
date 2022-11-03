using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private float lenght;
    Vector2 startpos;
    public GameObject Camera;
    public float paralaxStrenght = 0;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float temp = Camera.transform.position.x * (1 - paralaxStrenght);
        float dist = Camera.transform.position.x * paralaxStrenght;

        transform.position = new Vector3(startpos.x + dist, startpos.y);
        transform.position = transform.position + offset;

        if (temp > startpos.x + lenght) startpos.x += lenght;
        else if (temp < startpos.x - lenght) startpos.x -= lenght;
    }
}
