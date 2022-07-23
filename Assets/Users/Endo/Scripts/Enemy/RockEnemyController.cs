using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class RockEnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private MeshRenderer[] fractured;

    [SerializeField, Header("死亡 (割れた) 後にフェードアウトするまでの時間 (秒)")]
    private float waitTimeAfterDead;

    [SerializeField, Header("フェードアウト時間 (秒)")]
    private float fadeOutTime;

    private MeshFilter   _selfMeshFilter;
    private MeshRenderer _selfRenderer;
    private Collider     _selfCollider;

    private readonly MessageBroker _broker = new();

    private CancellationToken _destroyCancellationToken;

    public HP Hp { get; private set; }

    private void Start()
    {
        _selfMeshFilter = GetComponent<MeshFilter>();
        _selfRenderer   = GetComponent<MeshRenderer>();
        _selfCollider   = GetComponent<Collider>();

        _destroyCancellationToken = this.GetCancellationTokenOnDestroy();

        foreach (MeshRenderer shard in fractured)
        {
            shard.gameObject.SetActive(false);
        }

        Hp = new HP(1, 1, _broker);

        _broker.Receive<OnStatusDestroy>()
               .Subscribe(_ => OnDead())
               .AddTo(this);
    }

    public void OnDamage(AttackPower attackPower)
    {
        Hp.Damage(attackPower);
        RockMissionManager.Instance.CountUp();
    }

    private async void OnDead()
    {
        Destroy(_selfMeshFilter);
        Destroy(_selfRenderer);
        _selfCollider.enabled = false;

        foreach (MeshRenderer shard in fractured)
        {
            shard.gameObject.SetActive(true);
            shard.enabled = true;
        }

        // プレイヤーの張り付き解除
        // TODO: 別の場所に移動
        PlayerActionManager.Instance.WeaponThrowing.ResetSword();

        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTimeAfterDead), cancellationToken: _destroyCancellationToken);

        List<UniTask> fadeTasks = new();

        // フェードアウト処理
        foreach (MeshRenderer shard in fractured)
        {
            fadeTasks.Add(ObjectFader.FadeOut(shard, fadeOutTime, _destroyCancellationToken));
        }

        await UniTask.WhenAll(fadeTasks);

        Destroy(gameObject);
    }
}
