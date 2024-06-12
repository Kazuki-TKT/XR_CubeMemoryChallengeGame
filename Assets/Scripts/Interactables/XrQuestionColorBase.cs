using System.Collections.Generic;
using UnityEngine;

public class XrQuestionColorBase : MonoBehaviour
{
    //--
    [SerializeField] List<GameObject> cubeSpawnPoint = new(); // キューブの生成先のオブジェクトのリスト
    public List<GameObject> GetCubeSpawnPoint => cubeSpawnPoint;
    //--

    [SerializeField] GameObject redCubePrefab; // 赤のキューブプレファブオブジェクト

    [SerializeField] GameObject blueCubePrefab; // 青のキューブプレファブオブジェクト

    [SerializeField] GameObject greenCubePrefab; // 緑のキューブプレファブオブジェクト

    [SerializeField] GameObject yellowCubePrefab; // 黄のキューブプレファブオブジェクト

    [SerializeField] GameObject purpleCubePrefab; // 紫のキューブプレファブオブジェクト

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            GameObject targetObject = child.GetChild(0).gameObject; // 子オブジェクトの取得
            cubeSpawnPoint.Add(targetObject);
        }
    }

    // キューブをオブジェクトのリストの数だけ作成する
    public void CreateQuestionCubesForColors(List<CubeColorType> cubeColorType)
    {
        if (cubeColorType.Count == 0 || cubeColorType == null) return; // リストの値がないとリターン
        for (int i = 0; i < cubeSpawnPoint.Count; i++)
        {
            CreateCubesForColors(cubeSpawnPoint[i], cubeColorType[i]);
        }
    }

    // 対応したキューブを作成する
    void CreateCubesForColors(GameObject targetObject, CubeColorType cubeColorType)
    {
        switch (cubeColorType)
        {
            case CubeColorType.Red:
                Instantiate(redCubePrefab, targetObject.transform.position, targetObject.transform.rotation).transform.SetParent(transform);
                break;
            case CubeColorType.Green:
                Instantiate(greenCubePrefab, targetObject.transform.position, targetObject.transform.rotation).transform.SetParent(transform);
                break;
            case CubeColorType.Blue:
                Instantiate(blueCubePrefab, targetObject.transform.position, targetObject.transform.rotation).transform.SetParent(transform);
                break;
            case CubeColorType.Yellow:
                Instantiate(yellowCubePrefab, targetObject.transform.position, targetObject.transform.rotation).transform.SetParent(transform);
                break;
            case CubeColorType.Purple:
                Instantiate(purpleCubePrefab, targetObject.transform.position, targetObject.transform.rotation).transform.SetParent(transform);
                break;
        }
    }
}
