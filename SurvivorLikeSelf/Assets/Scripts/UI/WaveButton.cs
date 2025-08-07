using UnityEngine;

public class WaveButton : MonoBehaviour
{
    public void StartNextWave()
    {
        EventManager.TriggerEvent("StartWave", null);
    }
}
