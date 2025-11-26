using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    
public class LevelManager : MonoBehaviour
{
    static LevelManager instance;
    public static LevelManager Instance => instance;

    public Levels CurrentLevel { get; set; }


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            CurrentLevel = Levels.Level1;
    }


}

}