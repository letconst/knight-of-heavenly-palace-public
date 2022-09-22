using UnityEngine;

public sealed class DebugMovingEnemy : EnemyBase
{
    [SerializeField]
    private Vector3 centerPos;

    [SerializeField]
    private float moveScale = 1f;

    [SerializeField]
    private float rotateTime = 1f;

    private Rigidbody _selfRig;

    protected override void Start()
    {
        base.Start();

        centerPos = transform.position;
        _selfRig  = GetComponent<Rigidbody>();
    }

    public override void Attack()
    {
    }

    protected override void OnDead()
    {
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float x = Mathf.Sin(2 * Mathf.PI * (1f / rotateTime) * Time.time) * moveScale;
        float y = Mathf.Cos(2 * Mathf.PI * (1f / rotateTime) * Time.time) * moveScale;

        _selfRig.MovePosition(new Vector3(centerPos.x + x, centerPos.y + y, centerPos.z));
    }
}
