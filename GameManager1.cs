using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public event Action<bool> OnMagnetActivationChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void MagnetActivationChanged(bool isMagnetActive)
    {
        if (OnMagnetActivationChanged != null)
        {
            OnMagnetActivationChanged.Invoke(isMagnetActive);
        }
    }
}
