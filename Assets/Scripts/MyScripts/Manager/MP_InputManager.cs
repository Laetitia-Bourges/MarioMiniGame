using System;
using UnityEngine;

public class MP_InputManager : MP_Singleton<MP_InputManager>
{
    public event Action OnUpdateInput = null;

    private void OnDestroy()
    {
        OnUpdateInput = null;
    }
    private void Update()
    {
        OnUpdateInput?.Invoke();
    }


    public void RegisterButton(KeyCode _input, Action<bool> _callback)
    {
        OnUpdateInput += () => _callback?.Invoke(Input.GetKey(_input));
    }
    public void UnRegisterButton(KeyCode _input, Action<bool> _callback)
    {
        OnUpdateInput -= () => _callback?.Invoke(Input.GetKey(_input));
    }
}
