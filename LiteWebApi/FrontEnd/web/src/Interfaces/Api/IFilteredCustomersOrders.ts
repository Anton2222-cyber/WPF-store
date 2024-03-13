import ICustomerOrder from "./ICustomerOrder.ts";

export default interface IFilteredCustomersOrders {
    filteredOrders: ICustomerOrder[],
    availableQuantity: number
}