using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
public class MysteryShip : MonoBehaviour
{
    public float speed = 5f;
    public float cycleTime = 30f;
    public int score = 300;

    private Vector2 leftDestination;
    private Vector2 rightDestination;
    private int direction = -1;
    private bool spawned;

    [Inject]
    GameManager gameManager;

    private void Start()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        leftDestination = new Vector2(leftEdge.x - 1f, transform.position.y);
        rightDestination = new Vector2(rightEdge.x + 1f, transform.position.y);

        Despawn();

        Observable.EveryUpdate()
            .Where(_ => spawned && direction == 1)
            .Subscribe(_ => { Move(Vector3.right); });
        Observable.EveryUpdate()
            .Where(_ => spawned && direction == -1)
            .Subscribe(_ => { Move(Vector3.left); });
    }

   
    private void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;

        if (transform.position.x >= rightDestination.x ||
            transform.position.x <= leftDestination.x)
        {
            Despawn();
        }
    }

    private void Spawn()
    {
        direction *= -1;

        if (direction == 1) {
            transform.position = leftDestination;
        } else {
            transform.position = rightDestination;
        }

        spawned = true;
    }

    private void Despawn()
    {
        spawned = false;

        if (direction == 1) {
            transform.position = rightDestination;
        } else {
            transform.position = leftDestination;
        }

        Invoke(nameof(Spawn), cycleTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn();
            gameManager.OnMysteryShipKilled(this);
        }
    }

}
