using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidsNPC : NPC {

    public Enemy guard;
    private SpriteRenderer[] sprites;
    // Use this for initialization
    private void Awake()
    {
        Component[] spr = GetComponentsInChildren(typeof(SpriteRenderer));
        sprites = new SpriteRenderer[spr.Length];
        for (int i = 0; i < spr.Length; i++)
        {
            sprites[i] = spr[i] as SpriteRenderer;
        }
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].enabled = false;
        }
        GetComponent<BoxCollider2D>().enabled = false;
    }
    public override void Start () {
        base.Start();
        tolerance = 5;
        
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        if(guard == null)
        {
            Enable();
        }
    }

    public override void ReadDialouge()
    {
        if(sprites[0].enabled)
            base.ReadDialouge();

    }

    public void Enable()
    {
        
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].enabled = true;
        }
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
