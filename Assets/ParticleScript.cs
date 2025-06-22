using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    private void Awake()
    {
        ParticleSystem part = GetComponent<ParticleSystem>();
    }
}
