using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text;
using System;
using TMPro;

public class LevelRestartWindow : MonoBehaviour
{
    public CanvasGroup m_CanvasGroup;

    public StringBuilder m_LevelText = new StringBuilder();

    public Button m_OnRestartButton;

    public RectTransform m_WindowArea;

    public TextMeshProUGUI m_MessageText;

    public GameObject m_LayoutArea;

    private void Awake()
    {
        GameManager.OnGameOver += ShowPage;
    }
    public void ShowPage(string message)
    {
        m_LevelText.Length = 0;

        m_LevelText.Append(message);

        m_MessageText.text = m_LevelText.ToString();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(m_CanvasGroup.DOFade(1, 0.5f));
        sequence.Join(m_WindowArea.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutElastic));
        sequence.OnStart(() =>
        {
            m_LayoutArea.SetActive(true);
        });

        sequence.OnComplete(() =>
        {
            m_OnRestartButton.onClick.AddListener(() =>
            {
                HidePage();
            });
        });
    }

    private void HidePage()
    {
        m_OnRestartButton.onClick.RemoveAllListeners();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(m_CanvasGroup.DOFade(0, 0.5f));
        sequence.Join(m_WindowArea.DOAnchorPosY(2000, 0.5f));

        sequence.OnStart(() =>
        {
            GameManager.Instance.ResetLevel();
        });
        sequence.OnComplete(() =>
        {
            m_LayoutArea.SetActive(false);
        });
    }
}

