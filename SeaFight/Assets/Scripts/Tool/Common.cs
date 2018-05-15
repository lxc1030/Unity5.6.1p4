using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Common : MonoBehaviour
{
    public static GameObject Generate(string path)
    {
        return Instantiate(Resources.Load(path) as GameObject);
    }

    public static GameObject Generate(string path, Transform parent)
    {
        return Generate(Resources.Load(path) as GameObject, parent);
    }

    public static GameObject Generate(GameObject prefab, Transform parent)
    {
        GameObject obj = null;
        obj = Instantiate(prefab) as GameObject;
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
        return obj;
    }

    public static void Clear(Transform trans)
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < trans.childCount; i++)
        {
            temp.Add(trans.GetChild(i).gameObject);
        }
        trans.DetachChildren();
        for (int i = 0; i < temp.Count; i++)
        {
            Destroy(temp[i]);
        }
    }

    private static string ImagePath = "Image/";
    public static void ImageChange(Image image, string name)
    {
        image.sprite = Resources.Load(ImagePath + name, typeof(Sprite)) as Sprite;
    }
    private static string ImagePath2 = "SpriteImage/";
    public static void ImageChange(SpriteRenderer spRender, string name)
    {
        Sprite spriteB = Resources.Load<Sprite>(ImagePath2 + name);
        spRender.sprite = spriteB;
    }

    void changeSpriteByImage()
    {
        GameObject platform = null;
        //
        Texture2D Tex = Resources.Load("enter") as Texture2D;
        SpriteRenderer spr = platform.GetComponent<SpriteRenderer>();
        Sprite spriteA = Sprite.Create(Tex, spr.sprite.textureRect, new Vector2(0.5f, 0.5f));
        platform.GetComponent<SpriteRenderer>().sprite = spriteA;
    }
}
