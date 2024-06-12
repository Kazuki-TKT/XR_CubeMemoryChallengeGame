using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// �F�̃^�C�v�̗񋓌^
public enum CubeColorType
{
    Red, // ��
    Green, // ��
    Blue, // ��
    Yellow, // ��
    Purple, // ��
}

public class XrGrabCubeInteractable : XRGrabInteractable
{
    //--
    [SerializeField] CubeColorType colorType; // �F�̃^�C�v
    public CubeColorType GetColorType => colorType;
    //--

    [SerializeField] Material defaultMaterial; // �f�t�H���g�̃}�e���A��

    [SerializeField] Material emissionMaterial; // ���\���̃}�e���A���@*�Z���N�g���Ɏg�p

    void Start()
    {
        if (gameObject.TryGetComponent(out Renderer renderer))
        {
            if (renderer.material != defaultMaterial) renderer.material = defaultMaterial;
        }
    }

    // �}�e���A�����Z�b�g����
    void SetMaterial(Material material)
    {
        gameObject.GetComponent<Renderer>().material = material;
    }

    // �Z���N�g��ԂɂȂ�����
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        SetMaterial(emissionMaterial);
    }

    // �Z���N�g��ԂłȂ��Ȃ�����
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        SetMaterial(defaultMaterial);
    }
}
