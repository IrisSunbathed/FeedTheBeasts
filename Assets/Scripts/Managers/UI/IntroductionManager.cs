using System;
using System.Collections;
using FeedTheBeasts.Scripts;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class IntroductionManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] txtIntroduction;
    [SerializeField] MenuUI uiMenu;

    [SerializeField] UIManager uIManager;
    [SerializeField] GameObject background;
    [SerializeField] float timeBetweenTexts;
    [SerializeField] float timeAudioFading;
    [SerializeField] MusicManager musicManager;

    CamerasManager camerasManager;
    int index;

    void Start()
    {
        camerasManager = CamerasManager.Instance;
        camerasManager.SwitchCameras(isGameplayCamera: true);
    }
    void Awake()
    {
        Assert.IsNotNull(uIManager, "ERROR: uIManager is not added");
        Assert.IsNotNull(musicManager, "ERROR: musicManager is not added");
        Assert.IsTrue(txtIntroduction.Length > 0, "ERROR: txtIntroduction is empty");

        index = 0;
        Init();
    }

    private void Init()
    {

        background.SetActive(true);
        foreach (var item in txtIntroduction)
        {

            Color temp = item.color;
            temp.a = 0f;
            item.color = temp;
        }
        StartCoroutine(TextEffectCourutine());
        Debug.Log(index);
        uiMenu.SetActiveUIElements(false);
    }

    IEnumerator TextEffectCourutine()
    {
        Debug.Log(index);
        Color originalColor = txtIntroduction[index].color; //temp
        float temp_a = originalColor.a; //okay
        Color temp = txtIntroduction[index].color;
        while (txtIntroduction[index].color.a <= 1f)
        {
            temp_a += 0.007f;
            temp.a = temp_a;
            txtIntroduction[index].color = temp;
            yield return null;
        }
        temp_a = 1f;
        temp.a = temp_a;
        txtIntroduction[index].color = temp;

        yield return new WaitForSeconds(timeBetweenTexts);
        Debug.Log($"index: {index} < txtIntroduction.Length {txtIntroduction.Length}");
        if (index < txtIntroduction.Length - 1)
        {
            index++;
            StartCoroutine(TextEffectCourutine());

        }
        else
        {
            camerasManager.SwitchCameras(isGameplayCamera: false);
            background.SetActive(false);
            foreach (var item in txtIntroduction)
            {
                item.gameObject.SetActive(false);
            }
            musicManager.FadeCurrentMusic(0, timeAudioFading);
            yield return new WaitForSeconds(timeAudioFading);
            uIManager.Init();
        }
    }
}
