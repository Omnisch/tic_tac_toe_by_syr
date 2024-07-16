using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis
{
    [RequireComponent(typeof(Camera))]
    public class FrameDither : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Material frameDitherMat;
        #endregion

        #region Fields
        private Camera selfCamera;
        #endregion

        #region Life Cycle
        private void Start()
        {
            selfCamera = GetComponent<Camera>();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, frameDitherMat);
        }
        #endregion
    }
}
