import queryString from "query-string";
import {NavigateFunction} from "react-router-dom";

export function navigateAndSaveParams(navigate: NavigateFunction, url: string) {
    const queryParams = queryString.parse(window.location.search);

    navigate(`${url}?${queryString.stringify(queryParams)}`);
}

export function navigateAndSetParams(navigate: NavigateFunction, url: string, params: any) {
    const queryParams = queryString.parse(window.location.search);

    const newParams = {
        ...queryParams
    };

    for (const p in params) {
        if (params[p])
            newParams[p] = params[p];
        else
            delete newParams[p];
    }

    navigate(`${url}?${queryString.stringify(newParams)}`);
    window.location.reload();
}