using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 難易度の列挙型
public enum LevelType
{
    Easy,// 簡単
    Normal,// 普通
    Hard//　難しい
}

public class EducationalSystem : MonoBehaviour
{
    const string Socket_Base_Tag = "SocketBase";

    const string Cube_Tag = "Cube";

    private Coroutine playGameColoutin; // PlayGameCoroutinを格納するCoroutin型

    //--
    [SerializeField] LevelType currentLevelType; // 現在の選択レベル

    Dictionary<LevelType, int> levlToQuestionCount = new Dictionary<LevelType, int>()
    {
        {LevelType.Easy, 4},
        {LevelType.Normal, 8},
        {LevelType.Hard, 12},
    }; // レベルと問題の生成数を扱うディクショナリー

    public void SetEasyLevel() => currentLevelType = LevelType.Easy; // 難易度「簡単」にするメソッド *OnClickに登録

    public void SetNormalLevel() => currentLevelType = LevelType.Normal; // 難易度「普通」にするメソッド *OnClickに登録

    public void SetHardLevel() => currentLevelType = LevelType.Hard; // 難易度「難しい」にするメソッド *OnClickに登録
    //--

    [SerializeField] bool checkAnswer; // 問題のチェックに使用する真理値

    GameObject currentQuesitionColorBase; // 現在の問題のオブジェクトを格納するオブジェクト

    GameObject currentCubeSocketBase; // 現在のソケットをまとめたオブジェクトを格納するオブジェクト

    public List<QuesitonLevel> questionLevels = new List<QuesitonLevel>(); // 問題のレベル別のリスト

    [SerializeField] GameObject targetSpawnObject; // 問題を生成するターゲットのオブジェクト

    [SerializeField] List<CubeColorType> questionColors; // 問題の色のリスト

    [SerializeField] List<XrCubeSocketInteractable> cubeSockets = new List<XrCubeSocketInteractable>(); // キューブをハメるソケットのリスト

    [SerializeField] GameObject setGenerateCubeObject; // プレイヤーが掴むことのできるキューブ類をまとめたオブジェクト

    [SerializeField] GameObject chaeckButtonObject; // プレイヤーの回答をチェックするボタンのオブジェクト

    [SerializeField] GameObject rePlayPanelObject; // プレイ後に表示するパネルを扱うオブジェクト

    [Header("サウンド類")]

    [SerializeField] AudioSource educationalSound;

    [SerializeField] AudioSource timerSound;

    [SerializeField] AudioClip startClip;

    [SerializeField] AudioClip clearClip;

    [SerializeField] AudioClip perfectClearClip;

    [Header("タイマー関係")]

    [SerializeField] GameObject TimerObject; // タイマーを扱うオブジェクト

    [SerializeField] Text timerText; // タイマーの時間を表示するテキスト

    public float timerDuration = 10f;// タイマーの時間

    bool timerExit = false; // タイマーを飛ばす時に使用する真理値

    public void TimerExit() => timerExit = true; // タイマーを飛ばすメソッド *OnClickに登録


    // ゲームを行うメソッド
    public void PlayGame()
    {
        playGameColoutin = StartCoroutine(PlayGameCoroutin());
    }

    // ゲームを止めるメソッド
    public void StopGame()
    {
        if (playGameColoutin != null) StopCoroutine(playGameColoutin);
    }

    // ゲームを行うコルーチン
    IEnumerator PlayGameCoroutin()
    {
        CreateQuestionColorsList(); // 問題の色のリストを作成
        CreateQuesitionColorBaseAndSetColors(); // キューブを生成し色を設定
        yield return StartCoroutine(TimerCoroutine()); // タイマーコルーチンをスタート
        educationalSound.PlayOneShot(startClip);
        checkAnswer = false;
        chaeckButtonObject.SetActive(true);
        DestroyQuestionSocket();
        CreateQuestionSocketBase();
        if (setGenerateCubeObject != null) setGenerateCubeObject.SetActive(true);
    }

    // タイマーコルーチン
    IEnumerator TimerCoroutine()
    {
        TimerObject.SetActive(true);
        float remainingTime = timerDuration;
        timerSound.Play();
        timerExit = false;
        while (remainingTime > 0 && timerExit == false) // 残時間が0より上かつtimerExitがflase
        {
            remainingTime -= Time.deltaTime;
            int displayTime = Mathf.CeilToInt(remainingTime);
            timerText.text = displayTime.ToString();
            yield return null;
        }
        timerSound.Stop();
        TimerObject.SetActive(false);
    }

