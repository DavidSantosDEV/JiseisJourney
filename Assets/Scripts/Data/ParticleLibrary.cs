using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParticleData", menuName = "ScriptableObjects/ParticleLibrary", order = 1)]
public class ParticleLibrary : ScriptableObject
{
    [Header("Particles")]
    [SerializeField] private Dictionary<LayerMask, GameObject> layersAndParticles = new Dictionary<LayerMask,GameObject>();

    [SerializeField]
    private GameObject defaultParticle;
    public GameObject GetPrefabForLayer(LayerMask layer)
    {
        if(layersAndParticles.Count>0)
        if(layersAndParticles.ContainsKey(layer)) return layersAndParticles[layer];
        return defaultParticle;
    }
}
