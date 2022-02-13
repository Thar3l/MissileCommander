using System;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputController : Singleton<InputController>
{
    private struct InputKeyAction
    {
        private UnityAction<Vector2> action;

        public delegate bool Restriction();
        public event Restriction IsRestriction;

        private bool CanExecute => IsRestriction == null || !IsRestriction();

        public InputKeyAction(UnityAction<Vector2> action, Restriction restrictionMethod=null)
        {
            this.action = action;
            IsRestriction = restrictionMethod;
        }

        public void Execute()
        {
            if (CanExecute)
                action?.Invoke(Input.mousePosition);
        }
    }

    [SerializeField] private Camera camera;
    [SerializeField] private EventSystem EventSystem;
    private Dictionary<KeyCode, InputKeyAction> keyBindDict = new Dictionary<KeyCode, InputKeyAction>();
    
    void Update()
    {
        SearchForKeyDown();
    }

    public void BindKey(KeyCode key, UnityAction<Vector2> action)
    {
        if (keyBindDict.ContainsKey(key))
        {
            Debug.LogError(string.Format("Key {0} has already been bound.", key));
            return;
        }

        keyBindDict.Add(key, new InputKeyAction(action, GetKeyRestriction(key)));
    }

    private InputKeyAction.Restriction GetKeyRestriction(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Mouse0:
            case KeyCode.Mouse1:
            case KeyCode.Mouse2:
                return EventSystem.IsPointerOverGameObject;
        }

        return null;
    }

    private void ExecuteKeyAction(KeyCode key)
    {
        if (keyBindDict.ContainsKey(key))
            keyBindDict[key].Execute();
    }

    private void SearchForKeyDown()
    {
        if (Input.anyKeyDown)
            foreach (var key in keyBindDict.Keys)
                if (Input.GetKeyDown(key))
                    ExecuteKeyAction(key);
    }

    public Vector3 GetWorldPointFromMousePosition(Vector2 mousePosition)
    {
        return Utils.MousePositionToWorldPoint(camera, mousePosition);
    }
}