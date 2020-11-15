using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Rewired.Integration.UnityUI;
public class PausePanel : BehaviourBase
{
    public Transform OpenTransform;
    public Vector3 startPostition;

    public float animationSpeed;
    public Ease ease;
    public Button playButton;

    protected override void Awake()
    {
        base.Awake();

        startPostition = transform.position;
    }

    public void Continue()
    {
        MJGameManager.Instance.PauseUnpause();
    }

    public void Quit()
    {
        Application.Quit();
    }

    protected override void CustomFixedUpdate() { }

    protected override void CustomUpdate() { }

    protected override void PauseChanged(bool pause)
    {
        if (pause)
        {
            transform.DOMove(OpenTransform.position,animationSpeed).SetEase(ease);
            UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = playButton.gameObject;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }
        else
        {
            transform.DOMove(startPostition,animationSpeed).SetEase(ease);
        }
    }
}
