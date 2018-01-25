using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Core.Users
{
    public static class UserMgr
    {
        private static Dictionary<string, LoginUser> S2Users;
        private static Dictionary<string, LoginUser> N2Users;

        static UserMgr()
        {
            S2Users = new Dictionary<string, LoginUser>();
            N2Users = new Dictionary<string, LoginUser>();
        }

        /// <summary>
        /// 添加登录用户
        /// </summary>
        /// <param name="user"></param>
        public static void AddUser(LoginUser user)
        {
            if (S2Users.ContainsKey(user.SessionId))
            {
                throw new Exception("已经有这个用户了");
            }
            else
            {
                S2Users.Add(user.SessionId, user);
            }

            if (N2Users.ContainsKey(user.Name))
            {
                throw new Exception("已经有这个用户了");
            }
            else
            {
                N2Users.Add(user.Name, user);
            }
        }

        /// <summary>
        /// sessionid 获取用户
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static LoginUser GetUserBySId(string sid)
        {
            if (S2Users.ContainsKey(sid))
            {
                return S2Users[sid];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过名字获取用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static LoginUser GetUserByName(string name)
        {
            if (N2Users.ContainsKey(name))
            {
                return N2Users[name];
            }
            else
            {
                return null;
            }
        }
    }
}
