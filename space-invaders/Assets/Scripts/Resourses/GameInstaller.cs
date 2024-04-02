using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject]
    GameConfig config;
    public override void InstallBindings()
    {
        Container.Bind<KeyboardInput>().AsSingle();

        Container.BindFactory<Sprite[], float, int, GameManager, Invaders, SignalBus, Invader, Invader.InvaderFactory>()
            .FromComponentInNewPrefab(config.InvaderBase)
            .WithGameObjectName("Invader");

        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<TouchBorderSignal>();
    }
}