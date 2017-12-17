using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class InputManager : Singleton<InputManager>
{
    public Vector2 Threshold = new Vector2(0.1f, 0.4f);

    public DHInput.DHButton JumpButton { get; protected set; }

    public DHInput.DHButton RunButton { get; protected set; }

    public DHInput.DHButton SprintButton { get; protected set; }

    protected List<DHInput.DHButton> ButtonList;
    protected Vector3 primaryMovement = Vector3.zero;
    public Vector3 PrimaryMovement { get { return primaryMovement; } }

    protected string axisHorizontal;
    protected string axisVertical;


    protected virtual void Start()
    {
        InitializeButtons();
        InitializeAxis();
    }

    protected virtual void InitializeButtons()
    {
        ButtonList = new List<DHInput.DHButton>();
        ButtonList.Add(JumpButton = new DHInput.DHButton("Jump", JumpButtonDown, JumpButtonPressed, JumpButtonUp));
        ButtonList.Add(SprintButton = new DHInput.DHButton("Sprint", SprintButtonDown, SprintButtonPressed, SprintButtonUp));
    }

    protected virtual void InitializeAxis()
    {
        axisHorizontal = "Horizontal";
        axisVertical = "Vertical";
    }

    protected virtual void LateUpdate()
    {
        ProcessButtonStates();
    }

    protected virtual void Update()
    {
        SetMovement();
        GetInputButtons();
    }

    protected virtual void GetInputButtons()
    {
        foreach (DHInput.DHButton button in ButtonList)
        {
            if (Input.GetButton(button.ButtonID))
            {
                button.TriggerButtonPressed();
            }
            if (Input.GetButtonDown(button.ButtonID))
            {
                button.TriggerButtonDown();
            }
            if (Input.GetButtonUp(button.ButtonID))
            {
                button.TriggerButtonUp();
            }
        }
    }

    public virtual void ProcessButtonStates()
    {
        foreach (DHInput.DHButton button in ButtonList)
        {
            if (button.State.CurrentState == DHInput.ButtonStates.ButtonDown)
            {
                button.State.ChangeState(DHInput.ButtonStates.ButtonPressed);
            }
            if (button.State.CurrentState == DHInput.ButtonStates.ButtonUp)
            {
                button.State.ChangeState(DHInput.ButtonStates.Off);
            }
        }
    }

    public virtual void SetMovement()
    {
        primaryMovement.x = Input.GetAxis(axisHorizontal);
        primaryMovement.y = Input.GetAxis(axisVertical);
    }

    public virtual void SetMovement(Vector2 movement)
    {
            primaryMovement.x = movement.x;
            primaryMovement.y = movement.y;
    }

    public virtual void JumpButtonDown() { JumpButton.State.ChangeState(DHInput.ButtonStates.ButtonDown); }
    public virtual void JumpButtonPressed() { JumpButton.State.ChangeState(DHInput.ButtonStates.ButtonPressed); }
    public virtual void JumpButtonUp() { JumpButton.State.ChangeState(DHInput.ButtonStates.ButtonUp); }

    public virtual void SprintButtonDown() { SprintButton.State.ChangeState(DHInput.ButtonStates.ButtonDown); }
    public virtual void SprintButtonPressed() { SprintButton.State.ChangeState(DHInput.ButtonStates.ButtonPressed); }
    public virtual void SprintButtonUp() { SprintButton.State.ChangeState(DHInput.ButtonStates.ButtonUp); }
}