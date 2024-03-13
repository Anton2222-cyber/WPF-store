export default interface IProductFilter {
    categoryId?: number,
    name?: number,
    description?: string,
    minPrice?: number,
    maxPrice?: number,

    offset?: number,
    limit?: number
}

