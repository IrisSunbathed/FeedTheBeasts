using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace FeedTheBeasts.Scripts
{
    public class ScoreUIManager : MonoBehaviour
    {




        [Header("Score configuration")]
        [SerializeField] TMP_Text txtRoundModH;
        [SerializeField] TMP_Text txtScoreH;
        [SerializeField] internal TMP_Text txtScore;
        [SerializeField] TMP_Text extraScoreH;
        [SerializeField] internal TMP_Text extraScore;
        [SerializeField] TMP_Text scoreMultH;
        [SerializeField] internal TMP_Text scoreMult;
        internal int currentExtraScore;

        [Header("Testing")]
        [SerializeField] TMP_InputField inpExtra;
        [SerializeField] TMP_InputField inpMult;
        [SerializeField] Button bttTest;
        [Header("Score effect configuration")]


        [SerializeField, Range(0.5f, 2.5f)] float sizeToAdd;
        [SerializeField, Range(0.25f, .75f)] float increaseSizePerFrame;
        [SerializeField] FXSoundsManager fXSoundsManager;
        [SerializeField] CalculateScoreHelper calculateScoreHelper;

        internal bool IsScoreCalc { get; set; }
        float originalSize;



        void Awake()
        {
            Assert.IsNotNull(txtRoundModH, "ERROR: txtRoundModH is empty on UIManager");
            Assert.IsNotNull(txtScoreH, "ERROR: txtScoreStr is empty on UIManager");
            Assert.IsNotNull(txtScore, "ERROR: txtScore is empty on UIManager");
            Assert.IsNotNull(extraScoreH, "ERROR: extraScoreH is empty on UIManager");
            Assert.IsNotNull(extraScore, "ERROR: extraScore is empty on UIManager");
            Assert.IsNotNull(scoreMultH, "ERROR: extraScoreH is empty on UIManager");
            Assert.IsNotNull(scoreMult, "ERROR: extraScoreH is empty on UIManager");
            Assert.IsNotNull(calculateScoreHelper, "ERROR: calculateScoreHelper is empty on UIManager");
            Assert.IsNotNull(fXSoundsManager, "ERROR: fXSoundsManager game object is empty on UIManager");

            originalSize = txtScore.fontSizeMax;
            IsScoreCalc = true;

            RectTransform rectTranform = txtScore.GetComponent<RectTransform>();
            calculateScoreHelper.Configure(rectTranform);

            bttTest.onClick.AddListener(TestScore);

            Init();
        }

        private void TestScore()
        {
            if (int.TryParse(inpExtra.text, out int extraScore) & int.TryParse(inpMult.text, out int mult))
            {
                currentExtraScore = extraScore;
                scoreMult.text = mult.ToString();
                calculateScoreHelper.Calculate(mult);

            }

        }

        internal void Init()
        {
            txtScore.text = 0.ToString();
            currentExtraScore = 0;
            ActivateElementsOnMenu(false);

        }
        // Update is called once per frame


        internal void ActivateElementsOnMenu(bool isActive)
        {
            txtScoreH.gameObject.SetActive(isActive);
            txtScore.gameObject.SetActive(isActive);
            scoreMultH.gameObject.SetActive(isActive);
            scoreMult.gameObject.SetActive(isActive);
            extraScoreH.gameObject.SetActive(isActive);
            extraScore.gameObject.SetActive(isActive);
            txtRoundModH.gameObject.SetActive(isActive);


        }

        internal void ManageScore(int score)
        {
            txtScore.text = score.ToString();

            if (score > 0)
            {
                StartCoroutine(ScoreTextEffect());
            }
        }

        IEnumerator ScoreTextEffect()
        {
            float currentSizeToAdd = sizeToAdd;
            while (txtScore.fontSizeMax <= originalSize + currentSizeToAdd)
            {
                txtScore.fontSizeMax += increaseSizePerFrame;
                yield return null;
            }

            while (txtScore.fontSizeMax >= originalSize)
            {
                txtScore.fontSizeMax -= increaseSizePerFrame;
                yield return null;
            }
            txtScore.fontSizeMax = originalSize;
        }

        internal void AddMultiplayer(int scoreMult)
        {
            this.scoreMult.text = $"x{scoreMult}";
        }

        internal void CalculatePoints(int multiplayer)
        {
            calculateScoreHelper.Calculate(multiplayer);
        }
     
        internal void AddSum(int consecutiveShootsCurrent)
        {
            currentExtraScore += consecutiveShootsCurrent;

            extraScore.text = $"+{currentExtraScore}";
        }
    }

}