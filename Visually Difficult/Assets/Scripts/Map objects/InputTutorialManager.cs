using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject keyboardValues;
    [SerializeField] private GameObject controllerValues;

    private void Start()
    {
        bool hasGamePad = Gamepad.all.Count > 0;
        keyboardValues.SetActive(!hasGamePad);
        controllerValues.SetActive(hasGamePad);
    }
}