    // 問題となる色のリスト作成
    void CreateQuestionColorsList()
    {
        questionColors.Clear();
        // 現在のレベルに応じた問題数をディクショナリーから取得
        if (levlToQuestionCount.TryGetValue(currentLevelType, out int count))
        {
            for (int i = 0; i < count; i++)
            {
                questionColors.Add(GetRandomEnumValue<CubeColorType>());
            }
        }
        else
        {
            Debug.LogError($"未対応のレベルタイプ: {currentLevelType}");
        }
    }

    // 問題のベースとなるオブジェクトを作成し、色リストの値をセットする
    void CreateQuesitionColorBaseAndSetColors()
    {
        currentQuesitionColorBase = Instantiate(GetQuestionLevelColorsBaseObject(), targetSpawnObject.transform.position, transform.rotation);
        currentQuesitionColorBase.transform.SetParent(targetSpawnObject.transform);
        var xrQuesitionColorBase = currentQuesitionColorBase.GetComponent<XrQuestionColorBase>();
        xrQuesitionColorBase.CreateQuestionCubesForColors(questionColors);
    }

    // ソケットのベースとなるオブジェクトを生成
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

    // 現在のレベルにあった問題を格納するオブジェクトを返す
    GameObject GetQuestionLevelColorsBaseObject()
    {
        QuesitonLevel level = questionLevels.Find(level => level.GetLevelType == currentLevelType);
        return level.GetQuestionColorObjects;
    }

    // 現在のレベルにあったソケットオブジェクトを返す
    GameObject GetQuestionLevelSocketsBaseObject()
    {
        QuesitonLevel level = questionLevels.Find(level => level.GetLevelType == currentLevelType);
        return level.GetQuestionSocketObjects;
    }

    // 正解の判定を行う　
    public void CheckPlayAnswer()
    {
        if (checkAnswer) return;
        setGenerateCubeObject.SetActive(false);
        StartCoroutine(CheckCubeColors());
        checkAnswer = true;
    }

    // 正解の判定のコルーチン
    IEnumerator CheckCubeColors()
    {
        bool perfect = true; // 全問正解かどうか
        for (int i = 0; i < cubeSockets.Count; i++)
        {
            if (cubeSockets[i].hasSelection) // ソケットにオブジェクトがハマっている場合
            {
                if (questionColors[i] == cubeSockets[i].GetSocketedCubeColor) // 正解
                {
                    cubeSockets[i].Correct();
                }
                else // 不正解
                {
                    cubeSockets[i].Miss();
                    perfect = false;
                }
            }
            else // ソケットにオブジェクトがハマっていない場合はMiss
            {
                cubeSockets[i].Miss();
                perfect = false;
            }
            yield return new WaitForSeconds(0.7f);
        }

        // 正答数に応じて処理を分岐
        if (perfect) // 全問正解
        {
            PlaySound(perfectClearClip);
        }
        else
        {
            PlaySound(clearClip);
        }

        yield return new WaitForSeconds(2);

        // もう一度遊ぶかどうかの処理
        if (rePlayPanelObject != null)
        {
            DestroyCubeObject();
            DestroyQuestionSocket();
            rePlayPanelObject.SetActive(true);
        }
    }

    // 指定したタグのオブジェクトをシーンから探し全て削除
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
            Debug.Log($"指定したタグ: {tag} は見つかりませんでした");
        }
    }

    // ソケットタグのオブジェクトを削除
    void DestroyQuestionSocket() => FindTagToDestroy(Socket_Base_Tag);

    // キューブタグのオブジェクトを削除
    void DestroyCubeObject() => FindTagToDestroy(Cube_Tag);

    // ランダムなEnumの値を返す
    public TEnum GetRandomEnumValue<TEnum>()
    {
        TEnum[] values = (TEnum[])System.Enum.GetValues(typeof(TEnum));
        int randomIndex = Random.Range(0, values.Length);
        return values[randomIndex];
    }

    // 引数のクリップの音を鳴らす
    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            educationalSound.PlayOneShot(clip);
        }
    }

    // ゲームを終了させる
    public void Quit()
    {
        // アプリケーションを終了
        Application.Quit();

        // エディター内で実行中の場合、エディターも停止
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