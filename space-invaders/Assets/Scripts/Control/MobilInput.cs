using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MobilInput : MonoBehaviour, IButtonsHandler
{
    HoldButton holdButtonLeft;
    HoldButton holdButtonRight;
    Button buttonShoot;
    bool shoot;
    void Awake()
    {
        holdButtonLeft = transform.Find("ArrowLeft").GetComponent<HoldButton>();
        holdButtonRight = transform.Find("ArrowRight").GetComponent<HoldButton>();
        buttonShoot = transform.Find("Shoot").GetComponent<Button>();

        buttonShoot.onClick.AddListener(() => SetButtonShoot(true));
        Observable.EveryLateUpdate()
            .Where(_ => shoot)
            .Subscribe(_ => { SetButtonShoot(false); });
    }

    public bool GetButtonLeft()
    {
        return holdButtonLeft.isHold;
    }

    public bool GetButtonRight()
    {
        return holdButtonRight.isHold;
    }

    void SetButtonShoot(bool value)
    {
        shoot = value;
    }

    public bool GetButtonShoot()
    {
        return shoot;
    }

    
}
