using System.Collections;
using UnityEngine;

public class MiniPiece : Holdable
{
    public PieceData pieceData;
    private ParticleSystem _Particles;

    private void Start()
    {
        _Particles = GetComponentInChildren<ParticleSystem>();
    }

    public override void Hold(HoldController holder, Vector3 hitPoint)
    {
        base.Hold(holder, hitPoint);
        _Particles.Play();
    }

    public override void Release()
    {
        base.Release();
        _Particles.Stop();

    }

    public void Deactivate()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
