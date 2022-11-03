using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleLightAdder : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Rendering.Universal.Light2D prefabLight;
    [SerializeField]
    private float lightmultiplier=1;

    [SerializeField]
    private bool hideParticles=false;

    public bool HideParticles
    {
        get => hideParticles;
        set => hideParticles=value;
    }

    public void StartEnableLights()
    {
        StartCoroutine(IncreaseLight());
    }

    [SerializeField][Range(0,1)]
    float intensity = 1;
    [SerializeField]
    private float speed=1;
    private IEnumerator IncreaseLight()
    {
        while (intensity < 1)
        {
            intensity = Mathf.Clamp01(intensity += Time.deltaTime * speed);
            yield return null;
        }
        intensity = 1;
    }

    struct LightAndComponent
    {
        public GameObject obj;
        public UnityEngine.Rendering.Universal.Light2D component;

        public LightAndComponent(GameObject obje,UnityEngine.Rendering.Universal.Light2D lightc)
        {
            obj = obje;
            component = lightc;
        }
    }

    private  List<LightAndComponent> LightsAndComponents= new List<LightAndComponent>();

    private ParticleSystem m_ParticleSystem;
    private ParticleSystem.Particle[] m_Particles;

    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
        InstantiateParticles(m_ParticleSystem.GetParticles(m_Particles));
        //InstantiateParticles(m_ParticleSystem.main.maxParticles); //Using the max particles is a mistake, Unity may say its max x but it would just instantiate a bunch with no use
    }


    private void InstantiateParticles(int count)
    {
        for (int i=0;i< count; i++)
        {
            GameObject newObject = Instantiate(prefabLight.gameObject, transform);
            UnityEngine.Rendering.Universal.Light2D newLight = newObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            LightAndComponent add = new LightAndComponent(newObject, newLight);
            LightsAndComponents.Add(add);
        }
    }
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);
        //bool worldSpace = ();

        //if (count > LightsAndComponents.Count) InstantiateParticles(count - LightsAndComponents.Count);

        for (int i = 0; i < LightsAndComponents.Count; i++)
        {
            if (i < count)
            {
                if (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World)
                {
                    LightsAndComponents[i].obj.transform.position = m_Particles[i].position;
                }
                else
                {
                    LightsAndComponents[i].obj.transform.localPosition = m_Particles[i].position;
                }   

                LightsAndComponents[i].obj.SetActive(!hideParticles);
                LightsAndComponents[i].component.intensity = intensity * (  lightmultiplier * Mathf.Clamp(m_Particles[i].GetCurrentSize(m_ParticleSystem), 0, 1));
                LightsAndComponents[i].component.color = m_Particles[i].GetCurrentColor(m_ParticleSystem);


            }
            else
            {
                LightsAndComponents[i].obj.SetActive(false);
            }
        }
    }
}
