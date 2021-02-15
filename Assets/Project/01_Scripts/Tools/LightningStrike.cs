using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] m_Particles;
    private SoundManager sd;

    void Start()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
        ps = GetComponent<ParticleSystem>();
    }

    /*private void Update()
    {
        if (ps.isEmitting)
        {
            ps.GetParticles(m_Particles);
            var strike = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //strike.GetComponent<Renderer>().enabled = false;
            strike.transform.position = m_Particles[0].position;
            sd.PlayLightningStrikeSound(strike);
        }
    }*/
}
