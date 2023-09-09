public class Mission
{
    public string Description { get; private set; }
    public int TargetCount { get; private set; }
    public int CurrentCount { get; private set; }
    public bool IsCompleted { get; private set; }

    public Mission(string description, int targetCount)
    {
        Description = description;
        TargetCount = targetCount;
        CurrentCount = 0;
        IsCompleted = false;
    }

    public void IncrementCount()
    {
        if (!IsCompleted)
        {
            CurrentCount++;
            if (CurrentCount >= TargetCount)
            {
                Complete();
            }
        }
    }

    public void Complete()
    {
        IsCompleted = true;
        CurrentCount = TargetCount;
    }
}
