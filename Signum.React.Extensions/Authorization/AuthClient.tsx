﻿import * as React from 'react'
import { Route } from 'react-router'
import { ajaxPost, ajaxGet } from '../../../Framework/Signum.React/Scripts/Services';
import { EntitySettings } from '../../../Framework/Signum.React/Scripts/Navigator'
import * as Navigator from '../../../Framework/Signum.React/Scripts/Navigator'
import { EntityOperationSettings } from '../../../Framework/Signum.React/Scripts/Operations'
import * as Operations from '../../../Framework/Signum.React/Scripts/Operations'
import { UserEntity, UserEntity_Type, UserOperation, RoleEntity_Type, PermissionSymbol } from './Signum.Entities.Authorization'
import Login from './Login/Login';

export let userTicket: boolean;
export let resetPassword: boolean;

export function startPublic(options: { routes: JSX.Element[], userTicket: boolean, resetPassword: boolean }) {
    userTicket = options.userTicket;
    resetPassword = options.resetPassword;

    options.routes.push(<Route path="auth">
        <Route path="login" getComponent={(loc, cb) => require(["./Login/Login"], (Comp) => cb(null, Comp.default))}/>
        <Route path="about" />
    </Route>);
}

export function startAdmin() {
    Navigator.addSettings(new EntitySettings(UserEntity_Type, e => new Promise(resolve => require(['./Templates/User'], resolve))));
    Navigator.addSettings(new EntitySettings(RoleEntity_Type, e => new Promise(resolve => require(['./Templates/Role'], resolve))));
}

export function currentUser(): UserEntity {
    return Navigator.currentUser as UserEntity;
}

export const CurrentUserChangedEvent = "current-user-changed";
export function setCurrentUser(user: UserEntity) {
    Navigator.currentUser = user;

    document.dispatchEvent(new Event(CurrentUserChangedEvent));
}


export function logout() {

    Api.logout().then(() => {

        setCurrentUser(null);

        onLogout();
    }).done();
}

export function onLogout() {
    Navigator.currentHistory.push("/");
}

export function onLogin() {
    Navigator.currentHistory.push("/");
}

export function isPermissionAuthorized(permission: PermissionSymbol) {
    return true;
}

export function asserPermissionAuthorized(permission: PermissionSymbol) {
    if (!isPermissionAuthorized(permission))
        throw new Error(`Permission ${permission.key} is denied`);
}


export module Api {

    export interface LoginRequest {
        userName: string;
        password: string;
        rememberMe?: boolean; 
    }

    export interface LoginResponse {
        message: string;
        userEntity: UserEntity 
    }

    export function login(loginRequest: LoginRequest): Promise<LoginResponse> {
        return ajaxPost<LoginResponse>({ url: "/api/auth/login" }, loginRequest);
    }

    export function retrieveCurrentUser(): Promise<UserEntity> {
        return ajaxGet<UserEntity>({ url: "/api/auth/currentUser", cache: "no-cache" });
    }

    export function logout(): Promise<void> {
        return ajaxPost<void>({ url: "/api/auth/logout" }, null);
    }
}



