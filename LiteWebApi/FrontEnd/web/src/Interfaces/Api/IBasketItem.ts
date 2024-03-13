import IProduct from "./IProduct.ts";

export default interface IBasketItem {
    id: number;
    product: IProduct,
    quantity: number
}