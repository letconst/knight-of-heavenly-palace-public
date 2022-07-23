public class AttackMagnification 
{
    public float Value { get;private set; }
    public float MaxValue { get;private set; }

    public AttackMagnification(float floatValue, float floatMaxValue = float.MaxValue)
    {
        Value = 0 <= floatValue ? floatValue : 0;
        MaxValue = floatMaxValue;
    }
    
}
