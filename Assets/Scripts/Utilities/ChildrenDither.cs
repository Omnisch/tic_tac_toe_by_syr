using System.Linq;
using UnityEngine;

namespace Omnis
{
    [RequireComponent(typeof(Rigidbody))]
    public class ChildrenDither : MonoBehaviour
    {
        #region Interfaces
        public void Stroke(float force)
        {
            transform.GetComponentsInChildren<Rigidbody>().ToList().ForEach(child => child.AddForce(force * new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized));
        }
        #endregion
    }
}
