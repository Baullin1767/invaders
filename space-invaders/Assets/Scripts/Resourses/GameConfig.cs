using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Config", menuName = "Craete game config")]
public class GameConfig : ScriptableObject
{
    public int lives;
    [Space]
    [Header("Invaders")]
    public Invader InvaderBase;
    public InvadersStruct[] invaders;
    public AnimationCurve speedInvaders = new AnimationCurve();

    [Serializable]
    public struct InvadersStruct
    {
        public Sprite[] animationSpritesInvaders;
        [Space]
        public float animationTimeInvaders;
        public int scoreInvaders;

    }

    [Header("InvadersGrid")]
    public int rows = 5;
    public int columns = 11;

    [Header("InvadersMissiles")]
    public Projectile[] missilePrefabs;
    public float missileSpawnRate = 1f;

}
