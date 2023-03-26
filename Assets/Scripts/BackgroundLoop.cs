using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackgroundLoop : MonoBehaviour {
    private float width; // 배경의 가로 길이
    private BoxCollider2D boxCollider2D;
    private Vector2 offset;

    private void Awake() {
        // 가로 길이를 측정하는 처리
        this.boxCollider2D = GetComponent<BoxCollider2D>();
        this.width = this.boxCollider2D.size.x;
        this.offset = new Vector2(this.width * 2f, 0);
    }

    private void Update() {
        // 현재 위치가 원점에서 왼쪽으로 width 이상 이동했을 때 위치를 리셋
        if (transform.position.x <= -width) {
            Reposition();
        }
    }

    // 위치를 리셋하는 메서드
    private void Reposition() {
        transform.position = (Vector2)transform.position + this.offset;
    }
}