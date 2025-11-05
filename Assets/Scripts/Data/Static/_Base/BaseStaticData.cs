using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class BaseStaticData : ScriptableObject
{
    protected abstract string URL { get; }

    [Header("스프레드 시트의 시트 이름")][SerializeField] public string associatedWorksheet = "";
    [Header("읽기 시작할 행 번호")][SerializeField] public int START_ROW = 1;

    public abstract IEnumerator LoadSheet();
}

public abstract class BaseStaticData<T> : BaseStaticData where T : new()
{
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")][SerializeField]
    public List<T> DataList = new List<T>();

    public List<T> GetDataList() => DataList;

    protected void ParseSheet(string csvData)
    {
        Debug.Log($"[{GetType().Name}] ParseSheet called with {csvData.Length} chars");
        Debug.Log($"[{GetType().Name}] START_ROW = {START_ROW}");

        // CSV 파서 사용 (따옴표 안의 쉼표/개행 처리)
        string[] lines = CSVParser.SplitLines(csvData);
        Debug.Log($"[{GetType().Name}] Split into {lines.Length} lines");

        // START_ROW는 1-based, 배열은 0-based이므로 -1 필요
        int headerIndex = START_ROW - 1;

        if (lines.Length <= headerIndex)
        {
            Debug.LogWarning($"[{GetType().Name}] Not enough lines in CSV (have {lines.Length}, need at least {headerIndex + 1})");
            return;
        }

        // START_ROW에서 헤더 읽기
        string[] headers = CSVParser.ParseLine(lines[headerIndex]);
        Debug.Log($"[{GetType().Name}] Headers from row {START_ROW} (index {headerIndex}): {string.Join(", ", headers)}");

        Dictionary<string, int> headerMap = new Dictionary<string, int>();
        for (int i = 0; i < headers.Length; i++)
        {
            string headerName = headers[i].Trim();
            headerMap[headerName] = i;
        }

        DataList.Clear();

        int successCount = 0;
        int failCount = 0;

        // START_ROW + 1부터 데이터 읽기 (헤더 다음 행부터)
        for (int i = headerIndex + 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            // CSV 파서 사용 (따옴표로 감싸진 필드 처리)
            string[] values = CSVParser.ParseLine(lines[i]);

            try
            {
                T data = ParseDataRow(values, headerMap);
                if (data != null)
                {
                    DataList.Add(data);
                    successCount++;
                }
                else
                {
                    failCount++;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[{GetType().Name}] Failed to parse line {i}: {e.Message}");
                failCount++;
            }
        }

        Debug.Log($"[{GetType().Name}] Loaded {DataList.Count} entries (Success: {successCount}, Failed: {failCount})");
    }

    protected virtual T ParseDataRow(string[] values, Dictionary<string, int> headerMap)
    {
        T instance = new T();
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            ColumnAttribute columnAttr = field.GetCustomAttribute<ColumnAttribute>();

            if (columnAttr == null)
                continue;

            string headerName = columnAttr.HeaderName;

            if (!headerMap.ContainsKey(headerName))
            {
                if (columnAttr.Required)
                {
                    Debug.LogWarning($"[{GetType().Name}] Required column '{headerName}' not found in headers");
                }
                continue;
            }

            int columnIndex = headerMap[headerName];

            if (columnIndex >= values.Length)
            {
                Debug.LogWarning($"[{GetType().Name}] Column '{headerName}' index {columnIndex} out of range (values length: {values.Length})");
                continue;
            }

            string value = values[columnIndex].Trim();

            try
            {
                // 타입별 파싱
                if (field.FieldType == typeof(string))
                {
                    field.SetValue(instance, value);
                }
                else if (field.FieldType == typeof(int))
                {
                    if (int.TryParse(value, out int intValue))
                        field.SetValue(instance, intValue);
                    else
                        Debug.LogWarning($"[{GetType().Name}] Failed to parse '{value}' as int for field '{field.Name}'");
                }
                else if (field.FieldType == typeof(float))
                {
                    if (float.TryParse(value, out float floatValue))
                        field.SetValue(instance, floatValue);
                    else
                        Debug.LogWarning($"[{GetType().Name}] Failed to parse '{value}' as float for field '{field.Name}'");
                }
                else if (field.FieldType == typeof(bool))
                {
                    if (bool.TryParse(value, out bool boolValue))
                        field.SetValue(instance, boolValue);
                    else
                        Debug.LogWarning($"[{GetType().Name}] Failed to parse '{value}' as bool for field '{field.Name}'");
                }
                else if (field.FieldType == typeof(double))
                {
                    if (double.TryParse(value, out double doubleValue))
                        field.SetValue(instance, doubleValue);
                    else
                        Debug.LogWarning($"[{GetType().Name}] Failed to parse '{value}' as double for field '{field.Name}'");
                }
                else if (field.FieldType == typeof(long))
                {
                    if (long.TryParse(value, out long longValue))
                        field.SetValue(instance, longValue);
                    else
                        Debug.LogWarning($"[{GetType().Name}] Failed to parse '{value}' as long for field '{field.Name}'");
                }
                else
                {
                    Debug.LogWarning($"[{GetType().Name}] Unsupported field type: {field.FieldType} for field '{field.Name}'");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[{GetType().Name}] Failed to set field '{field.Name}': {e.Message}");
            }
        }

        return instance;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BaseStaticData), true)]
public class BaseStaticDataReaderEditor : Editor
{
    BaseStaticData data;

    void OnEnable()
    {
        data = (BaseStaticData)target;
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
            IEnumerator coroutine = data.LoadSheet();

            while (coroutine.MoveNext())
            {
                if (coroutine.Current != null)
                {
                    if (coroutine.Current is UnityEngine.Networking.UnityWebRequestAsyncOperation asyncOp)
                    {
                        Debug.Log("[BaseStaticData] Waiting for web request...");
                        while (!asyncOp.isDone)
                        {
                            await System.Threading.Tasks.Task.Delay(100);
                        }
                        Debug.Log("[BaseStaticData] Web request completed!");
                    }
                }
            }

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            Debug.Log("[BaseStaticData] Data loading completed!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[BaseStaticData] Error: {e.Message}\n{e.StackTrace}");
        }
    }
}
#endif
