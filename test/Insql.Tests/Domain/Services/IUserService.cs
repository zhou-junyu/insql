using Insql.Tests.Domain.Models;
using System.Collections.Generic;

namespace Insql.Tests.Domain.Services
{
    public interface IUserService
    {
        IEnumerable<UserInfo> GetUserList(string userName, UserGender? userGender);

        IEnumerable<UserInfo> GetUserInList(string[] userNames);

        UserInfo GetUser(int userId);

        void InsertUser(UserInfo info);

        void InsertUserList(IEnumerable<UserInfo> infoList);

        void InsertUserSelective(UserInfo info);

        void UpdateUser(UserInfo info);

        void UpdateUserSelective(UserInfo info);

        void DeleteUser(int userId);
    }
}
