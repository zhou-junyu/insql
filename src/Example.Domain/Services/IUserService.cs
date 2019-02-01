using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Domain.Services
{
    public interface IUserService
    {
        IEnumerable<UserInfo> GetUserList(string userName,Gender? userGender);

        void InsertUser(UserInfo info);

        void InsertUserSelective(UserInfo info);

        void UpdateUser(UserInfo info);

        void UpdateUserSelective(UserInfo info);

        void DeleteUser(int userId);
    }
}
