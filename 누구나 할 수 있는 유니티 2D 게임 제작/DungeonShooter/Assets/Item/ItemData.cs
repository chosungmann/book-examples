using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アイテムの種類
public enum ItemType
{
    arrow,      //矢
    key,        //カギ
    life,	   //ライフ
}

public class ItemData : MonoBehaviour
{
    public ItemType type;           //アイテムの種類
    public int count = 1;           //アイテム数

    public int arrangeId = 0;       //配置の識別に使う

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //接触（物理）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == ItemType.key)
            {
                //カギ
                ItemKeeper.hasKeys += 1;
            }
            else if (type == ItemType.arrow)
            {
                //矢
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if (type == ItemType.life)
            {
                //ライフ
                if (PlayerController.hp < 3)
                {
                    //HPが３以下の場合加算する
                    PlayerController.hp++;
                    //HPの更新
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }
            //++++ アイテム取得演出 ++++
            //当たりを消す
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            //アイテムのRigidbody2Dを取ってくる
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            //重力を戻す
            itemBody.gravityScale = 2.5f;
            //上に少し跳ねあげる演出
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            //0.5秒後に削除
            Destroy(gameObject, 0.5f);

            //配置Idの記録
            SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);

            //SE再生（アイテムゲット）
            SoundManager.soundManager.SEPlay(SEType.ItemGet);
        }
    }
}

