using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use

        [SerializeField]
        private WheelCollider[] m_WheelColliders = new WheelCollider[4]; //
        [SerializeField]
        private Vector3 m_CentreOfMassOffset;//

        private void Awake()
        {
            //m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

            // get the car controller
            m_Car = GetComponent<CarController>();
        }

        //-----------------------------------
        private void FixedUpdate()
        {
            //m_Car.Move(0, 0, 0, 0);
            //for (int i = 0; i < 4; i++)
            //{
            //    m_WheelColliders[i].motorTorque = 1000;
            //}
        }

        void Update()
        {
            
        }
        //-----------------------------------------

    }

}



/* original code */
//        private void FixedUpdate()
//        {
//            // pass the input to the car!
//            float h = CrossPlatformInputManager.GetAxis("Horizontal");
//            float v = CrossPlatformInputManager.GetAxis("Vertical");
//#if !MOBILE_INPUT
//            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
//            m_Car.Move(h, v, v, handbrake);

//#else

//            m_Car.Move(h, v, v, 0f);
//#endif
//        }