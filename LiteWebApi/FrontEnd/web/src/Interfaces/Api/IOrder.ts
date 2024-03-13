import IOrderStatus from "./IOrderStatus.ts";
import IPostOffice from "./IPostOffice.ts";
import IOrderedProduct from "./IOrderedProduct.ts";

export default interface IOrder {
    id: number;
    timeOfCreation: Date;
    status: IOrderStatus;
    postOffice: IPostOffice;
    orderedProducts: IOrderedProduct[]
}