using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region Variables

    [Header("Tutorial PopUps")]
    public GameObject[] popUps;
    private int popUpIndex;
    public InputManager input;

    [SerializeField] private bool canMoveForward;
    [SerializeField] private bool canMoveInCyclic;
    [SerializeField] private bool canMoveLeftRight;

    #endregion
    private void Update()
    {
        ManagePopUps();
    }

    private void ManagePopUps(){
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(i == popUpIndex);
        }

        switch (popUpIndex)
        {
            case 0:
                if (Mathf.Abs(input.Throttle) > 0.001f)
                {
                    popUpIndex++;
                    canMoveForward = true;
                }
                break;

            case 1:
                if (canMoveForward && Mathf.Abs(input.Cyclic.y) > 0.001f)
                {
                    popUpIndex++;
                    canMoveInCyclic = true;
                }
                break;

            case 2:
                if (canMoveInCyclic && Mathf.Abs(input.Pedals) > 0.001f)
                {
                    popUpIndex++;
                    canMoveLeftRight = true;
                }
                break;

            case 3:
                if (canMoveLeftRight && Mathf.Abs(input.Cyclic.x) > 0.001f)
                {
                    popUpIndex++;
                }
                break;
        }
    }

    #region getters
    public bool CanMoveForward()
    {
        return canMoveForward;
    }

    public bool CanMoveInCyclic()
    {
        return canMoveInCyclic;
    }

    public bool CanMoveLeftRight()
    {
        return canMoveLeftRight;
    }

    #endregion
}
