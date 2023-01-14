using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ColonySim
{
    public class FollowMouse : MonoBehaviour
    {
        private Vector3 mousePos;
        private Vector3 mouseVector;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            mouseVector = ray.GetPoint(-ray.origin.z / ray.direction.z);

            transform.position = Vector3.Lerp(transform.position, mouseVector, Time.deltaTime * 5f);

            Vector3 dir = (mouseVector - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(new Vector3(0, 0, angle)), transform.lossyScale);

            Vector3 forward;
            forward.x = matrix.m02;
            forward.y = matrix.m12;
            forward.z = matrix.m22;

            Vector3 upwards;
            upwards.x = matrix.m01;
            upwards.y = matrix.m11;
            upwards.z = matrix.m21;

            transform.rotation = Quaternion.LookRotation(forward, upwards);
        }

        private void OnDrawGizmos()
        {
            Vector3 dir = (mouseVector - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(new Vector3(0,0,angle)),transform.lossyScale);
            Gizmos.matrix = matrix;
            Vector3 cube = new Vector3(1.5f, 0.1f, 0.1f);
            Gizmos.DrawCube(Vector3.zero, cube);

        }
    }
}
