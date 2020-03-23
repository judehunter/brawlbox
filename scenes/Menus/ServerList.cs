using Godot;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

class ServerInfo
{
	public string name;
	public string IP;
	public int port;
	public ulong lastSeen;
}

public class ServerList : ItemList
{
	Timer cleanUpTimer = new Timer();
	PacketPeerUDP socket = new PacketPeerUDP();
	int PORT = 3111;
	Dictionary<string,  ServerInfo> knownServers;

	[Export]
	int server_cleanup_threshold = 3;

	public ServerList()
	{
		cleanUpTimer.WaitTime = server_cleanup_threshold;
		cleanUpTimer.OneShot = false;
		cleanUpTimer.Autostart = true;
		cleanUpTimer.Connect("timeout", this, nameof(cleanUp));
	}
	
	public override void _Ready()
	{

		if (socket.Listen(PORT) != Error.Ok)
		{
			GD.Print("LAN service: Error listening on port: " + PORT);
		} else
		{
			GD.Print("LAN service: Listening on port: " + PORT);
		}
	}

	public override void _Process(float delta)
	{
		if(socket.GetAvailablePacketCount() > 0)
		{
			string serverIP = socket.GetPacketIp();
			int serverPort = socket.GetPacketPort();
			byte[] packet = socket.GetPacket();

			if(serverIP != "" && serverPort > 0)
			{
				if (!knownServers.ContainsKey(serverIP))
				{
					Stream stream = new MemoryStream(packet);
					BinaryFormatter b = new BinaryFormatter();
					ServerInfo data = (ServerInfo) b.Deserialize(stream);
					data.IP = serverIP;
					data.port = serverPort;
					data.lastSeen = OS.GetUnixTime();
					GD.Print("New server found: {0} - {1}:{2}", data.name, data.IP, data.port);
					generateServerlist();

				}
			} else
			{
				knownServers[serverIP].lastSeen = OS.GetUnixTime();
			}
		}
	}

	private void generateServerlist()
	{

	}

	private void cleanUp()
	{
		ulong now = OS.GetUnixTime();
		foreach (KeyValuePair<string, ServerInfo> server in knownServers)
		{
			if (now - server.Value.lastSeen > (ulong)server_cleanup_threshold)
			{
				knownServers.Remove(server.Key);
				GD.Print("Server cleaned up: {0}", server.Key);
			}
		}
	}

	public override void _ExitTree()
	{
		socket.Close();
	}

	private void _on_Duo_pressed()
	{
		GetParent<CanvasItem>().Visible = true;
		GD.Print("DUO pressed");
		AddItem("Server1");
	}
}



