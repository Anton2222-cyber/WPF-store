import axios from 'axios';
import IToken from '../Interfaces/Api/IToken.ts';
import ICategoryFilter from '../Interfaces/Api/ICategoryFilter.tsx';
import ICategory from '../Interfaces/Api/ICategory.tsx';
import IFilteredCategories from '../Interfaces/Api/IFilteredCategories.tsx';
import IProduct from '../Interfaces/Api/IProduct.ts';
import IBasketItem from "../Interfaces/Api/IBasketItem.ts";
import IArea from '../Interfaces/Api/IArea.ts';
import ISettlement from "../Interfaces/Api/ISettlement.ts";
import IPostOffice from "../Interfaces/Api/IPostOffice.ts";
import IBasketTotalPrice from "../Interfaces/Api/IBasketTotalPrice.ts";
import IProductFilter from "../Interfaces/Api/IProductFilter.ts";
import IOrderFilter from "../Interfaces/Api/IOrderFilter.ts";
import IFilteredCustomersOrders from "../Interfaces/Api/IFilteredCustomersOrders.ts";
import IFilteredOrders from "../Interfaces/Api/IFilteredOrders.ts";
import IFilteredProducts from "../Interfaces/Api/IFilteredProducts.ts";

export const apiUrl = "http://localhost:5114";
export const imagesDir = apiUrl + "/Data/images/";

const imagesControllerUrl = apiUrl + "/api/Images/";
const categoryControllerUrl = apiUrl + "/api/Categories/";
const productsControllerUrl = apiUrl + "/api/Products/";
const accountControllerUrl = apiUrl + "/api/Account/";
const basketControllerUrl = apiUrl + "/api/Basket/";
const areasControllerUrl = apiUrl + "/api/Areas/";
const settlementsControllerUrl = apiUrl + "/api/Settlements/";
const postOfficeControllerUrl = apiUrl + "/api/PostOffice/";
const orderControllerUrl = apiUrl + "/api/Order/";

export const getImageAsync = async (name: string): Promise<Blob> => {
    const response = await axios.get<Blob>(`${imagesControllerUrl}Get/${name}`, {responseType: 'blob'});
    return response.data;
}

export const getCategoriesAsync = async (): Promise<ICategory[]> => {
    const response = await axios.get<ICategory[]>(categoryControllerUrl + "GetAll");
    return response.data;
}

export const getFilteredCategoriesAsync = async (filter: ICategoryFilter): Promise<IFilteredCategories> => {
    const response = await axios.get<IFilteredCategories>(categoryControllerUrl + "GetFiltered", {
        params: filter
    });
    return response.data;
}

export const createCategoryAsync = async (form: FormData) => {
    return await axios.post(categoryControllerUrl + "Create", form, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

export const editCategoryAsync = async (form: FormData) => {
    return await axios.put(categoryControllerUrl + "Update", form, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

export const deleteCategoryAsync = async (id: number) => {
    return await axios.delete(categoryControllerUrl + `Delete/${id}`);
}


export const getFilteredProductsAsync = async (filter: IProductFilter): Promise<IFilteredProducts> => {
    const response = await axios.get<IFilteredProducts>(productsControllerUrl + `GetFiltered`, {
        params: filter
    });
    return response.data;
}

export const getProductByIdAsync = async (id: number): Promise<IProduct> => {
    const response = await axios.get<IProduct>(productsControllerUrl + `GetById/${id}`);
    return response.data;
}

export const createProductAsync = async (form: FormData) => {
    return await axios.post(`${productsControllerUrl}Create`, form, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

export const editProductAsync = async (form: FormData) => {
    return await axios.put(`${productsControllerUrl}Put`, form);
}


export const addToBasketAsync = async (productId: number) => {
    return await axios.post(basketControllerUrl + `Create/${productId}`);
}

export const getBasketItemsAsync = async (): Promise<IBasketItem[]> => {
    const response = await axios.get<IBasketItem[]>(basketControllerUrl + "Get");
    return response.data;
}

export const setQuantityAsync = async (productId: number, quantity: number) => {
    return await axios.patch(`${basketControllerUrl}SetQuantity/${productId} ${quantity}`);
}

export const deleteBasketProductAsync = async (productId: number) => {
    return await axios.delete(`${basketControllerUrl}Delete/${productId}`);
}
export const getBasketTotalPriceAsync = async (): Promise<IBasketTotalPrice> => {
    const response = await axios.get<IBasketTotalPrice>(basketControllerUrl + "GetTotalPrice");
    return response.data;
}


export const getAreasAsync = async (): Promise<IArea[]> => {
    const response = await axios.get<IArea[]>(`${areasControllerUrl}GetAll`);
    return response.data;
}

export const getSettlementsAsync = async (areaId: number): Promise<ISettlement[]> => {
    const response = await axios.get<ISettlement[]>(`${settlementsControllerUrl}GetByAreaId/${areaId}`);
    return response.data;
}

export const getPostOfficesAsync = async (settlementId: number): Promise<IPostOffice[]> => {
    const response = await axios.get<IPostOffice[]>(`${postOfficeControllerUrl}GetBySettlementId/${settlementId}`);
    return response.data;
}

export const orderAsync = async (form: FormData) => {
    return await axios.post(orderControllerUrl + "Order", form, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

export const getOrdersFilteredAsync = async (filter: IOrderFilter): Promise<IFilteredOrders> => {
    const response = await axios.get<IFilteredOrders>(orderControllerUrl + `GetFiltered`, {
        params: filter
    });
    return response.data;
}
export const getCustomersOrdersFilteredAsync = async (filter: IOrderFilter): Promise<IFilteredCustomersOrders> => {
    const response = await axios.get<IFilteredCustomersOrders>(orderControllerUrl + `GetCustomersOrdersFiltered`, {
        params: filter
    });
    return response.data;
}


export const registrationAsync = async (form: FormData): Promise<IToken> => {
    return (await axios.post(accountControllerUrl + "Registration", form, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    })).data;
}

export const loginAsync = async (form: FormData): Promise<IToken> => {
    return (await axios.post(accountControllerUrl + "Login", form, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    })).data;
}