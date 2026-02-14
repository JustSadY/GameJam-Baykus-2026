namespace InteractionSystem.Struct
{
    public enum InteractionType
    {
        Instant,
        Hold
    }

    [System.Serializable]
    public class InteractionData
    {
        public string Prompt;
        public string IconName;
        public string Description;
        public InteractionType Type;
        public float HoldDuration;
        public float Cooldown;
        public bool IsActive;
    }
}