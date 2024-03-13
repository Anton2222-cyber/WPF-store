import {combineReducers} from "redux";
import {configureStore} from "@reduxjs/toolkit";
import AuthReducer from "../Reducer/AuthReducer.ts";
import thunk from "redux-thunk";

export const rootReducer = combineReducers({
    auth: AuthReducer
});

export const store = configureStore({
    reducer: rootReducer,
    devTools: true,
    middleware: [thunk]
});

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch