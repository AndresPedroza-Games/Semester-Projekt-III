using System.Collections;
using UnityEngine;

public class FusilSystem : MonoBehaviour
{
    [SerializeField] private float _CoolDown;

    private ParticleSystem _ParticleSystem;

    private bool _Active;

    private void Awake()
    {
        _ParticleSystem = GetComponentInChildren<ParticleSystem>();

        EventSystemHall4.instance.onReleaseBatery += () => _Active = true;
    }

    private void Update()
    {
        if (_Active)
        {
            _Active = false;
            StartCoroutine(RestartParticle());
        }
    }

    private IEnumerator RestartParticle()
    {
        yield return new WaitForSeconds(_CoolDown);
        _ParticleSystem.Play();
        _Active = true;
    }
}
