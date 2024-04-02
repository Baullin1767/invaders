using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    public float speed = 20f;

    private new BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();

        if (bunker == null || bunker.CheckCollision(collider, transform.position)
            || other.gameObject.layer == LayerMask.NameToLayer("Boomb"))
            Destroy(gameObject);


        //if(other.gameObject.layer == LayerMask.NameToLayer("Bunker"))
        //    Camera.main.transform
        //        .DOShakePosition(0.05f, 0.5f, 10, 90f, false, true, ShakeRandomnessMode.Harmonic)
        //        .SetEase(Ease.InOutBounce)
        //        .SetLink(Camera.main.gameObject);
    }

}
