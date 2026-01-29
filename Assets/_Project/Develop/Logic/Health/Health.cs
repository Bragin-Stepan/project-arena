using _Project.Develop.Runtime.Utils.ReactiveManagement;

public class Health
{
    private ReactiveVariable<float> _max;
    private ReactiveVariable<float> _current;
    private ReactiveVariable<float> _percent = new(1);
    
    private ReactiveVariable<bool> _isDead;
    
    public Health(float maxValue)
    {
        _max = new ReactiveVariable<float>(maxValue);
        _current = new ReactiveVariable<float>(maxValue);
        _isDead = new ReactiveVariable<bool>(false);
        
        _current.Subscribe(OnCurrentChanged);
    }

    public IReadOnlyVariable<bool> IsDead => _isDead;
    public IReadOnlyVariable<float> Current => _current;
    public IReadOnlyVariable<float> Max => _max;
    public IReadOnlyVariable<float> Percent => _percent;
    
    public bool TryIncrease(float value)
    {
        if (value < 0 && _isDead.Value)
            return false;

        _current.Value += value;

        if (_current.Value > _max.Value)
            _current = _max;

        return true;
    }

    public bool TryReduce(float value)
    {
        if (value < 0 && _isDead.Value)
            return false;
            
        _current.Value -= value;
            
        if (_current.Value <= 0)
        {
            _current.Value = 0;
            _isDead.Value = true;
        }
        
        return true;
    }

    public void Reset()
    {
        _current.Value = _max.Value;
        _isDead.Value = false;
    }
    
    public void Kill()
    {
        TryReduce(Max.Value);
    }
    
    private void OnCurrentChanged(float oldValue, float newValue)
    {
        _percent.Value = newValue / _max.Value;
    }
}