using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Invader : MonoBehaviour
{
    [Inject]
    public Sprite[] animationSprites = new Sprite[0];
    [Inject]
    public float animationTime = 1f;
    [Inject]
    public int score = 10;
    [Inject]
    GameManager gameManager;
    [Inject]
    Invaders invadersController;

    [Inject]
    SignalBus signalBus;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;


    Vector3 leftEdge;
    Vector3 rightEdge;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animationSprites[0];
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);


        Observable.EveryUpdate()
            .Where(_ => invadersController.direction == Vector3.right && transform.position.x >= (rightEdge.x - 1f))
            .Subscribe(_ => { signalBus.Fire(new TouchBorderSignal(Vector3.left)); });
        Observable.EveryUpdate()
            .Where(_ => invadersController.direction == Vector3.left && transform.position.x <= (leftEdge.x + 1f))
            .Subscribe(_ => { signalBus.Fire(new TouchBorderSignal(Vector3.right)); });

    }
    private void AnimateSprite()
    {
        animationFrame++;

        if (animationFrame >= animationSprites.Length) {
            animationFrame = 0;
        }

        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            gameManager.OnInvaderKilled(this);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary")) {
            gameManager.OnBoundaryReached();
        }
    }

    public class InvaderFactory : PlaceholderFactory<Sprite[], float, int, GameManager, Invaders, SignalBus, Invader> { };
}
