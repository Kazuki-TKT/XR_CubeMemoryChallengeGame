using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class XrCubeSocketInteractable : XRSocketInteractor
{
    // �I�[�f�B�I��
    //--
    [SerializeField] AudioSource socketSound;

    [SerializeField] AudioClip hoverClip;

    [SerializeField] AudioClip selectClip;

    [SerializeField] AudioClip deselectClip;

    [SerializeField] AudioClip correctClip;

    [SerializeField] AudioClip missClip;
    //--

    [SerializeField] TextMeshPro answerText; // ������\������e�L�X�g

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

    // �z�o�[��ԂɂȂ�����
    private void OnMyHoverEvent(HoverEnterEventArgs arg0)
    {
        PlaySound(hoverClip);
    }

    // �I����ԂɂȂ�����
    private void OnMySelectEntered(SelectEnterEventArgs arg0)
    {
        PlaySound(selectClip);
        if (arg0.interactableObject.transform.TryGetComponent(out XrGrabCubeInteractable xrGrabCube))
        {
            socketedCubeColor = xrGrabCube.GetColorType;
            Debug.Log($"***{socketedCubeColor}***");
        }
    }

    // �I����Ԃ���Ȃ��Ȃ�����
    private void OnMySelectExited(SelectExitEventArgs arg0)
    {
        PlaySound(deselectClip);
    }

    // ����
    public void Correct()
    {
        if (answerText != null)
        {
            answerText.text = "��";
            answerText.color = Color.red;
        }
        PlaySound(correctClip);
    }

    // �s����
    public void Miss()
    {
        if (answerText != null)
        {
            answerText.text = "�~";
            answerText.color = Color.blue;
        }
        PlaySound(missClip);
    }

    // socketSound��clip��炷
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
