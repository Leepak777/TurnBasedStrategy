// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

    public class GameEventListenerWithVar : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEventWithVar Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<Vector3> Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(Vector3 variable)
        {
            Response.Invoke(variable);
        }
    }
