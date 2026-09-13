using UnityEngine;
using UnityEngine.UI;

// Drives the "Deaths: N" label in the top-right corner of Level_5. Resets the
// shared PlayerController.DeathCount to 0 when this level loads, then keeps
// the label in sync with it every frame (it's incremented from PlayerController.Die(),
// and directly from Spike.cs / BoundaryReset.cs for Echo deaths).
public class DeathCounterUI : MonoBehaviour
{
    private Text label;
    private int lastShown = -1;

    void Awake()
    {
        label = GetComponent<Text>();
        PlayerController.DeathCount = 0;
    }

    void Update()
    {
        if (PlayerController.DeathCount != lastShown)
        {
            lastShown = PlayerController.DeathCount;
            label.text = "Deaths: " + lastShown;
        }
    }
}
