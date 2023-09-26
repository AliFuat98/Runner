using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
  [SerializeField] private Transform levelPartPrefab;
  [SerializeField] private int amountToPool;

  [SerializeField] private Vector3 nextLevelPartPosition;
  [SerializeField] private float distanceToSpawn;

  private List<Transform> pooledLevelPartList;
  private Player playerInstance;

  void Start() {
    pooledLevelPartList = new List<Transform>();
    Transform temp;
    for (int i = 0; i < amountToPool; i++) {
      temp = Instantiate(levelPartPrefab, transform);
      temp.gameObject.SetActive(false);
      pooledLevelPartList.Add(temp);
    }

    playerInstance = Player.Instance;
  }

  void Update() {
    
    if (Vector3.Distance(playerInstance.transform.position, nextLevelPartPosition) < distanceToSpawn) {
      LevelPart levelPart = levelPartPrefab.GetComponent<LevelPart>();
      Vector2 newPosition = new Vector2(nextLevelPartPosition.x - levelPart.GetStartPoint().position.x, 0);

      LevelPart newLevelPart = GetPooledLevelPart();
      newLevelPart.gameObject.SetActive(true);
      newLevelPart.transform.position = newPosition;

      nextLevelPartPosition = newLevelPart.GetEndPoint().position;
    }
  }

  private LevelPart GetPooledLevelPart() {
    for (int i = 0; i < amountToPool; i++) {
      if (!pooledLevelPartList[i].gameObject.activeInHierarchy) {
        return pooledLevelPartList[i].GetComponent<LevelPart>();
      }
    }
    return null;
  }
}