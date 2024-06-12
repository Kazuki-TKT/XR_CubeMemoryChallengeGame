using System.Collections.Generic;
using UnityEngine;

public class XrQuestionColorBase : MonoBehaviour
{
    //--
    [SerializeField] List<GameObject> cubeSpawnPoint = new(); // �L���[�u�̐�����̃I�u�W�F�N�g�̃��X�g
    public List<GameObject> GetCubeSpawnPoint => cubeSpawnPoint;
    //--

    [SerializeField] GameObject redCubePrefab; // �Ԃ̃L���[�u�v���t�@�u�I�u�W�F�N�g

    [SerializeField] GameObject blueCubePrefab; // �̃L���[�u�v���t�@�u�I�u�W�F�N�g

    [SerializeField] GameObject greenCubePrefab; // �΂̃L���[�u�v���t�@�u�I�u�W�F�N�g

    [SerializeField] GameObject yellowCubePrefab; // ���̃L���[�u�v���t�@�u�I�u�W�F�N�g

    [SerializeField] GameObject purpleCubePrefab; // ���̃L���[�u�v���t�@�u�I�u�W�F�N�g

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            GameObject targetObject = child.GetChild(0).gameObject; // �q�I�u�W�F�N�g�̎擾
            cubeSpawnPoint.Add(targetObject);
        }
    }

    // �L���[�u���I�u�W�F�N�g�̃��X�g�̐������쐬����
    public void CreateQuestionCubesForColors(List<CubeColorType> cubeColorType)
    {
        if (cubeColorType.Count == 0 || cubeColorType == null) return; // ���X�g�̒l���Ȃ��ƃ��^�[��
        for (int i = 0; i < cubeSpawnPoint.Count; i++)
        {
            CreateCubesForColors(cubeSpawnPoint[i], cubeColorType[i]);
        }
    }

    // �Ή������L���[�u���쐬����
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
