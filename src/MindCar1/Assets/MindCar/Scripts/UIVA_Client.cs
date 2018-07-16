///-----------------------------------------------------------------------------------
///-----------------------------------------------------------------------------------
/// UIVA (Unity Indie VRPN Adapter) Unity
/// 
/// Function: The client side of UIVA living in Unity as a DLL file.
///           Unity creates a UIVA class and calls its GetXXXData(out X, out X) functions
///           to get the latest data from the sensor devices.
/// 
/// About UIVA:
/// 
///   UIVA is a middle-ware between VRPN and Unity. It enables games developed by Unity3D INDIE
///   to be controlled by devices powered by VRPN. It has a client and a server simultaneously.
///   For VRPN, UIVA is its client which implements several callback functions to receive the 
///   latest data from the devices. For Unity, UIVA is a server that stores the latest sensor
///   data which allows it to query. The framework is shown as below:
///   
///        ~~~Sensor~~~      ~~~VRPN~~~      ~~~~~~~~~~~~UIVA~~~~~~~~~~~~~~~    ~~~Unity3D~~~     
///        
///   Kinect-----(FAAST)---->|--------|    |--------|--------|    |---------|
///    Wii ----(VRPN Wii)--->|        |    |        |        |    |         |--->Object transform
///   BPack --(VRPN BPack)-->|  VRPN  |    |  VRPN  | Unity  |    |  Unity  |
///           ...            |        |===>|  .net  | socket |===>|  socket |--->GUI
///           ...            | server |    |        |        |    |         |
///           ...            |        |    | client | server |    |  client |--->etc. of Unity3D
///           ...            |--------|    |--------|--------|    |---------|
///    
/// Special note: 
///
///      The VRPNWrapper implemented by the AR lab of Georgia Institute of Technology offers
///   a easier to use wrapper of VRPN to be used as a plugin in Unity3D Pro. If you can afford 
///   the Pro version of Unity. I suggest you to use VRPNWrapper. Their website is:
///           https://research.cc.gatech.edu/uart/content/about
///   They also implemented a ARToolkit wrapper which enables AR application in Unity. 
///   Check out their UART project, it is awesome!
///    
/// Author: 
/// 
/// Jia Wang (wangjia@wpi.edu)
/// Human Interaction in Virtual Enviroments (HIVE) Lab
/// Department of Computer Science
/// Worcester Polytechnic Institute
/// 
/// History: (1.0) 01/11/2011  by  Jia Wang
///
/// Acknowledge: Thanks to Chris VanderKnyff for the .NET version of VRPN
///                     to UNC for the awesome VRPN
///                     to Unity3D team for the wonderful engine
///              
///              and above all, special thanks to 
///                 Prof. Robert W. Lindeman (http://www.wpi.edu/~gogo) 
///              for the best academic advice.
///              
///-----------------------------------------------------------------------------------
///-----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

/// <summary>
/// UIVA_Client class
/// </summary>
public class UIVA_Client
{
    Socket socClient;       //Socket deal with communication
    byte[] recBuffer = new byte[256];   //Receive buffer
    string recStr = "";                 //Deciphered receive 

    /// <summary>
    /// Connect and test connection
    /// </summary>
    /// <param name="serverIP">The IP address of the server, 
    /// should be the local IP if used as Unity interface</param>
    public UIVA_Client(string serverIP)
    {
        // If the UIVA server is in the local machine,
        // find its IP address and connect automatically
        if(serverIP == "localhost")
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName()); 
            foreach (IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily.ToString() == "InterNetwork")
                {
                    serverIP = ip.ToString();
                }
            }
        }

        try
        {
            //Create a client socket
            socClient = new Socket(AddressFamily.InterNetwork,
                                SocketType.Stream, ProtocolType.IP);
            //Parse the IP address string into an IPAddress object
            IPAddress serverAddr = IPAddress.Parse(serverIP);
            //Port: 8881
            IPEndPoint serverMachine = new IPEndPoint(serverAddr, 8881);
            //Connect
            socClient.Connect(serverMachine);
            //Send a confirmation message
            SendMessage("Ready?\n");
            ReceiveMessage();
            if (recStr != "Ready!")
            {
                throw new Exception("Not ready?");
            }
        }
        catch (Exception e)
        {
            Exception initError = new Exception(e.ToString()
                                    + "\nClient failed to connect to server. Is your IP correct?"
                                    + "Is your UIVA working\n");
            throw initError;
        }
    }

    /// <summary>
    /// Send a message to the server
    /// </summary>
    /// <param name="msg">message content, end with a '\n'</param>
    private void SendMessage(string msg)
    {
        try
        {
            //Encode a message
            byte[] sendBuffer = Encoding.ASCII.GetBytes(msg);
            socClient.Send(sendBuffer);
        }
        catch (Exception e)
        {
            Exception sendError = new Exception(e.ToString() + "Client failed to send message.\n");
            throw sendError;
        }
    }

    /// <summary>
    /// Receive message from the server, decode and store in "recStr" variable
    /// </summary>
    private void ReceiveMessage()
    {
        try
        {
            socClient.Receive(recBuffer);
            recStr = Encoding.Default.GetString(recBuffer);
            //Remove the tailing '\0's after the '\n' token, caused by the buffer size
            int ixEnd = recStr.IndexOf('\n');
            recStr = recStr.Remove(ixEnd);
        }
        catch (Exception e)
        {
            Exception recError = new Exception(e.ToString()
                                    + "Client failed to receive message.\n");
            throw recError;
        }
    }

    /// <summary>
    /// Receive the data from the UIVA Server, actually from Epoc device     
    public void GetEpocData(int which, out double channel_val, out double num_channel, out DateTime anaTS, out string butt)
    {
        SendMessage(String.Format("Epoc?{0}?\n", which));
        ReceiveMessage();
        try
        {
            string[] sections = recStr.Split(new char[] { ',' });    //Parse
            //Skip "Epoc,,"
            channel_val = System.Convert.ToDouble(sections[2]);  //Get the values of each channels 
            num_channel = System.Convert.ToDouble(sections[3]);  //Get the number of channels
            //Skip the analog timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
            anaTS = DateTime.Parse(sections[4]);

            butt = sections[5];   // <= 여기에 버튼 a, z 그리고 버튼타임스탬프까지 하나의 string으로 다 들어온다.
            //buttZ = sections[6];
            //Skip the button timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
            //buttTS = DateTime.Parse(sections[7]);
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
        }
    }

    public void GetEpocData(out double channel_val, out double num_channel, out DateTime anaTS, out string butt)
    {
        GetEpocData(1, out channel_val, out num_channel, out anaTS, out butt);
    }
    /// <summary>
    

    /// <summary>
    /// Communicate to UIVA in reverse    
    public void Press(int num)
    {
        SendMessage(String.Format("Press?{0}?\n", num));
    }

    public void Release(int num)
    {
        SendMessage(String.Format("Release?{0}?\n", num));
    }
    /// </summary>


    /// <summary>
    /// Disconnect from UIVA
    /// </summary>
    public void Disconnect()
    {
        SendMessage("Bye?\n");      //Send disconnect request
    }
}

