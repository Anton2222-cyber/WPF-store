import IProduct from "./IProduct.ts";

export default interface IOrderedProduct {
    id: number
    product: IProduct
    unitPrice: number
    quantity: number
}