using UnityEngine;

public class LevelPart : MonoBehaviour {
  [SerializeField] private float distanceToDelete;
  [SerializeField] private Transform EndPointTransform;
  [SerializeField] private Transform StartPointTransform;
  private Player playerInstance;

  private void Start() {
    playerInstance = Player.Instance;
  }
  private void Update() {
    if (playerInstance.transform.position.x - transform.position.x > distanceToDelete) {
      gameObject.SetActive(false);
    }
  }

  public Transform GetEndPoint() {
    return EndPointTransform;
  }

  public Transform GetStartPoint() {
    return StartPointTransform;
  }
}