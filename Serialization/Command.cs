using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public enum Command
    {
        Authorization,
        Registration,
        GetUsersList,
        AddToFriend,
        DelFromFriend,
        GetFriendList,
        SendMessage,
        LoadImage,
    }
}
