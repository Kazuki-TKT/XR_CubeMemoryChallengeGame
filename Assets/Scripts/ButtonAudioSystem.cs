using UnityEngine;
using UnityEngine.UI;

public class ButtonAudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound; // ボタン専用のオーディオソース

    [SerializeField] AudioClip clickClip; // クリック音

    [SerializeField] Button[] inGameButtons; // ゲームで使用するボタンの配列

    private void Awake()
    {
        // ボタンのクリック音を設定
        foreach (Button button in inGameButtons)
        {
            button.onClick.AddListener(() => buttonSound.PlayOneShot(clickClip));
        }
    }
}
