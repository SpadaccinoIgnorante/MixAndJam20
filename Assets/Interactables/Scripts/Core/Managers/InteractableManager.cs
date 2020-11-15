using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableManager : MonoBehaviour
{
    [Serializable]
    private struct Managable
    {
        public AInteractable interactable;
        public bool skipUpdate;
    }

    public static InteractableManager Instance { get { return Factory(); } }
    private static InteractableManager _instance;

    [SerializeField]
    private Dictionary<Guid, Managable> interactables = new Dictionary<Guid, Managable>();

    private static bool initialized = false;

    private void Awake()
    {
        if (!initialized)
        {
            initialized = true;
            Factory();
        }

        SceneManager.activeSceneChanged += this.OnChangeScene;
    }

    public static InteractableManager Factory()
    {
        if (GameObject.FindObjectOfType<InteractableManager>() != null)
            return _instance;

        if (_instance == null)
        {
            var iManager = new GameObject("InteractableManager", typeof(InteractableManager));
            _instance = iManager.GetComponent<InteractableManager>();
        }
        return _instance;
    }

    public static bool IsInstanced()
    {
        return _instance != null;
    }

    public void OnChangeScene(Scene scene1, Scene scene2)
    {
        StopAll();
    }

    public void AddInteractable(AInteractable interactable)
    {
        if (interactable == null)
            return;

        if (!interactables.ContainsKey(interactable.ID))
            interactables.Add(interactable.ID, new Managable { interactable = interactable, skipUpdate = false });
    }

    public void RemoveInteractable(Guid id)
    {
        if (interactables == null)
            return;

        if (interactables.ContainsKey(id))
            interactables.Remove(id);
    }

    public void SkipUpdateFor(Guid interactableID)
    {
        if (interactables.ContainsKey(interactableID))
        {
            Managable outValue;
            if (interactables.TryGetValue(interactableID, out outValue))
                outValue.skipUpdate = true;
        }
    }

    public void StopAll()
    {
        if (interactables == null) return;

        foreach (var interactable in interactables)
        {
            if (interactable.Value.interactable == null) continue;

            interactable.Value.interactable.Stop();
        }
    }

    private void Update()
    {
        foreach (var interactable in interactables)
        {
            if (interactable.Value.skipUpdate)
                continue;

            interactable.Value.interactable.OnUpdate();
        }

        foreach (var kv in interactables)
        {
            if (kv.Value.interactable == null)
                interactables.Remove(kv.Key);
        }
    }

    private void OnDestroy()
    {
        StopAll();
    }

    private void OnApplicationQuit()
    {
        interactables = null;
        _instance = null;
    }
}
