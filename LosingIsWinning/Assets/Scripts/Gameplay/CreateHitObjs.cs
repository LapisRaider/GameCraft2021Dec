using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitObjs : HitObjs
{
    public List<Sprite> m_boxSprites;
    int m_dmgCounter;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        GetComponent<SpriteRenderer>().sprite = m_boxSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    public override bool Hit()
    {
        base.Hit();

        m_dmgCounter++;

        if (m_dmgCounter < m_boxSprites.Count)
        {
            GetComponent<SpriteRenderer>().sprite = m_boxSprites[m_dmgCounter];
        }
        else
        {
            gameObject.SetActive(false);
        }

        return true;
    }
}
