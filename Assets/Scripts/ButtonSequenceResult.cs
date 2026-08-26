using UnityEngine;

public class ButtonSequenceResult : MonoBehaviour
{
    public bool sequenceSolved = false;

    public void OnSequenceSuccess()
    {
        sequenceSolved = true;
        Debug.Log("Sequence solved! Lift activation goes here later.");
    }
}