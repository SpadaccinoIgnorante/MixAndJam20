using System;
using UnityEngine;
using TMPro;

public abstract class BasePanel : MonoBehaviour
{
    public Action onPanelClose;
    public Action onOpenPanel;

    public string PanelID { get { return _id; } private set { _id = value; } }

    public GameObject Content { get { return this._content; } }

    public bool IsContentFull
    {
        get
        {
            if (this._content == null)
            {
                Debug.LogError("Content is null");
                return false;
            }

            return this._content.transform.childCount > 0;
        }
    }

    public bool IsOpened { get { return isOpened; } }

    [Space(10)]
    [Header("Basic stuff")]
    [SerializeField]
    protected string panelTitle;

    [SerializeField]
    public bool headerVisible = true;

    [SerializeField]
    protected GameObject header;

    [SerializeField]
    protected TextMeshProUGUI title;

    [SerializeField]
    protected bool openAtStart;

    protected bool isOpened;

    [SerializeField]
    private GameObject _content;

    [SerializeField]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.InfoBox("WARNING! Set id to every panel", Sirenix.OdinInspector.InfoMessageType.Warning)]
#endif
    private string _id;

    protected virtual void Awake()
    {
        if (openAtStart)
        {
            OpenPanel();
        }
    }

    protected virtual void Start()
    {
        SetTitle(panelTitle);

        if (this._content == null)
        {
            var cT = this.transform.Find("Content");
            if(cT != null)
                this._content = cT.gameObject;
        }
    }

    public virtual void SetTitle(string title, float fontSize = -1)
    {
        this.panelTitle = title;

        if (this.title != null)
        {
            if (fontSize == -1)
                fontSize = this.title.fontSize;

            this.title.fontSize = fontSize;

            this.title.SetText(this.panelTitle);
        }
    }

    public virtual void OpenPanel()
    {
        this.isOpened = true;

        this.gameObject.SetActive(true);

        if (this.header != null)
            this.header.SetActive(headerVisible);

        onOpenPanel?.Invoke();
    }

    public virtual void ClosePanel(bool destroy = false)
    {
        if (!destroy)
        {
            this.gameObject.SetActive(false);
            this.isOpened = false;
        }
        else
            GameObject.Destroy(this.gameObject);

        onPanelClose?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        
    }
}
