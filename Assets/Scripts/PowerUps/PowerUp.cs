using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PowerUp : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnPowerUpCollected = new UnityEvent();
    public bool Collectable { get;set;}
    protected Vector2 GetOrientation()
        => ((Vector2)GameManager.Instance.PlanetController.transform.position - (Vector2)transform.position).normalized;

    protected abstract void ApplyEffect();

    public void Despawn()
    {
        Collectable = false;
        OnPowerUpCollected.RemoveAllListeners();
        Destroy(gameObject);
    }

    public void OnCollected()
    {
        if (!Collectable)
            return;
        ApplyEffect();
        Despawn();
    }    

    private void Start()
    {
        GameManager.Instance.OnGameOver.AddListener(() => Collectable = false);

        var planet = GameManager.Instance.PlanetController;
        float orientation = Vector2.SignedAngle(Vector2.down, GetOrientation());

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, orientation));

        Collectable = true;
        OnPowerUpCollected.AddListener(() => OnCollected());
    }

}
