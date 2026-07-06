using UnityEngine;

public enum ParticlesType
{
    End,
    Astralo,
    Chaser,
    ScalarUI,
    ScalarUICurrunt,
    chesed,
    TanZeta,
    Warden
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public GameObject[] particlePrefabs; // 여러 개의 파티클 프리팹 배열
    public GameObject[] activeParticles; // 현재 활성화된 파티클 객체 배열

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        activeParticles = new GameObject[particlePrefabs.Length];
    }

    public void PlayParticle(ParticlesType particleType)
    {
        int index = (int)particleType;

        // 파티클이 존재하지 않으면 새로 생성
        activeParticles[index] = Instantiate(particlePrefabs[index], transform.position, Quaternion.Euler(-90, 0, 0));
        activeParticles[index].transform.SetParent(transform);

        //activeParticles[index].transform.position = position;
        //activeParticles[index].transform.rotation = Quaternion.Euler(-90, 0, 0);
        ParticleSystem ps = activeParticles[index].GetComponent<ParticleSystem>();
        ps.Play();


    }

    public void StopParticle(ParticlesType particleType)
    {
        int index = (int)particleType;

        ParticleSystem ps = activeParticles[index].GetComponent<ParticleSystem>();
        if (ps != null && ps.isPlaying)
        {
            ps.Stop();
        }
    }

    void Update()
    {
        foreach (var particle in activeParticles)
        {
            if (particle != null)
            {
                particle.transform.position = GameManager.Instance.Player.transform.position;
            }

        }
    }
}
