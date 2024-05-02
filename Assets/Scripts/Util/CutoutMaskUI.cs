using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskUI : Image
{
    [SerializeField] private Material cutoutMaterial;

    private void Update()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        // Check if the material needs to be updated
        if (material == null || !material.HasProperty("_StencilComp"))
        {
            // Assign the cutout material to the image
            if (cutoutMaterial != null)
            {
                material = new Material(cutoutMaterial); // Create a new instance to avoid modifying the original material
                material.SetInt("_StencilComp", (int)CompareFunction.Equal); // Ensure correct stencil comparison function
                material.SetInt("_Stencil", (int)StencilOp.Replace); // Change stencil operation to replace
            }
            else
            {
                Debug.LogWarning("Cutout material is not assigned to CutoutMaskUI script!");
            }
        }
    }

    ////protected override void Awake()
    ////{
    ////    base.Awake();
    ////    // Assign the cutout material to the image
    ////    if (cutoutMaterial != null)
    ////    {
    ////        material = cutoutMaterial;
    ////    }
    ////    else
    ////    {
    ////        Debug.LogWarning("Cutout material is not assigned to CutoutMaskUI script!");
    ////    }
    ////}
    //public override Material materialForRendering
    //{
    //    get
    //    {
    //        Material material = new Material(base.materialForRendering);
    //        material.shader = Shader.Find("UI/Default");
    //        material.SetInt("_StencilComp", (int)CompareFunction.NotEqual); 
    //        return material;
    //    }
    //}
}
