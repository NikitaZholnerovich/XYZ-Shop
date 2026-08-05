using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZ_shop.Application.Abstractions.Hubs
{
    public interface INotificationHub
    {
        Task NewGameAdded(string gameName, string urlCover);
    }
}
