using UnityEngine;

public class CreatePlayerUseGrabCube : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab; // キューブのプレファブオブジェクト

    [SerializeField] GameObject currentCube; // 現在扱っているキューブ

    [SerializeField] AudioSource generateSound; // キューブの生成時に扱うオーディオソース

    private void OnEnable()
    {
        if (transform.childCount == 0)
        {
            CreateCube();
        }
    }

    private void Update()
    {
        if (currentCube == null) return;
        // 指定領域を超えた時にキューブを作成
        if (currentCube.transform.position.x > transform.position.x + 0.13f ||
            currentCube.transform.position.x < transform.position.x - 0.13f)
        {
            currentCube = null;
            generateSound.Play();
            CreateCube();
        }
    }

    // キューブを作成する
    void CreateCube()
    {
        currentCube = Instantiate(cubePrefab, transform.position, transform.rotation);
        currentCube.transform.SetParent(transform);
    }
}
