using System;
using System.Collections;
using FeedTheBeasts.Scripts;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class IntroductionManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] txtIntroduction;
    [SerializeField] MenuUI uiMenu;

    [SerializeField] UIManager uIManager;
    [SerializeField] GameObject background;
    [SerializeField] float timeBetweenTexts;
    [SerializeField] float timeAudioFading;
    [SerializeField] MusicManager musicManager;

    bool canBeClickedAway;

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
        canBeClickedAway = true;
        SetUpTexts();
        StartCoroutine(TextEffectCourutine());
        Debug.Log(index);
        uiMenu.SetActiveUIElements(false);
    }

    private void SetUpTexts()
    {
        foreach (var item in txtIntroduction)
        {

            Color temp = item.color;
            temp.a = 0f;
            item.color = temp;

        }
        txtIntroduction[0].text = Constants.INTRO_TEXT_1;
        txtIntroduction[1].text = Constants.INTRO_TEXT_2;
        txtIntroduction[2].text = Constants.INTRO_TEXT_3;
        txtIntroduction[3].text = Constants.INTRO_TEXT_4;
    }

    IEnumerator TextEffectCourutine()
    {
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
        if (index < txtIntroduction.Length - 1)
        {
            index++;
            StartCoroutine(TextEffectCourutine());

        }
        else
        {
            StartCoroutine(FadeInAndOut());
        }
    }
    IEnumerator FadeInAndOut()
    {
        Color originalColor = txtIntroduction[^1].color; //temp
        float temp_a = originalColor.a; //okay
        Color temp = txtIntroduction[^1].color;
        while (txtIntroduction[^1].color.a >= 0f)
        {
            temp_a -= 0.003f;
            temp.a = temp_a;
            txtIntroduction[^1].color = temp;
            yield return null;
        }
        temp_a = 0f;
        temp.a = temp_a;
        txtIntroduction[^1].color = temp;
        yield return new WaitForSeconds(.5f);

        while (txtIntroduction[^1].color.a <= 1f)
        {
            temp_a += 0.003f;
            temp.a = temp_a;
            txtIntroduction[^1].color = temp;
            yield return null;
        }
        temp_a = 1f;
        temp.a = temp_a;
        txtIntroduction[^1].color = temp;
        txtIntroduction[^1].color = temp;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(FadeInAndOut());


    }
    IEnumerator FadeOutTexts(int index)
    {
        Color originalColor = txtIntroduction[index].color; //temp
        float temp_a = originalColor.a; //okay
        Color temp = txtIntroduction[index].color;
        while (txtIntroduction[index].color.a >= 0f)
        {
            temp_a -= 0.007f;
            temp.a = temp_a;
            txtIntroduction[index].color = temp;
            yield return null;
        }
        temp_a = 1f;
        temp.a = temp_a;
        yield return null;

    }
    IEnumerator FadeOutBackground()
    {
        Image backgroundimg = background.GetComponent<Image>();
        Color originalColor = backgroundimg.color; //temp
        float temp_a = originalColor.a; //okay
        Color temp = backgroundimg.color;
        while (backgroundimg.color.a >= 0f)
        {
            temp_a -= 0.007f;
            temp.a = temp_a;
            backgroundimg.color = temp;
            yield return null;
        }
        temp_a = 1f;
        temp.a = temp_a;
        yield return null;
    }


    private void Update()
    {
        if (canBeClickedAway && Input.GetMouseButtonDown(0))
        {
               canBeClickedAway = false;
            StartCoroutine(MainMenuTransitionCoroutine());

        }
    }

    IEnumerator MainMenuTransitionCoroutine()
    {
     
        index = 0;
        for (int i = 0; i < txtIntroduction.Length; i++)
        {
            StartCoroutine(FadeOutTexts(i));
        }
        StartCoroutine(FadeOutBackground());
        yield return new WaitForSeconds(2f);
        Debug.Log("Introduction manager");
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
