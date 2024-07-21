using System.Linq;
using UnityEngine;

namespace Omnis
{
    [RequireComponent(typeof(Rigidbody))]
    public class ChildrenDither : MonoBehaviour
    {
        #region Interfaces
        public void Stroke(float force = 100f) => Stroke(Vector3.one, force);
        public void Stroke(Vector3 direction, float force = 100f)
        {
            transform.GetComponentsInChildren<Rigidbody>().ToList().ForEach(child => child.AddForce(force * direction.normalized));
        }
        #endregion
    }
}
