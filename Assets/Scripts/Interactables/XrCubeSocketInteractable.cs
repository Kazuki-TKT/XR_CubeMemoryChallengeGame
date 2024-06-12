using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class XrCubeSocketInteractable : XRSocketInteractor
{
    // オーディオ類
    //--
    [SerializeField] AudioSource socketSound;

    [SerializeField] AudioClip hoverClip;

    [SerializeField] AudioClip selectClip;

    [SerializeField] AudioClip deselectClip;

    [SerializeField] AudioClip correctClip;

    [SerializeField] AudioClip missClip;
    //--

    [SerializeField] TextMeshPro answerText; // 正答を表示するテキスト

    //--
    [SerializeField] CubeColorType socketedCubeColor; // 
    public CubeColorType GetSocketedCubeColor => socketedCubeColor;
    //--

    protected override void Start()
    {
        base.Start();
        hoverEntered.AddListener(OnMyHoverEvent);
        selectEntered.AddListener(OnMySelectEntered);
        selectExited.AddListener(OnMySelectExited);
    }

    // ホバー状態になった時
    private void OnMyHoverEvent(HoverEnterEventArgs arg0)
    {
        PlaySound(hoverClip);
    }

    // 選択状態になった時
    private void OnMySelectEntered(SelectEnterEventArgs arg0)
    {
        PlaySound(selectClip);
        if (arg0.interactableObject.transform.TryGetComponent(out XrGrabCubeInteractable xrGrabCube))
        {
            socketedCubeColor = xrGrabCube.GetColorType;
            Debug.Log($"***{socketedCubeColor}***");
        }
    }

    // 選択状態じゃなくなった時
    private void OnMySelectExited(SelectExitEventArgs arg0)
    {
        PlaySound(deselectClip);
    }

    // 正解
    public void Correct()
    {
        if (answerText != null)
        {
            answerText.text = "○";
            answerText.color = Color.red;
        }
        PlaySound(correctClip);
    }

    // 不正解
    public void Miss()
    {
        if (answerText != null)
        {
            answerText.text = "×";
            answerText.color = Color.blue;
        }
        PlaySound(missClip);
    }

    // socketSoundでclipを鳴らす
    void PlaySound(AudioClip clip)
    {
        if (clip != null && socketSound != null)
        {
            if (socketSound.isActiveAndEnabled) socketSound.PlayOneShot(clip);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        socketSound.Stop();
    }
}
