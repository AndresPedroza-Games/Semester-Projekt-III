using UnityEngine;

public class MiniPiece : Holdable
{
    public PieceData pieceData;
    private ParticleSystem _Particles;

    private void Start()
    {
        _Particles = GetComponentInChildren<ParticleSystem>();
    }

    public override void Hold(HoldController holder)
    {
        base.Hold(holder);
        _Particles.Play();
    }

    public override void Release()
    {
        base.Release();
        _Particles.Stop();

    }
}
