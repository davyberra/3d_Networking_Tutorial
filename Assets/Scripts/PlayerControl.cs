using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControl : NetworkBehaviour
{
    [SyncVar]
    public Vector3 Control;

    public Color c;

    private void Update()
    {
        if (GetComponent<NetworkIdentity>().hasAuthority)
        {
            GetComponent<Renderer>().material.color = c;
            Control = new Vector3(Input.GetAxis("Horizontal") * .2f, 0, Input.GetAxis("Vertical") * .2f);
            GetComponent<PhysicsLink>().ApplyForce(Control, ForceMode.VelocityChange);
            if (Input.GetAxis("Cancel") == 1)
            {
                GetComponent<PhysicsLink>().CmdResetPose();
            }
        }
    }
}
