import {UploadFile} from "antd";

export default interface ICreateProduct {
    name: string
    images: UploadFile[]
    description: string
    price: number
    categoryId: number
}

