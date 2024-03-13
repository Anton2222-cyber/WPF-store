import IOrderStatus from "./IOrderStatus.ts";
import IPostOffice from "./IPostOffice.ts";
import IOrderedProduct from "./IOrderedProduct.ts";
import ICustomer from "./ICustomer.ts";

export default interface ICustomerOrder {
    id: number;
    timeOfCreation: Date;
    status: IOrderStatus;
    postOffice: IPostOffice;
    orderedProducts: IOrderedProduct[],
    customer: ICustomer;
}