using QEntity;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class QInputField : UIEntity, ISelectHandler, IDeselectHandler
{
    InputField inputField;
    Action OnSelectEvent;
    Action OnDeselectEvent;

    public string text
    {
        get { return inputField.text; }
        set { inputField.text = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        inputField = GetComponent<InputField>();
    }

    private void Start()
    {
        ResetTransform();
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Ctrl.SetQuickkeyIgnore(true);
        if (OnSelectEvent.IsNotNull())
        {
            OnSelectEvent.Invoke();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Ctrl.SetQuickkeyIgnore(false);
        if (OnDeselectEvent.IsNotNull())
        {
            OnDeselectEvent.Invoke();
        }
    }
    public InputField GetInputField()
    {
        return inputField;
    }

    public void AddSelectEvent(Action action)
    {
        OnSelectEvent += action;
    }

    public void RemoveSelectEvent(Action action)
    {
        OnSelectEvent -= action;
    }

    public void AddDeselectEvent(Action action)
    {
        OnDeselectEvent += action;
    }

    public void RemoveDeselectEvent(Action action)
    {
        OnDeselectEvent -= action;
    }

    public void SetFocusOnInput(bool state)
    {
        if (state)
        {
            inputField.ActivateInputField();
        }
        else
        {
            inputField.DeactivateInputField();
        }
    }
}
