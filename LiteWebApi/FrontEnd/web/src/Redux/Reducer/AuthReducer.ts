import IUserData from "../../Interfaces/Token/IUserData.tsx";

export enum AuthReducerActionType {
    LOGIN_USER = "AUTH_LOGIN_USER",
    LOGOUT_USER = "LOGOUT_USER"
}

export interface IAuthReducerState {
    isAuth: boolean,
    user: IUserData | null
}

const initState: IAuthReducerState = {
    isAuth: false,
    user: null
}

const AuthReducer = (state = initState, action: any): IAuthReducerState => {
    const user = action.payload;

    switch (action.type) {
        case AuthReducerActionType.LOGIN_USER: {
            return {
                isAuth: true,
                user: {
                    email: user.email,
                    name: user.name,
                    photo: user.photo,
                    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
                    hasRole(role: string): boolean {
                        return this["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"].includes(role);
                    }
                } as IUserData
            };
        }
        case AuthReducerActionType.LOGOUT_USER: {
            return {
                isAuth: false,
                user: action.payload
            };
        }
        default:
            return state;
    }
}

export default AuthReducer;