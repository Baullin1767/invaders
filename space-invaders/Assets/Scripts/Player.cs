using DG.Tweening.Core.Easing;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;

    Projectile laser;

    [Inject]
    GameManager gameManager;

#if UNITY_EDITOR
    [Inject]
    KeyboardInput keyboardInput;
#else
    [Inject]
    MobilInput mobilInput;

#endif
    void Start()
    {
        Vector3 position = transform.position;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

#if UNITY_EDITOR
        Observable.EveryUpdate()
           .Where(_ => keyboardInput.GetButtonLeft())
           .Subscribe(_ => { position.x -= speed * Time.deltaTime; });
        Observable.EveryUpdate()
            .Where(_ => keyboardInput.GetButtonRight())
            .Subscribe(_ => { position.x += speed * Time.deltaTime; }); 
        Observable.EveryUpdate()
            .Where(_ => laser == null && keyboardInput.GetButtonShoot())
            .Subscribe(_ => { laser = Instantiate(laserPrefab, transform.position, Quaternion.identity); });
#else
        Observable.EveryUpdate()
           .Where(_ => mobilInput.GetButtonLeft())
           .Subscribe(_ => { position.x -= speed * Time.deltaTime; });
        Observable.EveryUpdate()
            .Where(_ => mobilInput.GetButtonRight())
            .Subscribe(_ => { position.x += speed * Time.deltaTime; }); 
        Observable.EveryUpdate()
            .Where(_ => laser == null && mobilInput.GetButtonShoot())
            .Subscribe(_ => { laser = Instantiate(laserPrefab, transform.position, Quaternion.identity); });

#endif

        Observable.EveryUpdate()
            .Where(_ => transform.position != position)
            .Subscribe(_ =>
            { transform.position = position; });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            gameManager.OnPlayerKilled(this);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Boomb"))
        {
            gameManager.SetScore(gameManager.Score + 5);
        }
    }

}
