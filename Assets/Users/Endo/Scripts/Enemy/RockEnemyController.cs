using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class RockEnemyController : EnemyBase
{
    [SerializeField]
    private MeshRenderer[] fractured;

    [SerializeField, Header("死亡 (割れた) 後にフェードアウトするまでの時間 (秒)")]
    private float waitTimeAfterDead;

    [SerializeField, Header("フェードアウト時間 (秒)")]
    private float fadeOutTime;

    [SerializeField]
    private Collider tutorialRangeCollider;

    private MeshFilter   _selfMeshFilter;
    private MeshRenderer _selfRenderer;
    private Collider     _selfCollider;

    private bool _isDead;

    private CancellationToken _destroyCancellationToken;

    protected override void Start()
    {
        base.Start();

        _selfMeshFilter = GetComponent<MeshFilter>();
        _selfRenderer   = GetComponent<MeshRenderer>();
        _selfCollider   = GetComponent<Collider>();

        _destroyCancellationToken = this.GetCancellationTokenOnDestroy();

        foreach (MeshRenderer shard in fractured)
        {
            shard.gameObject.SetActive(false);
        }

        tutorialRangeCollider.OnTriggerEnterAsObservable()
                             .Subscribe(other =>
                             {
                                 if (other.isTrigger)
                                     return;

                                 if (1 << other.gameObject.layer == LayerConstants.Player)
                                 {
                                     PlayerInputEventEmitter.Instance.Broker.Publish(
                                         MainGameEvent.Tutorial.OnTask4Passed.GetEvent());
                                 }
                             })
                             .AddTo(this);
    }

    public override void Attack()
    {
        // 攻撃なし
    }

    protected override async void OnDead()
    {
        if (_isDead) return;

        _isDead = true;

        MainGameMissionManager.Instance.CountUp();

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
