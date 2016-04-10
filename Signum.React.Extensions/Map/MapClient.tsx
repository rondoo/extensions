﻿
import * as React from 'react'
import { Route } from 'react-router'
import { Dic, classes } from '../../../Framework/Signum.React/Scripts/Globals';
import { ajaxPost, ajaxGet } from '../../../Framework/Signum.React/Scripts/Services';
import { EntitySettings } from '../../../Framework/Signum.React/Scripts/Navigator'
import * as Navigator from '../../../Framework/Signum.React/Scripts/Navigator'
import * as Finder from '../../../Framework/Signum.React/Scripts/Finder'
import { EntityOperationSettings } from '../../../Framework/Signum.React/Scripts/Operations'
import { Entity, Lite, liteKey, MList, toLite, is, EntityPack } from '../../../Framework/Signum.React/Scripts/Signum.Entities'
import * as Constructor from '../../../Framework/Signum.React/Scripts/Constructor'
import * as Operations from '../../../Framework/Signum.React/Scripts/Operations'
import * as QuickLinks from '../../../Framework/Signum.React/Scripts/QuickLinks'
import { PseudoType, QueryKey, getQueryKey, Type, isEntity, EntityData, EntityKind } from '../../../Framework/Signum.React/Scripts/Reflection'
import { TypeContext } from '../../../Framework/Signum.React/Scripts/TypeContext'
import { WidgetContext, onEmbeddedWidgets, EmbeddedWidgetPosition } from '../../../Framework/Signum.React/Scripts/Frames/Widgets'
import { FindOptions, FilterOption, FilterOperation, OrderOption, ColumnOption,
    FilterRequest, QueryRequest, Pagination, QueryTokenType, QueryToken, FilterType, SubTokensOptions, ResultTable, OrderRequest } from '../../../Framework/Signum.React/Scripts/FindOptions'
import * as AuthClient  from '../../../Extensions/Signum.React.Extensions/Authorization/AuthClient'
import { SchemaMapInfo, ClientColorProvider } from './Templates/SchemaMap'

import {  } from './Signum.Entities.Map'


export const getProviders: Array<() => Promise<ClientColorProvider[]>> = []; 

export function getAllProviders(): Promise<ClientColorProvider[]>{
    return Promise.all(getProviders.map(a => a())).then(result => result.filter(ps => !!ps).flatMap(ps => ps).filter(p => !!p));
}

export function start(options: { routes: JSX.Element[] }) {

    options.routes.push(<Route path="map">
        <Route path="types" getComponent={ (loc, cb) => require(["./View/DashboardPage"], (Comp) => cb(null, Comp.default)) } />
        <Route path="operations/:type" getComponent={ (loc, cb) => require(["./View/DashboardPage"], (Comp) => cb(null, Comp.default)) } />
    </Route>);
    
    getProviders.push(() => new Promise<ClientColorProvider[]>(resolve => {
        require(["./Templates/SchemaMapDefaultProviders"], c => resolve(c.default()));
    }));
}

export namespace API {
    export function types(): Promise<SchemaMapInfo> {
        return ajaxGet<SchemaMapInfo>({ url: "/api/map/types" });
    }

    export function operations(typeName: string): Promise<OperationMapInfo> {
        return ajaxGet<OperationMapInfo>({ url: "/api/map/operations/" + typeName });
    }

}






export interface OperationMapInfo {
    states: MapState[];
    operations: MapOperation[];
}

export interface MapOperation {
    key: string;
    niceName: string;
    count: number;
    fromStates: string[];
    toStates: string[];
}

export interface MapState {
    key: string;
    niceName: string;
    count: number;
    ignored: boolean;
    color: string;
    token: string;
}