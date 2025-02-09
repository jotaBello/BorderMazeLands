using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    void Start()
    {
        if (MusicManager.Instance == null)
        {
            MusicManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
