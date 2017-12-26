using UnityEngine;
using System.Collections;

public class DHInput : MonoBehaviour
{
    public enum ButtonStates { Off, ButtonDown, ButtonPressed, ButtonUp }

    public static ButtonStates ProcessAxisAsButton(string axisName, float threshold, ButtonStates currentState)
    {
        float axisValue = Input.GetAxis(axisName);
        ButtonStates returnState;

        //
        if (axisValue < threshold)
        {
            if (currentState == ButtonStates.ButtonPressed)
            {
                returnState = ButtonStates.ButtonUp;
            }
            else
            {
                returnState = ButtonStates.Off;
            }
        }
        else
        {
            if (currentState == ButtonStates.Off)
            {
                returnState = ButtonStates.ButtonDown;
            }
            else
            {
                returnState = ButtonStates.ButtonPressed;
            }
        }
        return returnState;
    }

    public class DHButton
    {
        public DHStateMachine<ButtonStates> State { get; protected set; }
        public string ButtonID;

        public delegate void ButtonDownMethodDelegate();
        public delegate void ButtonPressedMethodDelegate();
        public delegate void ButtonUpMethodDelegate();

        public ButtonDownMethodDelegate ButtonDownMethod;
        public ButtonPressedMethodDelegate ButtonPressedMethod;
        public ButtonUpMethodDelegate ButtonUpMethod;

        public DHButton(string buttonID, ButtonDownMethodDelegate btnDown, ButtonPressedMethodDelegate btnPressed, ButtonUpMethodDelegate btnUp)
        {
            ButtonID = buttonID;
            ButtonDownMethod = btnDown;
            ButtonUpMethod = btnUp;
            ButtonPressedMethod = btnPressed;
            State = new DHStateMachine<ButtonStates>(null, false);
            State.ChangeState(ButtonStates.Off);
        }

        public virtual void TriggerButtonDown()
        {
            ButtonDownMethod();
        }

        public virtual void TriggerButtonPressed()
        {
            ButtonPressedMethod();
        }

        public virtual void TriggerButtonUp()
        {
            ButtonUpMethod();
        }
    }
}
