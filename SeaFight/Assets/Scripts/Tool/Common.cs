using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class Common : MonoBehaviour
{
    public static GameObject PrefabLoad(string path)
    {
        return (GameObject)Resources.Load(path, typeof(GameObject));
    }

    public static GameObject Generate(string path, Transform parent, bool isOrgPosition = true, bool isOrgScale = true, bool isOrgRotation = true)
    {
        Object obj = Resources.Load(path);
        if (obj == null)
        {
            Debug.LogError("prefab = null,Check path:" + path);
            return null;
        }
        if (parent == null)
        {
            Debug.LogError("parent = null,Check path:" + path);
        }
        return Generate(obj as GameObject, parent);
    }

    public static GameObject Generate(GameObject prefab, Transform parent, bool isOrgPosition = true, bool isOrgScale = true, bool isOrgRotation = true)
    {
        GameObject obj = null;
        obj = Instantiate(prefab) as GameObject;
        obj.transform.SetParent(parent);
        if (isOrgPosition)
        {
            obj.transform.localPosition = Vector3.zero;
        }
        if (isOrgScale)
        {
            obj.transform.localScale = Vector3.one;
        }
        if (isOrgRotation)
        {
            obj.transform.localRotation = Quaternion.identity;
        }
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
