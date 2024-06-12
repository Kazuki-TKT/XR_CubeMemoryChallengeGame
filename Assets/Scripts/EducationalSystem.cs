using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��Փx�̗񋓌^
public enum LevelType
{
    Easy,// �ȒP
    Normal,// ����
    Hard//�@���
}

public class EducationalSystem : MonoBehaviour
{
    const string Socket_Base_Tag = "SocketBase";

    const string Cube_Tag = "Cube";

    private Coroutine playGameColoutin; // PlayGameCoroutin���i�[����Coroutin�^

    //--
    [SerializeField] LevelType currentLevelType; // ���݂̑I�����x��

    Dictionary<LevelType, int> levlToQuestionCount = new Dictionary<LevelType, int>()
    {
        {LevelType.Easy, 4},
        {LevelType.Normal, 8},
        {LevelType.Hard, 12},
    }; // ���x���Ɩ��̐������������f�B�N�V���i���[

    public void SetEasyLevel() => currentLevelType = LevelType.Easy; // ��Փx�u�ȒP�v�ɂ��郁�\�b�h *OnClick�ɓo�^

    public void SetNormalLevel() => currentLevelType = LevelType.Normal; // ��Փx�u���ʁv�ɂ��郁�\�b�h *OnClick�ɓo�^

    public void SetHardLevel() => currentLevelType = LevelType.Hard; // ��Փx�u����v�ɂ��郁�\�b�h *OnClick�ɓo�^
    //--

    [SerializeField] bool checkAnswer; // ���̃`�F�b�N�Ɏg�p����^���l

    GameObject currentQuesitionColorBase; // ���݂̖��̃I�u�W�F�N�g���i�[����I�u�W�F�N�g

    GameObject currentCubeSocketBase; // ���݂̃\�P�b�g���܂Ƃ߂��I�u�W�F�N�g���i�[����I�u�W�F�N�g

    public List<QuesitonLevel> questionLevels = new List<QuesitonLevel>(); // ���̃��x���ʂ̃��X�g

    [SerializeField] GameObject targetSpawnObject; // ���𐶐�����^�[�Q�b�g�̃I�u�W�F�N�g

    [SerializeField] List<CubeColorType> questionColors; // ���̐F�̃��X�g

    [SerializeField] List<XrCubeSocketInteractable> cubeSockets = new List<XrCubeSocketInteractable>(); // �L���[�u���n����\�P�b�g�̃��X�g

    [SerializeField] GameObject setGenerateCubeObject; // �v���C���[���͂ނ��Ƃ̂ł���L���[�u�ނ��܂Ƃ߂��I�u�W�F�N�g

    [SerializeField] GameObject chaeckButtonObject; // �v���C���[�̉񓚂��`�F�b�N����{�^���̃I�u�W�F�N�g

    [SerializeField] GameObject rePlayPanelObject; // �v���C��ɕ\������p�l���������I�u�W�F�N�g

    [Header("�T�E���h��")]

    [SerializeField] AudioSource educationalSound;

    [SerializeField] AudioSource timerSound;

    [SerializeField] AudioClip startClip;

    [SerializeField] AudioClip clearClip;

    [SerializeField] AudioClip perfectClearClip;

    [Header("�^�C�}�[�֌W")]

    [SerializeField] GameObject TimerObject; // �^�C�}�[�������I�u�W�F�N�g

    [SerializeField] Text timerText; // �^�C�}�[�̎��Ԃ�\������e�L�X�g

    public float timerDuration = 10f;// �^�C�}�[�̎���

    bool timerExit = false; // �^�C�}�[���΂����Ɏg�p����^���l

    public void TimerExit() => timerExit = true; // �^�C�}�[���΂����\�b�h *OnClick�ɓo�^


    // �Q�[�����s�����\�b�h
    public void PlayGame()
    {
        playGameColoutin = StartCoroutine(PlayGameCoroutin());
    }

    // �Q�[�����~�߂郁�\�b�h
    public void StopGame()
    {
        if (playGameColoutin != null) StopCoroutine(playGameColoutin);
    }

    // �Q�[�����s���R���[�`��
    IEnumerator PlayGameCoroutin()
    {
        CreateQuestionColorsList(); // ���̐F�̃��X�g���쐬
        CreateQuesitionColorBaseAndSetColors(); // �L���[�u�𐶐����F��ݒ�
        yield return StartCoroutine(TimerCoroutine()); // �^�C�}�[�R���[�`�����X�^�[�g
        educationalSound.PlayOneShot(startClip);
        checkAnswer = false;
        chaeckButtonObject.SetActive(true);
        DestroyQuestionSocket();
        CreateQuestionSocketBase();
        if (setGenerateCubeObject != null) setGenerateCubeObject.SetActive(true);
    }

    // �^�C�}�[�R���[�`��
    IEnumerator TimerCoroutine()
    {
        TimerObject.SetActive(true);
        float remainingTime = timerDuration;
        timerSound.Play();
        timerExit = false;
        while (remainingTime > 0 && timerExit == false) // �c���Ԃ�0���ォ��timerExit��flase
        {
            remainingTime -= Time.deltaTime;
            int displayTime = Mathf.CeilToInt(remainingTime);
            timerText.text = displayTime.ToString();
            yield return null;
        }
        timerSound.Stop();
        TimerObject.SetActive(false);
    }

    // ���ƂȂ�F�̃��X�g�쐬
    void CreateQuestionColorsList()
    {
        questionColors.Clear();
        // ���݂̃��x���ɉ�������萔���f�B�N�V���i���[����擾
        if (levlToQuestionCount.TryGetValue(currentLevelType, out int count))
        {
            for (int i = 0; i < count; i++)
            {
                questionColors.Add(GetRandomEnumValue<CubeColorType>());
            }
        }
        else
        {
            Debug.LogError($"���Ή��̃��x���^�C�v: {currentLevelType}");
        }
    }

