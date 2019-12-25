namespace Krypton.Network
{
    public static class TcpFactory
    {
        public static Systems.TcpClient CreateClient(string ip, int port)
            => new Systems.TcpClient(ip, port);

        public static Systems.TcpServer CreateServer(string ip, int port)
            => new Systems.TcpServer(ip, port);
    }
}
