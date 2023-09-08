using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    #region Variables

    private Vector2 _cyclic;
    private float _pedals;
    private float _throttle;

    private float _loaded;
    private float _skipTutorial;


    public Vector2 Cyclic => _cyclic;
    public float Pedals => _pedals;
    public float Throttle => _throttle;
    public float Loaded => _loaded;
    public float SkipTutorial => _skipTutorial;

    #endregion

    #region Main Methods

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Input Methods

    private void OnCyclic(InputValue value)
    {
        _cyclic = value.Get<Vector2>();

    }

    private void OnPedals(InputValue value)
    {
        _pedals = value.Get<float>();
    }

    private void OnThrottle(InputValue value)
    {
        _throttle = value.Get<float>();
    }

    private void OnLoadCargo(InputValue value){
        _loaded = value.Get<float>();
    }

    private void OnSkipTut(InputValue value){
        _skipTutorial = value.Get<float>();
        Debug.Log("Skip tutorial: " + _skipTutorial);
    }

    #endregion
}
