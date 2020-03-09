using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caillou : MonoBehaviour
{
    // Start is called before the first frame update
    private FollowerOfTheRhythm tempo;
    private SpriteRenderer spriteRenderer;
    private Transform m_Transform;
    private BoxCollider2D m_Collider;

    public Sprite north;
    public Sprite east;
    public Sprite south;
    public Sprite west;
    private List<Sprite> sprites = new List<Sprite>();

    private int curSpriteIndex = 0;
    private int dir = 1;

    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_Transform = GetComponent<Transform>();
        m_Collider = GetComponent<BoxCollider2D>();
        sprites.Add(north);
        sprites.Add(east);
        sprites.Add(south);
        sprites.Add(west);
    }

    // Update is called once per frame
    void Update()
    {
        if (tempo.canMoveToRythm())
        {
            if (curSpriteIndex == 8)
                dir = -1;
            if (curSpriteIndex == 0)
                dir = +1;
            curSpriteIndex += dir;
            spriteRenderer.sprite = sprites[curSpriteIndex % 4];
            m_Transform.Translate(new Vector3((m_Collider.size.x * m_Transform.lossyScale.x
                + m_Collider.size.y * m_Transform.lossyScale.y) / 2 * dir, 0));
        }
    }
}
