using UnityEngine;

/// <summary> �e�̐e�N���X </summary>
public class BulletParent : MonoBehaviour
{
    protected float speed;//�ړ����x
    protected Vector3 velocity;//�ړ�
    protected SpriteRenderer spriteRenderer;//��ʓ��ɂ��邩��������

    protected Vector3 dir;//��]����

    protected int attackPower;//�U����

    protected virtual void Start()
    {
        speed = 1.0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        dir = Vector3.zero;
        attackPower = 1;
    }

    private void FixedUpdate()
    {
        Move();

        //�e����ʊO�ɏo����폜����
        if(!spriteRenderer.isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary> �ړ����� </summary>
    protected virtual void Move()
    {
        //�i�s�����։�]
        this.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        //�ړ�
        this.transform.position += velocity * Time.deltaTime * speed;
    }

    /// <summary> �U���� </summary>
    public int AttackPower()
    {
        return attackPower;
    }

    /// <summary> �ړ����������߂� </summary>
    /// <param name="isLeft">true:�������ֈړ����� false:�E�����ֈړ�����</param>
    public void MoveDirection(bool isLeft)
    {
        if(isLeft)
        {
            velocity = new Vector3(-1.0f, 0.0f, 0.0f);

            var nextPos = velocity * speed + this.transform.position;
            dir = nextPos - this.transform.position;
        }
        else
        {
            velocity = new Vector3(1.0f, 0.0f, 0.0f);
        }
    }
}
