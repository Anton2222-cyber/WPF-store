import ICategory from "./ICategory.tsx";

export default interface IProduct {
    id: number,
    dateCreated: Date,
    name: string,
    description: string,
    price: number,
    category: ICategory,
    images: string[]
}