using FeedTheBeasts.Scripts;
using UnityEngine;

public class FXSoundsManager : MonoBehaviour
{
    GameCatalog gameCatalog;

 
    void Start()
    {
        gameCatalog = GameCatalog.Instance;
    }

    internal AudioClip GetFXSound(FXTypes fXTypes)
    {
        return gameCatalog.GetFXClip(FXTypes.ClickOnButton);
    }
}
