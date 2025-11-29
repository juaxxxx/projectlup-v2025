namespace LUP.ST
{

    public interface IPoolable
    {
        void OnSpawn();      // 풀에서 꺼낼 때
        void OnDespawn();    // 풀로 돌아갈 때
    }
}