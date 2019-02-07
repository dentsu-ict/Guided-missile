using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public int OldPosNum = 20;  //ビームの長さ
    public float width = 0.1f;  //ビームの幅
    Vector2[] OldPos;           //位置情報履歴
    Vector3[] vertices;         //頂点情報
    int[] triangles;            //インデックス情報
    Vector2[] uv;               //uv情報
    Mesh mesh;                  //mesh

    void Start()
    {
        OldPos = new Vector2[OldPosNum];
        for (int i = 0; i < OldPosNum; i++)
        {
            OldPos[i] = transform.position;
        }

        mesh = GetComponent<MeshFilter>().mesh;

        //頂点情報の設定
        vertices = new Vector3[(OldPosNum - 1) * 2];
        TransformVertices();
        mesh.vertices = vertices;

        //インデックス情報（頂点の順番）の設定
        triangles = new int[(OldPosNum - 2) * 6];
        for (int i = 0; i <= OldPosNum - 3; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                triangles[i * 6 + j] = i * 2 + j;
                triangles[i * 6 + j + 3] = i * 2 + (3 - j);
            }
        }
        mesh.triangles = triangles;

        //各頂点のテクスチャの情報の設定
        uv = new Vector2[(OldPosNum - 1) * 2];
        for (int i = 0; i <= OldPosNum - 2; i++)
        {
            uv[i * 2] = new Vector2(0.0f, 1.0f - (float)i / (OldPosNum - 2));
            uv[i * 2 + 1] = new Vector2(1.0f, 1.0f - (float)i / (OldPosNum - 2));
        }
        mesh.uv = uv;
        mesh.RecalculateBounds();
    }

    void FixedUpdate()
    {
        //位置情報の履歴を渡す
        for (int i = OldPosNum - 1; i >= 0; i--)
        {
            if (i == 0) OldPos[i] = transform.position;
            else OldPos[i] = OldPos[i - 1];
        }
        //位置情報の履歴を使って頂点情報の更新
        TransformVertices();
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    public void TransformVertices()
    {
        for (int i = 0; i <= OldPosNum - 2; i++)
        {
            Vector2 sub;
            //ローカルポジションの設定
            Vector2 localpos = OldPos[i] - OldPos[0];
            //履歴からビームの向きを検出
            sub = (OldPos[i + 1] - OldPos[i]).normalized * width;
            //ビームの向きを左に変更
            Vector2 left = new Vector2(sub.y, -sub.x);
            //ビームの向きを右に変更
            Vector2 right = new Vector2(-sub.y, sub.x);
            //左右の頂点の情報を設定			
            vertices[i * 2] = localpos + left;
            vertices[i * 2 + 1] = localpos + right;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
