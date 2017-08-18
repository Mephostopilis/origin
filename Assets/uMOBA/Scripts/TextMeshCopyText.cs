// Copy text from one text mesh to another (for shadows etc.)
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextMeshCopyText : MonoBehaviour {
   [SerializeField] TextMesh target;
   
    void Update () {
       GetComponent<TextMesh>().text = target.text;
    }
}
