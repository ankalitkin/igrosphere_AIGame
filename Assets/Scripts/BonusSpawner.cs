using System.Collections;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private float delay = 5;
    private int _bonusCount;

    private void Update()
    {
        if (_bonusCount == 0 && GameManager.Instance.DeadCharacters.childCount > 0)
        {
            _bonusCount++;
            StartCoroutine(SpawnBonus());
        }
    }

    private IEnumerator SpawnBonus()
    {
        yield return new WaitForSeconds(delay);
        Vector3 pos = GameManager.Instance.SpawnSystem.GetSpawnPoint(2.5f);
        pos.y = -.95f;
        Instantiate(bonusPrefab, pos, Quaternion.identity, transform);
    }

    public void BonusRemoved()
    {
        _bonusCount--;
    }
}