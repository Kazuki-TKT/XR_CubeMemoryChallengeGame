using UnityEngine;

public class CreatePlayerUseGrabCube : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab; // �L���[�u�̃v���t�@�u�I�u�W�F�N�g

    [SerializeField] GameObject currentCube; // ���݈����Ă���L���[�u

    [SerializeField] AudioSource generateSound; // �L���[�u�̐������Ɉ����I�[�f�B�I�\�[�X

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
        // �w��̈�𒴂������ɃL���[�u���쐬
        if (currentCube.transform.position.x > transform.position.x + 0.13f ||
            currentCube.transform.position.x < transform.position.x - 0.13f)
        {
            currentCube = null;
            generateSound.Play();
            CreateCube();
        }
    }

    // �L���[�u���쐬����
    void CreateCube()
    {
        currentCube = Instantiate(cubePrefab, transform.position, transform.rotation);
        currentCube.transform.SetParent(transform);
    }
}
