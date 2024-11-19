using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlManager : MonoBehaviour
{
    [SerializeField] private List<InputAction> throttles;
    [SerializeField] private List<InputAction> breaks;
    [SerializeField] private int currentController = -1;

    private void OnEnable()
    {
        GetController();
        if(currentController != -1)
        {
            throttles[currentController].Enable();
        }
    }

    private void OnDisable()
    {
        if(currentController != -1)
        {
            throttles[currentController].Disable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentController != -1)
        {
            ReadThrottle();
        }
    }

    private void ReadThrottle()
    {
        print(throttles[currentController].ReadValue<float>() + " - Throttle");
        
        // Set throttle value in train.
    }
    private void ReadBreaks()
    {
        print(breaks[currentController].ReadValue<float>() + " - Breaks");
    }

    private void GetController()
    {
        var joystick = Joystick.current;
        name = joystick.name;
        print(name);

        if (name == "Saitek Saitek Pro Flight Quadrant")
        {
            currentController = 0;
        }
        else if(name == "asdasd")
        {
            currentController = 1;
        }
    }
}
