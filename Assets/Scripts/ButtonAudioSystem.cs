using UnityEngine;
using UnityEngine.UI;

public class ButtonAudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound; // �{�^����p�̃I�[�f�B�I�\�[�X

    [SerializeField] AudioClip clickClip; // �N���b�N��

    [SerializeField] Button[] inGameButtons; // �Q�[���Ŏg�p����{�^���̔z��

    private void Awake()
    {
        // �{�^���̃N���b�N����ݒ�
        foreach (Button button in inGameButtons)
        {
            button.onClick.AddListener(() => buttonSound.PlayOneShot(clickClip));
        }
    }
}
