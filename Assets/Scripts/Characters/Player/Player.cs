using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField]
    bool regenerateHealth = true;
    [SerializeField]
    float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)]
    float healthRegeneratePercent;

    [Header("--- INPUT ---")]
    [SerializeField] private PlayerInput input;

    [Header("--- MOVE ---")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float paddingX = 0.2f;
    [SerializeField] private float paddingY = 0.2f;
    [SerializeField] private float accelerationTime = 3f;
    [SerializeField] private float decelerationTime = 3f;
    [SerializeField] private float moveRotationAngle = 50f;


    [Header("--- FIRE ---")]
    [SerializeField] private GameObject projectile1;
    [SerializeField] private GameObject projectile2;
    [SerializeField] private GameObject projectile3;
    [SerializeField, Range(0, 2)] int weaponPower = 0;
    /// <summary>
    /// projectile spawn position
    /// </summary>
    [SerializeField] private Transform muzzleMiddle;
    [SerializeField] private Transform muzzleTop;
    [SerializeField] private Transform muzzleBottom;
    [SerializeField] private float fireInterval = 0.2f;


    private Rigidbody2D _rigidbody;
    private Coroutine _moveCoroutine;
    private Coroutine _healthRegenerateCoroutine;

    private WaitForSeconds waitForFireInterval;

    private WaitForSeconds waitHealthRegenerateTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        input.ONMove += Move;
        input.ONStopMove += StopMove;
        input.ONFire += Fire;
        input.ONStopFire += StopFire;
    }

    private void OnDisable()
    {
        input.ONMove -= Move;
        input.ONStopMove -= StopMove;
        input.ONFire -= Fire;
        input.ONStopFire -= StopFire;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
    }

    private void Start()
    {
        _rigidbody.gravityScale = 0.0f;
        input.EnableGameplayInput();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (gameObject.activeSelf)
        {
            if (_healthRegenerateCoroutine != null)
            {
                StopCoroutine(_healthRegenerateCoroutine);
            }
            _healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
        }
    }

    #region MOVE

    private void Move(Vector2 moveInput)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        _moveCoroutine =
            StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, moveRotation));
        StartCoroutine(MovePositionLimitCoroutine());
    }

    private void StopMove()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        _moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    private IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMovablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }

    private IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        var t = 0f;

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, moveVelocity, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t);
            yield return null;
        }
    }

    #endregion

    #region FIRE

    private void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    private void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMiddle.position, Quaternion.identity);
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleTop.position, Quaternion.identity);
                    PoolManager.Release(projectile1, muzzleBottom.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position, Quaternion.identity);
                    PoolManager.Release(projectile2, muzzleTop.position, Quaternion.identity);
                    PoolManager.Release(projectile3, muzzleBottom.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
            yield return waitForFireInterval;
        }
    }

    #endregion
}