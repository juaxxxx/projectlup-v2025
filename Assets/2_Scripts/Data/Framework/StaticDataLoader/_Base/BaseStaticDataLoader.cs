using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BaseStaticDataLoader : ScriptableObject
{
    [Header("데이터 소스 설정")]
    [SerializeField] public LUP.Define.DataSourceType sourceType = LUP.Define.DataSourceType.CSV;

    [Header("스프레드 시트의 시트 이름")][SerializeField] public string associatedWorksheet = "";
    [Header("읽기 시작할 행 번호")][SerializeField] public int START_ROW = 1;

    public abstract IEnumerator LoadSheet();
}

public abstract class BaseStaticDataLoader<T> : BaseStaticDataLoader where T : new()
{
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")]
    [SerializeField]
    public List<T> DataList = new List<T>();

    protected abstract string CSV_URL { get; }

    public List<T> GetDataList() => DataList;

    public override IEnumerator LoadSheet()
    {
        IDataSourceAdapter adapter = CreateAdapter(sourceType);

        string url = GetURL(sourceType);

        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError($"[{GetType().Name}] URL is empty for source type: {sourceType}");
            yield break;
        }

        Debug.Log($"[{GetType().Name}] Loading data from {sourceType} source: {url}");

        // 어댑터 데이터 로드
        string rawData = null;
        string error = null;

        yield return adapter.LoadData(url,
            data => rawData = data,
            err => error = err);

        // 에러 체크
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError($"[{GetType().Name}] Failed to load data: {error}");
            yield break;
        }

        // rawData null 체크
        if (string.IsNullOrEmpty(rawData))
        {
            Debug.LogError($"[{GetType().Name}] Loaded data is null or empty. URL: {url}");
            yield break;
        }

        Debug.Log($"[{GetType().Name}] Data loaded successfully. Size: {rawData.Length} chars");

        // 어댑터 파싱
        DataList = adapter.ParseToObjects<T>(rawData, START_ROW);
        Debug.Log($"[{GetType().Name}] Successfully loaded {DataList.Count} entries from {sourceType}");
    }

    private IDataSourceAdapter CreateAdapter(LUP.Define.DataSourceType type)
    {
        switch (type)
        {
            case LUP.Define.DataSourceType.CSV:
                return new CSVDataSourceAdapter();
            default:
                throw new System.NotImplementedException($"Adapter for {type} not implemented");
        }
    }

    private string GetURL(LUP.Define.DataSourceType type)
    {
        switch (type)
        {
            case LUP.Define.DataSourceType.CSV:
                return CSV_URL;
            default:
                return "";
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(BaseStaticDataLoader), true)]
public class BaseStaticDataReaderEditor : Editor
{
    BaseStaticDataLoader data;

    void OnEnable()
    {
        data = (BaseStaticDataLoader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 읽기"))
        {
            Debug.Log("[BaseStaticData] Button clicked, starting load...");
            LoadDataAsync();
        }
    }

    private async void LoadDataAsync()
    {
        try
        {
            Debug.Log("[BaseStaticData] Starting LoadDataAsync...");
            IEnumerator coroutine = data.LoadSheet();
            await ProcessCoroutine(coroutine);

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            Debug.Log("[BaseStaticData] Data loading completed!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[BaseStaticData] Error: {e.Message}\n{e.StackTrace}");
        }
    }

    private async System.Threading.Tasks.Task ProcessCoroutine(IEnumerator coroutine)
    {
        while (coroutine.MoveNext())
        {
            object current = coroutine.Current;

            if (current == null)
            {
                // null인 경우 yield return null과 같음
                await System.Threading.Tasks.Task.Delay(10);
            }
            else if (current is IEnumerator nestedCoroutine)
            {
                // 중첩된 코루틴 재귀 처리
                Debug.Log("[BaseStaticData] Processing nested coroutine...");
                await ProcessCoroutine(nestedCoroutine);
            }
            else if (current is UnityEngine.Networking.UnityWebRequestAsyncOperation asyncOp)
            {
                // UnityWebRequest 대기
                Debug.Log("[BaseStaticData] Waiting for web request...");
                while (!asyncOp.isDone)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                }
                Debug.Log("[BaseStaticData] Web request completed!");
            }
            else
            {
                // 기타 yield 처리
                await System.Threading.Tasks.Task.Delay(10);
            }
        }
    }
}
#endif
