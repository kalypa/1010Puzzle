using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private BackgroundBlockSpawner backgroundBlockSpawner; // ��� ��� ����
    [SerializeField]
    private BackgroundBlockSpawner foregroundBlockSpawner; // ��� ��� ����
    [SerializeField]
    private DragBlockSpawner dragBlockSpawner; // �巡�� ��� ����
    [SerializeField]
    private BlockArrangeSystem blockArrangeSystem; // ��� ��ġ

    private BackgroundBlock[] backgroundBlocks; // ������ ��� ��� ���� ����
    private int currentDragBlockCount; // ���� �����ִ� �巡�� ��� ����

    private readonly Vector2Int blockCount = new Vector2Int(10, 10); // ��� �ǿ� ��ġ�Ǵ� ��� ����
    private readonly Vector2 blockHalf = new Vector2(0.5f, 0.5f); // ��� �ϳ��� ���� ũ��
    private readonly int maxDragBlockCount = 3; // �� ���� ������ �� �ִ� �巡�� ��� ����

    private List<BackgroundBlock> filledBlockList;

    private void Awake()
    {
        filledBlockList = new List<BackgroundBlock>();
        // �� ������� ���Ǵ� ��� ����� ����
        backgroundBlockSpawner.SpawnBlocks(blockCount, blockHalf);

        // �巡�� ����� ��ġ�� �� ������ ����Ǵ� ��� ����� ����
        backgroundBlocks = new BackgroundBlock[blockCount.x * blockCount.y];
        backgroundBlocks = foregroundBlockSpawner.SpawnBlocks(blockCount, blockHalf);

        // ��� ��ġ �ý���
        blockArrangeSystem.Setup(blockCount, blockHalf, backgroundBlocks, this);

        // �巡�� ��� ����
        SpawnDragBlock();
    }

    private void SpawnDragBlock()
    {
        // ���� �巡�� ����� ������ �ִ�(3)�� ����
        currentDragBlockCount = maxDragBlockCount;
        // �巡�� ��� ����
        dragBlockSpawner.SpawnBlocks();
    }

    public void AfterBlockArrangement(DragBlock block)
    {
        StartCoroutine("OnAfterBlockArrangement", block);
    }

    /// <summary>
    /// ��� ��ġ ��ó��
    /// �巡�� ��� ����/����, �� �ϼ�, ���ӿ���, ����
    /// </summary>
    private IEnumerator OnAfterBlockArrangement(DragBlock block)
    {
        //��ġ�� �Ϸ�� �巡�� ��� ����
        Destroy(block.gameObject);

        //�ϼ��� ���� �ִ��� �˻��ϰ�, �ϼ��� ���� ��ϵ��� ������ ����
        int filledLineCount = CheckFilledLine();

        //���� �ϼ��� ��ϵ��� ���� (�������� ��ġ�� ����� �������� ������������ ����)
        yield return StartCoroutine(DestroyFilledBlocks(block));

        //��� ��ġ�� ���������� ���� �����ִ� �巡�� ����� ������ 1 ����
        currentDragBlockCount--;
        //���� ��ġ ������ �巡�� ����� ������ 0�̸� �巡�� ��� ����
        if (currentDragBlockCount == 0)
        {
            SpawnDragBlock();
        }

    }

    private int CheckFilledLine()
    {
        int filledLineCount = 0;

        filledBlockList.Clear();

        for( int y = 0; y < blockCount.y; y++)
        {
            int fillBlockCount = 0;
            for(int x = 0; x < blockCount.x; x++)
            {
                if (backgroundBlocks[y * blockCount.x + x].BlockState == BlockState.Fill)
                {
                    fillBlockCount++;
                }
            }

            if (fillBlockCount == blockCount.x)
            {
                for (int x = 0; x < blockCount.x; x++)
                {
                    filledBlockList.Add(backgroundBlocks[y * blockCount.x + x]);
                }
                filledLineCount++;
            }
        }

        for (int x = 0; x < blockCount.x; x++)
        {
            int fillBlockCount = 0;
            for (int y = 0; y < blockCount.y; y++)
            {
                if (backgroundBlocks[y * blockCount.x + x].BlockState == BlockState.Fill)
                {
                    fillBlockCount++;
                }
            }

            if (fillBlockCount == blockCount.y)
            {
                for (int y = 0; y < blockCount.y; y++)
                {
                    filledBlockList.Add(backgroundBlocks[y * blockCount.x + x]);
                }
                filledLineCount++;
            }
        }

        return filledLineCount;
    }

    private IEnumerator DestroyFilledBlocks(DragBlock block)
    {
        filledBlockList.Sort((a, b) => (a.transform.position - block.transform.position).sqrMagnitude.CompareTo((b.transform.position - block.transform.position).sqrMagnitude));
        for(int i = 0; i < filledBlockList.Count; i++)
        {
            filledBlockList[i].EmptyBlock();

            yield return new WaitForSeconds(0.01f);
        }

        filledBlockList.Clear();
    }
}
