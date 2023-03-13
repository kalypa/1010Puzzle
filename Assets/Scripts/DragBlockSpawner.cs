using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBlockSpawner : MonoBehaviour
{
    [SerializeField]
    private BlockArrangeSystem blockArrangeSystem; // DragBlock Ŭ�������� ���
    [SerializeField]
    private Transform[] blockSpawnPoints; // �巡�� ������ ����� ��ġ�Ǵ� ��ġ
    [SerializeField]
    private GameObject[] blockPrefabs; // ���� ������ ��� ��� ������
    [SerializeField]
    private Vector3 spawnGapAmount = new Vector3(10, 0, 0); // ó�� ������ �� �θ�� ������ �Ÿ�

    //private void Awake()
    public void SpawnBlocks()
    {
        StartCoroutine("OnSpawnBlocks");
    }
    
    private IEnumerator OnSpawnBlocks()
    {
        // �巡�� ��� 3��(blocksParent.Length) ����
        for ( int i = 0; i < blockSpawnPoints.Length; ++i )
        {
            yield return new WaitForSeconds(0.1f);

            // ������ �巡�� ��� ����
            int index = Random.Range(0, blockPrefabs.Length);
            // �巡�� ����� �����Ǵ� ��ġ
            Vector3 spawnPosition = blockSpawnPoints[i].position + spawnGapAmount;
            // �巡�� ��� ���� (���� ������, ���� ��ġ, �ʱ� ȸ����, �θ� Transform)
            GameObject clone = Instantiate(blockPrefabs[index], spawnPosition, Quaternion.identity, blockSpawnPoints[i]);

            // �巡�� ����� �����ϰ�, �θ��� ��ġ���� �̵��ϴ� �ִϸ��̼� ���
            clone.GetComponent<DragBlock>().Setup(blockArrangeSystem, blockSpawnPoints[i].position);
        }


    }
}
