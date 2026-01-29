using _Project.Develop.Runtime.Utils.ReactiveManagement;

public interface IHealable
{
    IReadOnlyVariable<bool> CanHeal { get; }
    IReadOnlyVariable<float> HealthPercent { get; }
    void Heal(float value);
}

