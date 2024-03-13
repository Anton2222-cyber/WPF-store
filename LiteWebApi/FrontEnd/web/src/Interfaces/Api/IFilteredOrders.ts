import IOrder from "./IOrder.ts";

export default interface IFilteredOrders {
    filteredOrders: IOrder[],
    availableQuantity: number
}
