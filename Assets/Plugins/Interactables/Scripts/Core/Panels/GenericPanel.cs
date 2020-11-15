using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;

public class GenericPanel : BasePanel
{
    public bool IsMinimized { get { return this.isMinimized; } }

    protected const string MINIMIZE_RESOURCES_PATH = "UI/MinimizerButton";

    [Space(10)]
    [Header("Mimimize")]
    #region MinimizeFields
    
    [SerializeField]
    protected bool canBeClosedOrMinimized;
    [SerializeField]
    protected bool destroyIfCantBeMinimized;

    [SerializeField]
    protected Ease minimzeAnimation;

    [SerializeField]
    protected GameObject closeButton;
    [SerializeField]
    protected GameObject minimazerButtonContainer;

    protected GameObject minimizeButton;
    protected bool isMinimized;
    protected Vector3 maximazePos;

    #endregion

    [Space(10)]
    [Header("Graphics")]
    [SerializeField]
    protected Sprite panelIcon;

    protected override void Start()
    {
        base.Start();

        // Attach callback
        var draggable = this.gameObject.GetComponent<DraggableUI>();

        if (draggable != null)
            draggable.OnMove.AddListener(this.PanelMoved);

        this.InitMinimizer();
    }

    public virtual void Minimize()
    {
        if (minimizeButton == null && destroyIfCantBeMinimized)
        {
            GameObject.DestroyImmediate(this.gameObject);
            return;
        }
        else if (minimizeButton == null && !destroyIfCantBeMinimized)
        {
            ClosePanel();
            return;
        }

        this.maximazePos = this.transform.position;

        this.isMinimized = true;

        var lastSelected = EventSystem.current.currentSelectedGameObject;

        this.transform.DOMove(this.minimizeButton.transform.position, 1.1f, false).
                             OnPlay(() => { this.transform.DOScale(0,0.8f); }).
                             SetEase(this.minimzeAnimation);

    }

    public virtual void Maximize()
    {
        // in this way you can click on the maximaze button, and if the panel is already maximazed, then will minimized
        if (!this.isMinimized)
            this.Minimize();
        else
        {
            var lastSelected = EventSystem.current.currentSelectedGameObject;

            this.transform.DOMove(this.maximazePos, 1.1f, false).
                                  OnPlay(() => { this.transform.DOScale(1, 0.8f); }).
                                  SetEase(this.minimzeAnimation);
           
            this.isMinimized = false;
        }
    }

    protected virtual void InitMinimizer()
    {
        if (!canBeClosedOrMinimized)
        {
            if (closeButton != null)
                closeButton.SetActive(false);
            return;
        }


        if (minimazerButtonContainer == null) return;

        this.minimizeButton = GameObject.Instantiate(Resources.Load<GameObject>(MINIMIZE_RESOURCES_PATH),
                                                     this.minimazerButtonContainer.transform);

        var minimizerButton = this.minimizeButton.GetComponent<MinimizerButton>();

        if (minimizerButton != null)
        {
            minimizerButton.InitializeButton(this.panelTitle, this.panelIcon);

            minimizerButton.OnClick.AddListener(this.Maximize);
        }

        this.maximazePos = this.transform.position;
    }

    protected virtual void PanelMoved(Vector3 pos)
    {
        this.maximazePos = pos;
    }
}
