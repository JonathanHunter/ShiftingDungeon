using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ShiftingDungeon.Util;

public static class Navigator
{
    public static void Navigate(CustomInput.UserInput direction, GameObject defaultGameObject)
    {
        GameObject next = EventSystem.current.currentSelectedGameObject;
        if (next == null)
        {
            if(defaultGameObject != null) EventSystem.current.SetSelectedGameObject(defaultGameObject);
            return;
        }

        bool nextIsValid = false;
        while (!nextIsValid)
        {
            switch (direction)
            {
                case CustomInput.UserInput.Up:
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp() != null)
                        next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp().gameObject;
                    else next = null;
                    break;
                case CustomInput.UserInput.Down:
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown() != null)
                        next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown().gameObject;
                    else next = null;
                    break;
                case CustomInput.UserInput.Left:
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft() != null)
                        next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft().gameObject;
                    else next = null;
                    break;
                case CustomInput.UserInput.Right:
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight() != null)
                        next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight().gameObject;
                    else next = null;
                    break;
            }
            if (next != null)
            {
                EventSystem.current.SetSelectedGameObject(next);
                nextIsValid = next.GetComponent<Selectable>().interactable;
            }
            else nextIsValid = true;
        }
    }

    public static void CallSubmit()
    {
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, pointer, ExecuteEvents.submitHandler);
    }
}