using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
{
    [SerializeField] GameConfig config;
    public override void InstallBindings()
    {
        Container.BindInstance(config).AsSingle();
    }
}