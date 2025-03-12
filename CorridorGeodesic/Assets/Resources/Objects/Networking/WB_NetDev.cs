//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WB_NetDev : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public TMP_InputField saddress;
    public TMP_InputField caddress;
    public TMP_InputField port;
    public Tugboat transport;
    public NetworkManager serv;


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool serverStarted;
    public bool clientStarted;
    public TMP_Text serverStartText;
    public TMP_Text clientStartText;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void SetData()
    {
        if (!transport) return;
        ushort _port = 0;
        ushort.TryParse(port.text, out _port);
        transport.SetPort(_port);
        transport.SetServerBindAddress(saddress.text, IPAddressType.IPv4);
        transport.SetClientAddress(caddress.text);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void ToggleServer()
    {
        SetData();
        if (!serv) return;
        if (!serverStarted)
        {
            serv.ServerManager.StartConnection();
            serverStartText.text = "Stop Server";
            serverStarted = true;
        }
        else
        {
            serv.ServerManager.StopConnection(true);
            serverStartText.text = "Stop Server";
            serverStarted = false;
        }
    }
    public void ToggleClient()
    {
        SetData();
        if (!serv) return;
        if (!clientStarted)
        {
            serv.ClientManager.StartConnection();
            clientStartText.text = "Disconnect";
            clientStarted = true;
        }
        else
        {
            serv.ClientManager.StopConnection();
            clientStartText.text = "Join Client";
            clientStarted = false;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
