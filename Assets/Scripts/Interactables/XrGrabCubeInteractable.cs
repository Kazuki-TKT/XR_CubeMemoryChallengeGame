using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 色のタイプの列挙型
public enum CubeColorType
{
    Red, // 赤
    Green, // 緑
    Blue, // 青
    Yellow, // 黄
    Purple, // 紫
}

public class XrGrabCubeInteractable : XRGrabInteractable
{
    //--
    [SerializeField] CubeColorType colorType; // 色のタイプ
    public CubeColorType GetColorType => colorType;
    //--

    [SerializeField] Material defaultMaterial; // デフォルトのマテリアル

    [SerializeField] Material emissionMaterial; // 光表現のマテリアル　*セレクト時に使用

    void Start()
    {
        if (gameObject.TryGetComponent(out Renderer renderer))
        {
            if (renderer.material != defaultMaterial) renderer.material = defaultMaterial;
        }
    }

    // マテリアルをセットする
    void SetMaterial(Material material)
    {
        gameObject.GetComponent<Renderer>().material = material;
    }

    // セレクト状態になった時
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        SetMaterial(emissionMaterial);
    }

    // セレクト状態でなくなった時
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        SetMaterial(defaultMaterial);
    }
}
