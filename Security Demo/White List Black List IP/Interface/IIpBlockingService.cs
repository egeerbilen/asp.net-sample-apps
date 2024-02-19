using System.Net;

namespace White_List_Black_List_IP.Interface
{
    public interface IIpBlockingService
    {
        bool IsBlocked(IPAddress ipAddress);
    }
}
