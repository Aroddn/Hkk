using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ConfirmationDialog : MonoBehaviour
{
    public TMP_Text messageText;
    public Button yesButton;
    public Button noButton;

    private static ConfirmationDialog instance;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public static void Hide()
    {
        if (instance != null)
        {
            instance.gameObject.SetActive(false);
        }
    }

    public static void Show(string message, Action onYes)
    {
        instance.gameObject.SetActive(true);
        instance.messageText.text = message;

        // Clear previous listeners
        instance.yesButton.onClick.RemoveAllListeners();
        instance.noButton.onClick.RemoveAllListeners();

        instance.yesButton.onClick.AddListener(() =>
        {
            onYes?.Invoke();
            instance.gameObject.SetActive(false);
        });

        instance.noButton.onClick.AddListener(() =>
        {
            instance.gameObject.SetActive(false);
        });
    }
}