    // ���̃x�[�X�ƂȂ�I�u�W�F�N�g���쐬���A�F���X�g�̒l���Z�b�g����
    void CreateQuesitionColorBaseAndSetColors()
    {
        currentQuesitionColorBase = Instantiate(GetQuestionLevelColorsBaseObject(), targetSpawnObject.transform.position, transform.rotation);
        currentQuesitionColorBase.transform.SetParent(targetSpawnObject.transform);
        var xrQuesitionColorBase = currentQuesitionColorBase.GetComponent<XrQuestionColorBase>();
        xrQuesitionColorBase.CreateQuestionCubesForColors(questionColors);
    }

    // �\�P�b�g�̃x�[�X�ƂȂ�I�u�W�F�N�g�𐶐�
    void CreateQuestionSocketBase()
    {
        cubeSockets.Clear();
        currentCubeSocketBase = Instantiate(GetQuestionLevelSocketsBaseObject(), targetSpawnObject.transform.position, transform.rotation);
        if (currentCubeSocketBase != null)
        {
            currentCubeSocketBase.transform.SetParent(targetSpawnObject.transform);
            if (currentCubeSocketBase.TryGetComponent(out XrCubeSocketsBase xrCubeSocketsBase))
            {
                cubeSockets = xrCubeSocketsBase.GetCubeSocketsBase;
            }
        }
    }

    // ���݂̃��x���ɂ����������i�[����I�u�W�F�N�g��Ԃ�
    GameObject GetQuestionLevelColorsBaseObject()
    {
        QuesitonLevel level = questionLevels.Find(level => level.GetLevelType == currentLevelType);
        return level.GetQuestionColorObjects;
    }

    // ���݂̃��x���ɂ������\�P�b�g�I�u�W�F�N�g��Ԃ�
    GameObject GetQuestionLevelSocketsBaseObject()
    {
        QuesitonLevel level = questionLevels.Find(level => level.GetLevelType == currentLevelType);
        return level.GetQuestionSocketObjects;
    }

    // �����̔�����s���@
    public void CheckPlayAnswer()
    {
        if (checkAnswer) return;
        setGenerateCubeObject.SetActive(false);
        StartCoroutine(CheckCubeColors());
        checkAnswer = true;
    }

    // �����̔���̃R���[�`��
    IEnumerator CheckCubeColors()
    {
        bool perfect = true; // �S�␳�����ǂ���
        for (int i = 0; i < cubeSockets.Count; i++)
        {
            if (cubeSockets[i].hasSelection) // �\�P�b�g�ɃI�u�W�F�N�g���n�}���Ă���ꍇ
            {
                if (questionColors[i] == cubeSockets[i].GetSocketedCubeColor) // ����
                {
                    cubeSockets[i].Correct();
                }
                else // �s����
                {
                    cubeSockets[i].Miss();
                    perfect = false;
                }
            }
            else // �\�P�b�g�ɃI�u�W�F�N�g���n�}���Ă��Ȃ��ꍇ��Miss
            {
                cubeSockets[i].Miss();
                perfect = false;
            }
            yield return new WaitForSeconds(0.7f);
        }

        // �������ɉ����ď����𕪊�
        if (perfect) // �S�␳��
        {
            PlaySound(perfectClearClip);
        }
        else
        {
            PlaySound(clearClip);
        }

        yield return new WaitForSeconds(2);

        // ������x�V�Ԃ��ǂ����̏���
        if (rePlayPanelObject != null)
        {
            DestroyCubeObject();
            DestroyQuestionSocket();
            rePlayPanelObject.SetActive(true);
        }
    }

    // �w�肵���^�O�̃I�u�W�F�N�g���V�[������T���S�č폜
    void FindTagToDestroy(string tag)
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tag);
        if (objectsToDestroy.Length > 0)
        {
            foreach (GameObject obj in objectsToDestroy)
            {
                DestroyImmediate(obj);
            }
        }
        else
        {
            Debug.Log($"�w�肵���^�O: {tag} �͌�����܂���ł���");
        }
    }

    // �\�P�b�g�^�O�̃I�u�W�F�N�g���폜
    void DestroyQuestionSocket() => FindTagToDestroy(Socket_Base_Tag);

    // �L���[�u�^�O�̃I�u�W�F�N�g���폜
    void DestroyCubeObject() => FindTagToDestroy(Cube_Tag);

    // �����_����Enum�̒l��Ԃ�
    public TEnum GetRandomEnumValue<TEnum>()
    {
        TEnum[] values = (TEnum[])System.Enum.GetValues(typeof(TEnum));
        int randomIndex = Random.Range(0, values.Length);
        return values[randomIndex];
    }

    // �����̃N���b�v�̉���炷
    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            educationalSound.PlayOneShot(clip);
        }
    }

    // �Q�[�����I��������
    public void Quit()
    {
        // �A�v���P�[�V�������I��
        Application.Quit();

        // �G�f�B�^�[���Ŏ��s���̏ꍇ�A�G�f�B�^�[����~
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

[System.Serializable]
public class QuesitonLevel
{
    //--
    [SerializeField] LevelType levelType;
    public LevelType GetLevelType => levelType;
    //--

    //--
    [SerializeField] GameObject questionSocketObjects;
    public GameObject GetQuestionSocketObjects => questionSocketObjects;
    //--

    //--
    [SerializeField] GameObject questionColorObjects;
    public GameObject GetQuestionColorObjects => questionColorObjects;
    //--
}