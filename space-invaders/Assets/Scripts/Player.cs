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

    private Projectile laser;

    [Inject]
    GameManager gameManager;

    private void Start()
    {
        Vector3 position = transform.position;
        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            .Subscribe(_ => { position.x -= speed * Time.deltaTime; });
        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            .Subscribe(_ => { position.x += speed * Time.deltaTime; });


        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

        Observable.EveryUpdate()
            .Where(_ => transform.position != position)
            .Subscribe(_ =>
        { transform.position = position; });

        Observable.EveryUpdate()
            .Where(_ => laser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            .Subscribe(_ => { laser = Instantiate(laserPrefab, transform.position, Quaternion.identity); });

    }

    private void OnTriggerEnter2D(Collider2D other)
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
