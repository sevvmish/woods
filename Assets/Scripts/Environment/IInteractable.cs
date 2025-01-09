

public interface IInteractable
{
    public Asset CurrentAsset { get; set; }
    public void GetHit(float damage);
    public float CurrentHP { get; set; }
}
