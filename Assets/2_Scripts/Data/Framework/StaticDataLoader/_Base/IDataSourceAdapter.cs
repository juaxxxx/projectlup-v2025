using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 데이터 소스 어댑터 인터페이스
/// CSV, JSON 등 다양한 데이터 소스로부터 데이터를 로드하는 어댑터 패턴 구현
/// </summary>
public interface IDataSourceAdapter
{
    /// <summary>
    /// 데이터 소스로부터 원본 데이터를 로드합니다
    /// </summary>
    /// <param name="url">데이터 소스 URL</param>
    /// <param name="onSuccess">성공 시 콜백 (로드된 원본 텍스트 데이터)</param>
    /// <param name="onError">실패 시 콜백 (에러 메시지)</param>
    /// <returns>코루틴</returns>
    IEnumerator LoadData(string url, System.Action<string> onSuccess, System.Action<string> onError);

    /// <summary>
    /// 원본 데이터를 객체 리스트로 파싱합니다
    /// ColumnAttribute 기반 Reflection 매핑 사용
    /// </summary>
    /// <typeparam name="T">파싱할 데이터 타입</typeparam>
    /// <param name="rawData">원본 데이터 (CSV 또는 JSON 문자열)</param>
    /// <param name="startRow">데이터 읽기 시작 행 (CSV의 경우 헤더 위치)</param>
    /// <returns>파싱된 객체 리스트</returns>
    List<T> ParseToObjects<T>(string rawData, int startRow) where T : new();
}
