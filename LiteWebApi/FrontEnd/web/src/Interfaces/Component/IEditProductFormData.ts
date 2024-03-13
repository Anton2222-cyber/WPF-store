import {UploadFile} from "antd";

export default interface IEditProductFormData {
    name: string
    description: string
    price: number
    images: UploadFile[]
    categoryId: number
}