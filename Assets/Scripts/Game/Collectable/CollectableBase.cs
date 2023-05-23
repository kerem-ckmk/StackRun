using DG.Tweening;
using System;
using UnityEngine;

public abstract class CollectableBase : MonoBehaviour
{
    [Header("Collectable")]
    public ParticleSystem particleObject;
    public Transform visual;
    public Collider visualCollider;
    public bool IsInitialized { get; private set; }

    private Sequence _sequence;

    public event Action<int> OnAddCollectablePrice;

    public void Initialize()
    {
        RotationAnimation();
        IsInitialized = true;
    }

    public void TriggeredPlayer()
    {
        particleObject.transform.SetParent(null);
        particleObject.Play();

        int price = Price();
        OnAddCollectablePrice?.Invoke(price);

        int random = UnityEngine.Random.Range(0, 2);
        AudioClip clip = random == 0 ? GameConfigs.Instance.CollectableSounds[0] : GameConfigs.Instance.CollectableSounds[1];
        GameManager.PlaySound(clip);

        Destroy(gameObject);
    }

    public void RotationAnimation()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        _sequence.Append(visual.DORotate(new Vector3(0f, 360f, 0f), 3f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        _sequence.SetLoops(-1);
        _sequence.Play();
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
        _sequence = null;
        OnAddCollectablePrice = null;
    }

    private void Update()
    {
        if (!IsInitialized)
            return;

    }

    public abstract int Price();
    public abstract void OnRotateAnimation();
}
