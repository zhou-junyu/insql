using Insql.Tests.Domain.Models;
using System.Collections.Generic;

namespace Insql.Tests.Domain.Services
{
    public interface IUserService
    {
        IEnumerable<UserInfo> GetUserList(string userName, UserGender? userGender);

        UserInfo GetUser(int userId);

        void InsertUser(UserInfo info);

        void InsertUserSelective(UserInfo info);

        void UpdateUser(UserInfo info);

        void UpdateUserSelective(UserInfo info);

        void DeleteUser(int userId);
    }
}
