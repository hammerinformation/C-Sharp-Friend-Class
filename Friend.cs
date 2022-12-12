using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public interface IFriend
{
}

public class Friend
{
    private readonly List<System.Type> _friendsList = new List<System.Type>();

    public Friend AddFriend(System.Type type) 
    {
        
        _friendsList.Add(type);
        return this;
    }


    public TReturnType GetField<TFriendClass, TReturnType>(IFriend friend, string fieldName)
        where TFriendClass : IFriend
    {
        if (_friendsList.Contains(typeof(TFriendClass)))
        {
            var fieldInfos = ((TFriendClass)friend).GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
                .ToList();


            foreach (var info in fieldInfos)
            {
                if (info.Name == fieldName)
                {
                    return (TReturnType)info.GetValue(((TFriendClass)friend));
                }
            }
        }

        throw new Exception("Variable not found");
    }

    public void SetField<TFriendClass, TValue>(IFriend friend, string fieldName, TValue value)
        where TFriendClass : IFriend
    {
        if (_friendsList.Contains(typeof(TFriendClass)))
        {
            var fieldInfos = ((TFriendClass)friend).GetType().GetFields(BindingFlags.Instance | BindingFlags.Public |
                                                                        BindingFlags.Static | BindingFlags.NonPublic);
            foreach (var info in fieldInfos)
            {
                if (info.Name == fieldName)
                {
                    info.SetValue(friend, value);
                    break;
                }
                else
                {
                    throw new Exception("Variable not found");
                }
            }
        }
    }

    public TReturnType CallMethod<TFriendClass, TReturnType>(IFriend friend, string methodName,
        object[] parametersArray)
        where TFriendClass : IFriend
    {
        if (_friendsList.Contains(typeof(TFriendClass)))
        {
            if (parametersArray == null)
            {
                var m = typeof(TFriendClass).GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);


                var returnType = m?.Invoke(friend, null);

                return (TReturnType)returnType;
            }
            else
            {
                var m = typeof(TFriendClass).GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);


                var returnType = m?.Invoke(friend, parametersArray);

                return (TReturnType)returnType;
            }
        }

        throw new Exception("Method not found");
    }


    public void CallMethod<TFriendClass>(IFriend friend, string methodName,
        object[] parametersArray)
        where TFriendClass : IFriend
    {
        if (_friendsList.Contains(typeof(TFriendClass)))
        {
            if (parametersArray == null)
            {
                var m = typeof(TFriendClass).GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);


                m?.Invoke(friend, null);
                if (m != null)
                {
                    return;
                }
            }
            else
            {
                var m = typeof(TFriendClass).GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

                m?.Invoke(friend, parametersArray);
                if (m != null)
                {
                    return;
                }
            }
        }

        throw new Exception("Method not found");
    }
}