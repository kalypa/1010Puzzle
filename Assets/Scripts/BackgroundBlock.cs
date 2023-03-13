using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockState { Empty = 0, Fill = 1 }

public class BackgroundBlock : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // 배경 블록의 색상 제어를 위한 컴포넌트

    public BlockState BlockState { private set; get; } // 배경 블록의 상태

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        BlockState = BlockState.Empty;
    }

    /// <summary>
    /// 블록을 드래그하여 배경 블록에 배치했을 때
    /// 배경 블록의 색상을 드래그 한 블록과 동일하게 설정
    /// </summary>
    public void FillBlock(Color color)
    {
        BlockState = BlockState.Fill;
        spriteRenderer.color = color;
    }

    public void EmptyBlock()
    {
        BlockState = BlockState.Empty;

        StartCoroutine("ScaleTo", Vector3.zero);
    }

    private IEnumerator ScaleTo(Vector3 end)
    {
        Vector3 start = transform.localScale;
        float current = 0;
        float percent = 0;
        float time = 0.15f;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.localScale = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        //축소 애니메이션이 종료되면 블록의 색상을 하얀색으로 설정하고, 블록 크기를 1로 설정
        spriteRenderer.color = Color.white;
        transform.localScale = Vector3.one;
    }
}
