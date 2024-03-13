import IProduct from "./IProduct.ts";

export default interface IFilteredProducts {
    filteredProducts: IProduct[],
    availableQuantity: number
}

