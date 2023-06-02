using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private KeyCode[] up = { KeyCode.W, KeyCode.UpArrow };
    [SerializeField] private KeyCode[] down = { KeyCode.S, KeyCode.DownArrow };
    [SerializeField] private KeyCode[] left = { KeyCode.A, KeyCode.LeftArrow };
    [SerializeField] private KeyCode[] right = { KeyCode.D, KeyCode.RightArrow };
    [SerializeField] private KeyCode[] select = { KeyCode.M };

    private Selectable nextSelectable;
    private Button button;

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return;

        if (GetKeyDown(up))
        {
            if (nextSelectable = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp())
                nextSelectable.Select();
        }
        else if (GetKeyDown(down))
        {
            if (nextSelectable = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown())
                nextSelectable.Select();
        }
        else if (GetKeyDown(left))
        {
            if (nextSelectable = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft())
                nextSelectable.Select();
        }
        else if (GetKeyDown(right))
        {
            if (nextSelectable = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight())
                nextSelectable.Select();
        }
        else if (GetKeyDown(select))
        {
            if (button = eventSystem.currentSelectedGameObject.GetComponent<Button>())
                button.onClick.Invoke();
            else
                Debug.Log("Selectable could not be pressed");
        }
    }

    private bool GetKeyDown(KeyCode[] keyCodes)
    {
        foreach (KeyCode keyCode in keyCodes)
        {
            if (Input.GetKeyDown(keyCode))
                return true;
        }
        return false;
    }
}


