﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Signum.Entities.Authorization;
using Signum.Entities;
using Signum.Utilities.DataStructures;
using Signum.Entities.Basics;

namespace Signum.Services
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ILoginServer
    {
        [OperationContract, NetDataContract]
        void Login(string username, string passwordHash);

        [OperationContract, NetDataContract]
        void LoginChagePassword(string username, string passwordHash, string newPasswordHash);

        [OperationContract, NetDataContract]
        void ChagePassword(Lite<UserDN> user, string passwordHash, string newPasswordHash);

        [OperationContract, NetDataContract]
        UserDN GetCurrentUser();

        [OperationContract, NetDataContract]
        string PasswordNearExpired();

        [OperationContract, NetDataContract]
        byte[] DownloadAuthRules();
    }

    [ServiceContract]
    public interface ITypeAuthServer
    {
        [OperationContract, NetDataContract]
        TypeRulePack GetTypesRules(Lite<RoleDN> role);

        [OperationContract, NetDataContract]
        void SetTypesRules(TypeRulePack rules);

        [OperationContract, NetDataContract]
        DefaultDictionary<Type, TypeAllowedAndConditions> AuthorizedTypes();

        [OperationContract, NetDataContract]
        bool IsAllowedForInUserInterface(Lite<IIdentifiable> lite, TypeAllowedBasic allowed);
    }

    [ServiceContract]
    public interface IPermissionAuthServer
    {
        [OperationContract, NetDataContract]
        PermissionRulePack GetPermissionRules(Lite<RoleDN> role);

        [OperationContract, NetDataContract]
        void SetPermissionRules(PermissionRulePack rules);

        [OperationContract, NetDataContract]
        DefaultDictionary<PermissionSymbol, bool> PermissionRules();
    }

    [ServiceContract]
    public interface IPropertyAuthServer
    {
        [OperationContract, NetDataContract]
        PropertyRulePack GetPropertyRules(Lite<RoleDN> role, TypeDN typeDN);

        [OperationContract, NetDataContract]
        void SetPropertyRules(PropertyRulePack rules);

        [OperationContract, NetDataContract]
        Dictionary<PropertyRoute, PropertyAllowed> OverridenProperties();
    }

    [ServiceContract]
    public interface IQueryAuthServer
    {
        [OperationContract, NetDataContract]
        QueryRulePack GetQueryRules(Lite<RoleDN> role, TypeDN typeDN);

        [OperationContract, NetDataContract]
        void SetQueryRules(QueryRulePack rules);

        [OperationContract, NetDataContract]
        HashSet<object> AllowedQueries();
    }

    [ServiceContract]
    public interface IOperationAuthServer
    {
        [OperationContract, NetDataContract]
        OperationRulePack GetOperationRules(Lite<RoleDN> role, TypeDN typeDN);

        [OperationContract, NetDataContract]
        void SetOperationRules(OperationRulePack rules);

        [OperationContract, NetDataContract]
        Dictionary<OperationSymbol, OperationAllowed> AllowedOperations();
    }
}
