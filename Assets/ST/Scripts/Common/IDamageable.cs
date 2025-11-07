namespace ST
{

    public interface IDamageable
    {
        void TakeDamage(float damage);
        float GetCurrentHealth();
        float GetMaxHealth();
        bool IsDead();
    }
}