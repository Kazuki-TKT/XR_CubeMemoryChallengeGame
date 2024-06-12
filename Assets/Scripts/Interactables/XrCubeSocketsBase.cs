using System.Collections.Generic;
using UnityEngine;

public class XrCubeSocketsBase : MonoBehaviour
{
    [SerializeField] List<XrCubeSocketInteractable> cubeSocketsBase = new List<XrCubeSocketInteractable>(); // XrCubeSocketInteractable‚ÌƒŠƒXƒg
    public List<XrCubeSocketInteractable> GetCubeSocketsBase => cubeSocketsBase;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            XrCubeSocketInteractable xrCubeSocket = child.GetComponentInChildren<XrCubeSocketInteractable>();
            if (xrCubeSocket != null)
            {
                cubeSocketsBase.Add(xrCubeSocket);
            }
        }
    }
}
