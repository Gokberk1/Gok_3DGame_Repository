using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gok3DGame.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float _speed;

        Vector2 _move, _mouseLook, _joystickLook;
        Vector3 _rotationTarget;

        [SerializeField] bool _isPc;

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }
        public void OnMouseLook(InputAction.CallbackContext context)
        {
            _mouseLook = context.ReadValue<Vector2>();
        }
        public void OnJoystickLook(InputAction.CallbackContext context)
        {
            _joystickLook = context.ReadValue<Vector2>();
        }

     

        // Update is called once per frame
        void Update()
        {
            if (_isPc)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(_mouseLook);
                
                if(Physics.Raycast(ray, out hit))
                {
                    _rotationTarget = hit.point;
                }

                MovePlayerWithAim();
            }
            else
            {
                if(_joystickLook.x == 0 && _joystickLook.y == 0)
                {
                    MovePlayer();
                }
                else
                {
                    MovePlayerWithAim();
                }
            }
        }

        public void MovePlayer()
        {
            Vector3 movement = new Vector3(_move.x, 0f, _move.y);
            if(movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.2f);
            }
            
            transform.Translate(movement * _speed * Time.deltaTime, Space.World);
        }

        public void MovePlayerWithAim()
        {
            if (_isPc)
            {
                var lookPos = _rotationTarget - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);

                Vector3 aimDirection = new Vector3(_rotationTarget.x, 0, _rotationTarget.z);
                if(aimDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
                }
            }
            else
            {
                Vector3 aimDirection = new Vector3(_joystickLook.x, 0, _joystickLook.y);
                if (aimDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.2f);
                }
            }
            Vector3 movement = new Vector3(_move.x, 0f, _move.y);
            transform.Translate(movement * _speed * Time.deltaTime, Space.World);
        }
    }
}

