

public interface IInteractable
{
    public Asset CurrentAsset { get; }
    public void GetHit(float damage);
    public float CurrentHP { get; }
}
