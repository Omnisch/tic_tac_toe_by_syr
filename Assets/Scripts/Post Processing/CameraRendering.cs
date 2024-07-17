using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Omnis
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraRendering : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private List<Camera> backgroundCameras;
        [SerializeField] private List<Camera> foregroundCameras;
        [SerializeField] private Material frameDitherMat;
        #endregion

        #region Fields
        #endregion

        #region Life Cycle
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            backgroundCameras.ForEach(camera => Graphics.Blit(camera.targetTexture, destination));
            foregroundCameras.ForEach(camera => Graphics.Blit(camera.targetTexture, destination));
        }
        #endregion
    }
}
