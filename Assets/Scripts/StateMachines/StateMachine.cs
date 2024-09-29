using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State _currentState;

    #region  Public Methods
    public void SwitchState(State newState)
    {
        if (newState == null) return;
        _currentState?.Exit();
        newState?.Enter();
        _currentState = newState;
    }
    #endregion

    #region  Private Methods
    private void Update()
    {
        _currentState?.Tick(Time.deltaTime);
    }
    #endregion
}
