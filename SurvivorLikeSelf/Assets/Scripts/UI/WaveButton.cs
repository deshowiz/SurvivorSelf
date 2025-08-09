using UnityEngine;

public class WaveButton : MonoBehaviour
{
    public void StartNextWave()
    {
        EventManager.OnStartWave?.Invoke();
    }
}
