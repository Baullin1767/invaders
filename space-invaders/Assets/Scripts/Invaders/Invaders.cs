using UniRx;
using UnityEngine;
using Zenject;

public class Invaders : MonoBehaviour
{
    [Inject]
    GameConfig config;
    [Inject]
    GameManager gameManager;
    [Inject]
    SignalBus signalBus;
    [Inject]
    Invader.InvaderFactory invaderFactory;


    AnimationCurve speed;

    public Vector3 direction = Vector3.right;
    private Vector3 initialPosition;

    [HideInInspector]
    int rows;
    [HideInInspector]
    int columns;

    Projectile[] missilePrefabs;
    float missileSpawnRate;


    int totalCount;
    int amountAlive;
    int amountKilled;
    float percentKilled;

    float curSpeed;


    private void Awake()
    {

        initialPosition = transform.position;
        rows = config.rows; columns = config.columns;
        missilePrefabs = config.missilePrefabs;
        missileSpawnRate = config.missileSpawnRate;
        speed = config.speedInvaders;

    }
    
    public void Start()
    {

        CreateInvaderGrid();
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);

        Observable.EveryUpdate()
            .Where(_ => curSpeed != speed.Evaluate(percentKilled))
            .Subscribe(_ => { curSpeed = speed.Evaluate(percentKilled); });

        Observable.EveryUpdate()
            .Where(_ => totalCount != rows * columns)
            .Subscribe(_ => { totalCount = rows * columns; });

        Observable.EveryUpdate()
            .Where(_ => amountAlive != GetAliveCount())
            .Subscribe(_ => { amountAlive = GetAliveCount(); });

        Observable.EveryUpdate()
            .Where(_ => amountKilled != totalCount - amountAlive)
            .Subscribe(_ => { amountKilled = totalCount - amountAlive; });

        Observable.EveryUpdate()
            .Where(_ => percentKilled != amountKilled / totalCount)
            .Subscribe(_ => { percentKilled = amountKilled / totalCount; });

        Observable.EveryUpdate()
            .Where(_ => percentKilled != amountKilled / totalCount)
            .Subscribe(_ => { percentKilled = amountKilled / totalCount; });

        signalBus.Subscribe<TouchBorderSignal>(AdvanceRow);
    }

    void OnDestroy()
    {
        signalBus.Unsubscribe<TouchBorderSignal>(AdvanceRow);
    }


    /// <summary>
    /// Calling Invader Factory
    /// </summary>
    /// <param name="index">index of invader prefab</param>
    /// <param name="parent">parent object for invader</param>
    /// <returns></returns>
    Invader CreateInvader(int index, Transform parent)
    {
        Invader invader = invaderFactory.Create(config.invaders[index].animationSpritesInvaders,
            config.invaders[index].animationTimeInvaders,
            config.invaders[index].scoreInvaders,
            gameManager,
            this,
            signalBus);
        invader.transform.SetParent(parent);

        return invader;
    }



    private void CreateInvaderGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {
                Invader invader = CreateInvader(i, transform);
                Vector3 position = rowPosition;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }
    private void MissileAttack()
    {
        int amountAlive = GetAliveCount();

        if (amountAlive == 0) {
            return;
        }

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeInHierarchy) {

                if (Random.value < (1f / amountAlive))
                {
                    Instantiate(missilePrefabs[Random.Range(0, missilePrefabs.Length)]
                        ,invader.position
                        ,Quaternion.identity);

                    break;
                }
            }
        }
    }

    private void Update()
    {
        transform.position += curSpeed * Time.deltaTime * direction;
    }

    private void AdvanceRow(TouchBorderSignal touchBorder)
    {
        direction = touchBorder.direction;

        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform) {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf) {
                count++;
            }
        }

        return count;
    }

}
