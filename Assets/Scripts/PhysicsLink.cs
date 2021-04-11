using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PhysicsLink : NetworkBehaviour
{
    public Rigidbody rb;

    [SyncVar]
    public Vector3 Velocity;
    [SyncVar]
    public Quaternion Rotation;
    [SyncVar]
    public Vector3 Position;
    [SyncVar]
    public Vector3 AngularVelocity;

    void Update()
    {
        if (GetComponent<NetworkIdentity>().isServer)
        {
            Position = rb.position;
            Rotation = rb.rotation;
            Velocity = rb.velocity;
            AngularVelocity = rb.angularVelocity;
            rb.position = Position;
            rb.rotation = Rotation;
            rb.velocity = Velocity;
            rb.angularVelocity = AngularVelocity;
        }
        if (GetComponent<NetworkIdentity>().isClient)
        {
            rb.position = Position + Velocity * (float)NetworkTime.rtt;
            rb.rotation = Rotation * Quaternion.Euler(AngularVelocity * (float)NetworkTime.rtt);
            rb.velocity = Velocity;
            rb.angularVelocity = AngularVelocity;
        }
    }

    [Command]
    public void CmdResetPose()
    {
        rb.position = new Vector3(0, 1, 0);
        rb.velocity = new Vector3();
    }

    public void ApplyForce(Vector3 force, ForceMode FMode)
    {
        rb.AddForce(force, FMode);
        CmdApplyForce(force, FMode);
    }

    [Command]
    public void CmdApplyForce(Vector3 force, ForceMode FMode)
    {
        rb.AddForce(force, FMode);
    }
}
