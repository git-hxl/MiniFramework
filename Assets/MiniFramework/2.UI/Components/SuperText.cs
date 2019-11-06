using System.Text.RegularExpressions;
using UnityEngine;
using MiniFramework;
using System.Collections.Generic;

[RequireComponent(typeof(TextMesh))]
public class SuperText : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private TextMesh textMesh;
   // private string emoji = "<quad material=1 size=35 x=0 y=0 width=1 height=1 />";
    private static readonly Regex regex = new Regex(@"<(.+?)>", RegexOptions.Singleline);
    // Use this for initialization
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        textMesh = GetComponent<TextMesh>();
        foreach (Match item in regex.Matches(textMesh.text))
        {
            int name = int.Parse(item.Groups[1].Value);

            string newName = "<quad material=" + name + " size=35 x=0 y=0 width=1 height=1 />";

            Material mat = new Material(Shader.Find("UI/Unlit/Transparent"));
            mat.SetTexture("_MainTex", ResourceManager.Instance.Load<Texture>("贴吧表情/" + name));

            List<Material> mats = new List<Material>(meshRenderer.materials);
            mats.Add(mat);
            meshRenderer.materials = mats.ToArray();

            textMesh.text = textMesh.text.Replace(item.Value, newName);
        }
    }
}
