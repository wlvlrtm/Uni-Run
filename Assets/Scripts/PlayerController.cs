using UnityEngine;

public enum CLIP {
    DIE = 0,
    JUMP = 1
}

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
    [SerializeField] private float jumpForce = 700f; // 점프 힘
    [SerializeField] private AudioClip[] playerAudioClips;   // 사용할 오디오 클립 배열

    private int jumpCount = 0; // 누적 점프 횟수
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태
    private Vector2 jumpDir;    // 점프 방향 벡터
    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트
    

    private void Start() {
        // 초기화
        this.playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        this.animator = gameObject.GetComponent<Animator>();
        this.playerAudio = gameObject.GetComponent<AudioSource>();

        this.jumpDir = new Vector2(0, this.jumpForce);
    }

    private void Update() {
        // 사용자 입력을 감지하고 점프하는 처리
        if (this.isDead) {  // 플레이어가 사망했을 경우, 바로 return
            return;
        }

        // 플레이어 점프
        if (Input.GetKeyDown(KeyCode.Space) && this.jumpCount < 2) {
            this.jumpCount++;   // 카운트 += 1    
            this.playerRigidbody.velocity = Vector2.zero;   // 속도 0
            this.playerRigidbody.AddForce(this.jumpDir);    // 점프
            this.playerAudio.clip = this.playerAudioClips[(int)CLIP.JUMP]; // 오디오 클립 설정
            this.playerAudio.Play();    // 오디오 재생
        }
        else if (Input.GetKeyUp(KeyCode.Space) && this.playerRigidbody.velocity.y > 0) {
            this.playerRigidbody.velocity *= 0.5f;
        }

        this.animator.SetBool("Grounded", this.isGrounded);
    }

    private void Die() {
        // 사망 처리
        this.animator.SetTrigger("Die");
        this.playerAudio.clip = this.playerAudioClips[(int)CLIP.DIE];   // 오디오 클립 설정; DIE
        this.playerAudio.Play();
        this.playerRigidbody.velocity = Vector2.zero;
        this.isDead = true;
        GameManager.instance.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.transform.tag == "Dead" && !isDead) {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // 바닥에 닿았음을 감지하는 처리
        if (collision.contacts[0].normal.y > 0.7f) {
            this.isGrounded = true;
            this.jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // 바닥에서 벗어났음을 감지하는 처리
        this.isGrounded = false;
    }
}