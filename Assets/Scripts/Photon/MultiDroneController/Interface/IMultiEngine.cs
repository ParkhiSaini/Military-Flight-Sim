using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultiEngine
{
    void InitEngine();
    void UpdateEngine(Rigidbody rb, InputManager inputs);
}