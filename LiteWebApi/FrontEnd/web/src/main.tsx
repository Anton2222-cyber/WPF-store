import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import IUserData from "./Interfaces/Token/IUserData.tsx";
import {jwtDecode} from "jwt-decode";
import {store} from "./Redux/Store";
import {Provider} from "react-redux";
import {AuthReducerActionType} from "./Redux/Reducer/AuthReducer.ts";
import axios from "axios";

const token = localStorage.token;

if (token) {
    axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    const user: IUserData = jwtDecode<IUserData>(token);

    store.dispatch({
            type: AuthReducerActionType.LOGIN_USER,
            payload: {
                email: user.email,
                name: user.name,
                photo: user.photo,
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
                hasRole(role: string): boolean {
                    return this["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"].includes(role);
                }
            } as IUserData
        }
    );
}

ReactDOM.createRoot(document.getElementById('root')!).render(
    <Provider store={store}>
        <React.StrictMode>
            <App/>
        </React.StrictMode>,
    </Provider>
)