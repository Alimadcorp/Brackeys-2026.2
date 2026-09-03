using UnityEngine;
using UnityEngine.Events;

public class DelayedExec : MonoBehaviour
{
    public UnityEvent Event;
    public bool startOnStart;
    public float delay;

    public void Execute() { Execute(delay); }
    
    public void Execute(float _delay)
    {
        Invoke(nameof(InvokeEvent), _delay);
    }

    private void InvokeEvent()
    {
        Event.Invoke();
    }

    private void Start()
    {
        Invoke(nameof(Execute), 0);
    }
}